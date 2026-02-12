import { Component, OnInit, TemplateRef, ViewChild } from "@angular/core";
import { CommonService } from "src/app/services/common.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { TransactionViewModel } from "src/app/view-models/payroll-transaction.viewmodel";
import { BaseComponent } from "../base.component";
import { DatePipe } from "@angular/common";
import { TransactionService } from "src/app/services/transaction.service";
import { GeneratePayrollTransactionSM } from "src/app/service-models/app/v1/client/generate-payroll-transaction-s-m";
import { format } from 'date-fns';
import { ModuleNameSM } from "src/app/service-models/app/enums/module-name-s-m.enum";
import { PermissionSM } from "src/app/service-models/app/v1/client/permission-s-m";
import { AccountService } from "src/app/services/account.service";


@Component({
    selector: "app-transactions",
    templateUrl: "./transactions.component.html",
    styleUrls: ["./transactions.component.scss"],
    standalone: false
})
export class TransactionsComponent
  extends BaseComponent<TransactionViewModel>
  implements OnInit
{
  @ViewChild("content") content: TemplateRef<any> | undefined;

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    public datepipe: DatePipe,
    private transactionService: TransactionService,
    private accountService:AccountService
  ) {
    super(commonService, logService);
    this.viewModel = new TransactionViewModel();
  }

  async ngOnInit() {
    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;
    await this.getMonths();
    this.viewModel.selectedMonth = "";
    await this.getPermissions()
  }
  async getPayrolltransactionCountOfCompany() {
    try {
      await this._commonService.presentLoading();
      let UTCDate = this._commonService.getISODateFromMonthYear(
        this.viewModel.selectedMonth
      );
      let resp = await this.transactionService.getPayrollTransactionCountOfCompany(UTCDate);
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.pagination.totalCount = resp.successData.intResponse;
      }
    } catch (error) {
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  async getTransactionMonthlyBased() {
    try {
      await this._commonService.presentLoading();
      let UTCDate = this._commonService.getISODateFromMonthYear(
        this.viewModel.selectedMonth
      );
      let resp =
        await this.transactionService.getPayrollTransactionMonthlyBased(this.viewModel,
          UTCDate
        );
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        await this._commonService.ShowToastAtTopEnd(
          resp.errorData.displayMessage,
          "error"
        );
      } else {
        this.viewModel.showTable = true;
        this.viewModel.payrollTransactionList = resp.successData;
       await this.getPayrolltransactionCountOfCompany()

        let filteredList = this.viewModel.payrollTransactionList.filter(
          (x) => x.errorInGeneration == true
        );

        if (this.viewModel.payrollTransactionList.length == 0) {
          await this._commonService.ShowToastAtTopEnd(
            "No records Found",
            "info"
          );
        } else if (filteredList.length > 0) {
          this.viewModel.showGenerateAllBtn = true;
          this.viewModel.showPayAllBtn = true;
        } else {
          this.viewModel.showPayAllBtn = true;
          await this._commonService.ShowToastAtTopEnd(
            "Data Successfully Retrieved",
            "success"
          );
        }
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  async loadPagedataWithPagination(pageNumber: number) {
    if (pageNumber && pageNumber > 0) {
      // this.viewModel.PageNo = pageNumber;
      this.viewModel.pagination.PageNo = pageNumber;
      await this.getTransactionMonthlyBased();
    }
  }
  // async loadPagedataWithPagination(event: any) {
  //   this.viewModel.PageNo = event;
  //   await this.getTransactionMonthlyBased();
  // }
       //check permissions
       async getPermissions(){
        let ModuleName=ModuleNameSM.PayrollTransacton
         let resp:PermissionSM | any =await this.accountService.getMyModulePermissions(ModuleName)
         this.viewModel.Permission=resp
        }
  /**
   * Generate Payroll For a particular Employee
   * @param generatePayroll
   */
  async generateEmployeePayrollByClientUserId(
    generatePayroll: GeneratePayrollTransactionSM
  ) {
    try {
      await this._commonService.presentLoading();
      let genratePayrollForMonth = format(
        new Date(this.viewModel.selectedMonth),
        'MMMM-yyyy'
      );
      generatePayroll.paymentFor = genratePayrollForMonth;
      let resp = await this.transactionService.generatePayrollByClientUserId(
        generatePayroll
      );
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        await this._commonService.ShowToastAtTopEnd(
          resp.errorData.displayMessage,
          "error"
        );
      } else {
        await this.getTransactionMonthlyBased();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Generate Payroll For All The Employees
   */
  async generateAllEmployeePayroll() {
    try {
      await this._commonService.presentLoading();
      let filteredList = this.viewModel.payrollTransactionList.filter(
        (x) => x.paymentAmount == 0 && x.employeeStatus.toString() == "Active"
      );
      let resp = await this.transactionService.GenerateAllEmployeePayroll(
        filteredList
      );
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        await this._commonService.showInfoOnAlertWindowPopup(
          "error",
          resp.errorData.displayMessage,
          "error"
        );
      } else {
        this.viewModel.payrollTransactionList = resp.successData;
        let filteredList = this.viewModel.payrollTransactionList.filter(
          (x) => x.errorInGeneration == true
        );
        if (filteredList.length > 0) {
          await this._commonService.ShowToastAtTopEnd(
            "Add Salary To All Records First ",
            "info"
          );
        }
        await this.getTransactionMonthlyBased();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Get Names Of Month
   */
  async getMonths() {
    let date = new Date();
    let months = [],
      monthNames = [
        "January",
        "February",
        "March",
        "April",
        "May",
        "June",
        "July",
        "August",
        "September",
        "October",
        "November",
        "December",
      ];
    for (var i = 0; i < 12; i++) {
      months.push(monthNames[date.getMonth()] + " " + date.getFullYear());
      date.setMonth(date.getMonth() - 1);
    }
    this.viewModel.months = months;
  }
}
