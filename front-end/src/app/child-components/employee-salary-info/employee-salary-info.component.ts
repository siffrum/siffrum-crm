import { DecimalPipe } from "@angular/common";
import { Component, Input, OnInit } from "@angular/core";
import { BaseComponent } from "src/app/components/base.component";
import { CommonService } from "src/app/services/common.service";
import { EmployeeService } from "src/app/services/employee.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { PayrollService } from "src/app/services/payroll.service";
import {
  EmployeeSalaryInfoViewModel,
  ExtendedClientEmployeePayrollComponentSM,
  ExtendedClientEmployeeSalarySM,
  UpdateComponentsType,
} from "src/app/view-models/employee-salary-info.viewmodel";
import { ComponentCalculationTypeSM } from "src/app/service-models/app/enums/component-calculation-type-s-m.enum";
import { ComponentPeriodTypeSM } from "src/app/service-models/app/enums/component-period-type-s-m.enum";
import { ClientEmployeeCTCDetailSM } from "src/app/service-models/app/v1/client/client-employee-c-t-c-detail-s-m";
import { ClientGenericPayrollComponentSM } from "src/app/service-models/app/v1/client/client-generic-payroll-component-s-m";
import { StorageService } from "src/app/services/storage.service";
import { ModuleNameSM } from "src/app/service-models/app/enums/module-name-s-m.enum";
import { PermissionSM } from "src/app/service-models/app/v1/client/permission-s-m";
import { AccountService } from "src/app/services/account.service";
import { RoleTypeSM } from "src/app/service-models/app/enums/role-type-s-m.enum";
import { NgForm } from "@angular/forms";

@Component({
    selector: "app-employee-salary-info",
    templateUrl: "./employee-salary-info.component.html",
    styleUrls: ["./employee-salary-info.component.scss"],
    standalone: false
})
export class EmployeeSalaryInfoComponent
  extends BaseComponent<EmployeeSalaryInfoViewModel>
  implements OnInit
{
  @Input() empid!: number;
  @Input() readonly!: boolean;
  @Input() isaddmode!: boolean;
  roleType: any;

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private employeeService: EmployeeService,
    // private modalService: NgbModal,
    private payrollService: PayrollService,
    private decimalPipe: DecimalPipe,
    private accountService: AccountService
  ) {
    super(commonService, logService);
    this.viewModel = new EmployeeSalaryInfoViewModel();
  }

  async ngOnInit() {
    this.roleType = RoleTypeSM[this._commonService.layoutVM.tokenRole];
    let role = this.roleType;
    if (role == "ClientEmployee") {
      this.viewModel.hideAddButton;
    } else {
      this.viewModel.hideAddButton = true;
    }
    if (!this.isaddmode) {
      await this.getEmployeeSalaryCount();
      await this.loadPageData();
    }
    await this.getPermissions();
  }
  //Check For Action Permissions
  async getPermissions() {
    let ModuleName = ModuleNameSM.EmployeeCTC;
    let resp: PermissionSM | any =
      await this.accountService.getMyModulePermissions(ModuleName);
    this.viewModel.Permission = resp;
  }
  /**Get Employee CTC Details  */
  override async loadPageData() {
    try {
      await this._commonService.presentLoading();
      await this.getEmployeeSalaryCount();
      await this._commonService.getCompanyIdFromStorage();
      let resp = await this.employeeService.getEmployeeSalaryInfoByOdata(
        this.viewModel,
        this.empid
      );
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData.displayMessage);
        this._commonService.showSweetAlertToast({
          title: "Error!",
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error",
        });
      } else {
        this.viewModel.employeeSalaryList = resp.successData;
        let salaryStatus=this.viewModel.employeeSalaryList.filter((x)=>x.currentlyActive==true)
        this.viewModel.startDateUTC = salaryStatus.map((item) => item.startDateUTC).toString().slice(0, 16);

      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  editDetails() {
    this.viewModel.isReadonly = !this.viewModel.isReadonly;
    this.viewModel.showButton = !this.viewModel.showButton;
  }

  /**Open Modal To Add or Edit Salary Information */
  async openSalaryInfoModal(ctcId: number) {
    this.viewModel.selectedEmployeeSalary =
      new ExtendedClientEmployeeSalarySM();
    this.viewModel.listCompomentPeriodType = [];
    this.viewModel.listCompomentPeriodType.push(
      this._commonService.SingleEnumToString(
        ComponentPeriodTypeSM,
        ComponentPeriodTypeSM.Monthly
      ),
      this._commonService.SingleEnumToString(
        ComponentPeriodTypeSM,
        ComponentPeriodTypeSM.Yearly
      )
    );
    this.viewModel.listPayrollCalculationType =
      this._commonService.EnumToStringArray(ComponentCalculationTypeSM);
    this.viewModel.enableComponentsEdit = false;
    if (ctcId > 0) {
      this.viewModel.ctcViewMode = true;
      let resp = await this.payrollService.GetEmployeeSalaryByCTCId(ctcId);
      this.convertToExtendedClientEmployeeSalarySM(resp.successData);
    } else {
      this.viewModel.ctcViewMode = false;
      this.viewModel.selectedEmployeeSalary =
        new ExtendedClientEmployeeSalarySM();
      let resp = await this.payrollService.getGenericComponents();
      resp.successData.forEach((_genericComp) => {
        this.viewModel.selectedEmployeeSalary.clientEmployeePayrollComponents.push(
          this.convertToExtendedClientEmployeePayrollComponentSM(_genericComp)
        );
      });
    }
    this.UpdateTotalMonthlyInfo();
    // this.modalService.open(content, { size: "lg", windowClass: "modal-xl" });
    this.viewModel.displayStyle = "block";
  }
  closePopup(salaryInputForm: NgForm) {
    this.viewModel.displayStyle = "none";
    this.viewModel.formSubmitted = false; // Clear the formSubmitted flag
    if (salaryInputForm) {
      salaryInputForm.reset(); // Reset the form
    }
  }

  private async convertToExtendedClientEmployeeSalarySM(
    successData: ClientEmployeeCTCDetailSM
  ) {
    this.viewModel.selectedEmployeeSalary = {
      ...successData,
      otherComponents: 0,
      percentageUI: 0,
      ctcMonthly: successData.ctcAmount / 12,
      amountMonthly: 0,
      clientEmployeePayrollComponents: [],
    };
    successData.clientEmployeePayrollComponents.forEach((componentSM) => {
      let _tempComponent: ExtendedClientEmployeePayrollComponentSM =
        this.convertToExtendedClientEmployeePayrollComponentSM(componentSM);
      _tempComponent.amountYearly = componentSM.amountYearly;
      _tempComponent.clientEmployeeCTCDetailId =
        componentSM.clientEmployeeCTCDetailId;
      this.updateDetailsByYearlyAmount(_tempComponent, false);
      this.viewModel.selectedEmployeeSalary.clientEmployeePayrollComponents.push(
        _tempComponent
      );
    });
    this.viewModel.selectedEmployeeSalary.startDateUTC =
      await this.getFormattedDate(
        this.viewModel.selectedEmployeeSalary.startDateUTC,
        false
      );
  }

  private convertToExtendedClientEmployeePayrollComponentSM(
    genericComponentSM: ClientGenericPayrollComponentSM
  ): ExtendedClientEmployeePayrollComponentSM {
    let _tempComponent: ExtendedClientEmployeePayrollComponentSM = {
      ...genericComponentSM,
      amountMonthlyUI: 0,
      amountYearlyUI: 0,
      percentageUI: genericComponentSM.percentage,
      amountMonthly: 0,
      amountYearly: 0,
      clientEmployeeCTCDetailId: 0,
    };
    return _tempComponent;
  }

  private UpdateTotalMonthlyInfo() {
    this.viewModel.selectedEmployeeSalary.amountMonthly =
      this.viewModel.selectedEmployeeSalary.clientEmployeePayrollComponents.reduce(
        (sum, current) => {
          if (
            current.componentPeriodType ==
            this._commonService.SingleEnumToString(
              ComponentPeriodTypeSM,
              ComponentPeriodTypeSM.Monthly
            )
          ) {
            if (
              current.componentCalculationType ==
              this._commonService.SingleEnumToString(
                ComponentCalculationTypeSM,
                ComponentCalculationTypeSM.Addition
              )
            )
              return (sum = sum + current.amountMonthly);
            else return (sum = sum - current.amountMonthly);
          }
          return sum;
        },
        0
      );
    this.viewModel.selectedEmployeeSalary.otherComponents =
      this.viewModel.selectedEmployeeSalary.ctcMonthly -
      this.viewModel.selectedEmployeeSalary.amountMonthly;
    // this.viewModel.selectedEmployeeSalary.PercentageUI = 0;
    // this.viewModel.selectedEmployeeSalary.clientEmployeePayrollComponents.forEach(comp => {
    //   this.viewModel.selectedEmployeeSalary.PercentageUI += comp.percentageUI;
    // });
    this.viewModel.selectedEmployeeSalary.percentageUI =
      (this.viewModel.selectedEmployeeSalary.clientEmployeePayrollComponents.reduce(
        (sum, current) => sum + current.percentageUI,
        0
      ) *
        100) /
      100;
    if (this.viewModel.selectedEmployeeSalary.percentageUI > 100) {
      this.viewModel.showPercentagValidationMessage=true;
      this.viewModel.percentagValidationMessage="percentage can't be greater than 100!"
      // this._commonService.showSweetAlertToast({
      //   title: "percentage can't be greater than 100!",
      //   icon: "error",
      // });

    }
    else{
      this.viewModel.showPercentagValidationMessage=false;
    }
  }
  /**
   * Update CTC Components
   * @param updateType
   * @param componentId
   */
  async UpdateComponents(
    updateType: UpdateComponentsType,
    componentId: number
  ) {
    this.viewModel.enableComponentsEdit = true;
    switch (updateType) {
      case UpdateComponentsType.CTC: {
        if (
          this.viewModel.selectedEmployeeSalary.ctcAmount == null ||
          undefined ||
          this.viewModel.selectedEmployeeSalary.ctcAmount <= 0
        ) {
          this.viewModel.enableComponentsEdit = false;
          break;
        }
        this.viewModel.selectedEmployeeSalary.ctcMonthly =
          this.viewModel.selectedEmployeeSalary.ctcAmount / 12;
        this.viewModel.selectedEmployeeSalary.clientEmployeePayrollComponents.forEach(
          (element) => {
            if (element.percentage != null && element.percentage > 0)
              this.UpdateAmountUsingPercentage(element, false);
            else
              switch (element.componentPeriodType) {
                case this._commonService.SingleEnumToString(
                  ComponentPeriodTypeSM,
                  ComponentPeriodTypeSM.Monthly
                ): {
                  this.updateDetailsByMonthlyAmount(element, false);
                  break;
                }
                case this._commonService.SingleEnumToString(
                  ComponentPeriodTypeSM,
                  ComponentPeriodTypeSM.Yearly
                ): {
                  this.updateDetailsByMonthlyAmount(element, false);
                  break;
                }
              }
          }
        );
        break;
      }
      case UpdateComponentsType.ComponentAmountMonthly: {
        let item =
          this.viewModel.selectedEmployeeSalary.clientEmployeePayrollComponents.filter(
            (x) => x.id == componentId
          )[0];
        this.updateDetailsByMonthlyAmount(item, true);
        break;
      }
      case UpdateComponentsType.ComponentAmountYearly: {
        let item =
          this.viewModel.selectedEmployeeSalary.clientEmployeePayrollComponents.filter(
            (x) => x.id == componentId
          )[0];
        this.updateDetailsByYearlyAmount(item, true);
        break;
      }
      case UpdateComponentsType.ComponentPercentage: {
        let item =
          this.viewModel.selectedEmployeeSalary.clientEmployeePayrollComponents.filter(
            (x) => x.id == componentId
          )[0];
        this.UpdateAmountUsingPercentage(item, true);
        break;
      }
      case UpdateComponentsType.ComponentCalculationPeriod: {
        break;
      }
      case UpdateComponentsType.ComponentCalculationType: {
        break;
      }
    }
    this.UpdateTotalMonthlyInfo();
  }

  private updateDetailsByMonthlyAmount(
    payrollComponent: ExtendedClientEmployeePayrollComponentSM,
    useUIamount: boolean
  ) {
    if (useUIamount)
      payrollComponent.amountMonthly = payrollComponent.amountMonthlyUI;
    payrollComponent.amountYearly = payrollComponent.amountMonthly * 12;
    payrollComponent.percentage =
      (payrollComponent.amountYearly /
        this.viewModel.selectedEmployeeSalary.ctcAmount) *
      100;
    this.UIValuesToApproximate(payrollComponent);
  }

  private updateDetailsByYearlyAmount(
    payrollComponent: ExtendedClientEmployeePayrollComponentSM,
    useUIamount: boolean
  ) {
    if (useUIamount)
      payrollComponent.amountYearly = payrollComponent.amountYearlyUI;
    payrollComponent.amountMonthly = payrollComponent.amountYearly / 12;
    payrollComponent.percentage =
      (payrollComponent.amountYearly /
        this.viewModel.selectedEmployeeSalary.ctcAmount) *
      100;
    this.UIValuesToApproximate(payrollComponent);
  }

  private UpdateAmountUsingPercentage(
    payrollComponent: ExtendedClientEmployeePayrollComponentSM,
    useUIpercent: boolean
  ) {
    if (useUIpercent)
      payrollComponent.percentage = payrollComponent.percentageUI;
    payrollComponent.amountMonthly =
      this.viewModel.selectedEmployeeSalary.ctcMonthly *
      (payrollComponent.percentage / 100);
    payrollComponent.amountYearly =
      this.viewModel.selectedEmployeeSalary.ctcAmount *
      (payrollComponent.percentage / 100);
    this.UIValuesToApproximate(payrollComponent);
  }

  private UIValuesToApproximate(
    payrollComponent: ExtendedClientEmployeePayrollComponentSM
  ) {
    let amtYearly = this.decimalPipe.transform(
      payrollComponent.amountYearly,
      "2.2-2"
    );
    if (amtYearly != null)
      payrollComponent.amountYearlyUI = parseFloat(amtYearly.replace(/,/g, ""));

    let amtMonthly = this.decimalPipe.transform(
      payrollComponent.amountMonthly,
      "2.2-2"
    );
    if (amtMonthly != null)
      payrollComponent.amountMonthlyUI = parseFloat(
        amtMonthly.replace(/,/g, "")
      );

    let y = Math.ceil(payrollComponent.percentage * 100) / 100;
    if (Math.floor(y) == Math.floor(payrollComponent.percentage))
      payrollComponent.percentageUI =
        Math.ceil(payrollComponent.percentage * 100) / 100;
    else
      payrollComponent.percentageUI =
        Math.floor(payrollComponent.percentage * 100) / 100;
  }
  //Check if required salary Form Fields are filled or Emplty
  isFormValid(): boolean {
    let amount = !!this.viewModel.selectedEmployeeSalary.ctcAmount;
    let currency = !!this.viewModel.selectedEmployeeSalary.currencyCode;
    return amount && currency;
  }
  /**
   * Add Employee Salary
   */

  async addEmployeeSalary(salaryInputForm: NgForm) {
    this.viewModel.formSubmitted = true;
    try {
      if (salaryInputForm.invalid) {
        await this._commonService.showSweetAlertToast({
          title: "Form Fields Are Not valid !",
          icon: "error",
        });
        return;
      }
      if (this.viewModel.selectedEmployeeSalary.percentageUI > 100) {
        await this._commonService.showSweetAlertToast({
          title: "percentage can't be greater than 100!",
          icon: "error",
        });
        return;
      }
      this.viewModel.selectedEmployeeSalary.clientEmployeePayrollComponents.filter(
        (x) => (x.id = 0)
      )[0];

      this.viewModel.selectedEmployeeSalary.clientUserId = this.empid;
      this.viewModel.selectedEmployeeSalary.currentlyActive = false;
      let resp = await this.employeeService.addEmployeeSalary(
        this.viewModel.selectedEmployeeSalary
      );
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: "Error!",
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error",
        });
      } else {
        this._commonService.ShowToastAtTopEnd(
          "Employee Salary Added Successfully",
          "success"
        );
        // this.modalService.dismissAll();
        this.viewModel.displayStyle = "none";
        this.loadPageData();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Get Employee total Salary  Count
   * @param id
   */
  async getEmployeeSalaryCount() {
    try {
      await this._commonService.presentLoading();
      let resp = await this.employeeService.getEmployeeSalaryCount(this.empid);
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: "Error!",
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error",
        });
      } else {
        this.viewModel.pagination.totalCount = resp.successData.intResponse;
      }
    } catch (error) {
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**this function is used to create an event for pagination */
  async loadPagedataWithPagination(pageNumber: number) {
    if (pageNumber && pageNumber > 0) {
      // this.viewModel.PageNo = pageNumber;
      this.viewModel.pagination.PageNo = pageNumber;
      await this.loadPageData();
    }
  }
}
