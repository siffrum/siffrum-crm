import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { AppConstants } from "src/app-constants";
import { BaseComponent } from "src/app/components/base.component";
import { ModuleNameSM } from "src/app/service-models/app/enums/module-name-s-m.enum";
import { ClientUserSM } from "src/app/service-models/app/v1/app-users/client-user-s-m";
import { ClientEmployeeBankDetailSM } from "src/app/service-models/app/v1/client/client-employee-bank-detail-s-m";
import { PermissionSM } from "src/app/service-models/app/v1/client/permission-s-m";
import { AccountService } from "src/app/services/account.service";
import { CommonService } from "src/app/services/common.service";
import { EmployeeService } from "src/app/services/employee.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { EmployeeBankInfoViewModel } from "src/app/view-models/employee-bank-info.viewmodel";

@Component({
    selector: "app-employee-bank-info",
    templateUrl: "./employee-bank-info.component.html",
    styleUrls: ["./employee-bank-info.component.scss"],
    standalone: false
})
export class EmployeeBankInfoComponent
  extends BaseComponent<EmployeeBankInfoViewModel>
  implements OnInit
{
  @Input() employee: ClientUserSM = new ClientUserSM();
  @Input() isReadonly!: boolean;
  @Input() isaddmode!: boolean;
  @Input() isEditMode!: boolean;
  @Output() newItemEvent = new EventEmitter<ClientEmployeeBankDetailSM>();

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private employeeService: EmployeeService,
    private router: Router,
    private accountService: AccountService
  ) {
    super(commonService, logService);
    this.viewModel = new EmployeeBankInfoViewModel();
  }

  async ngOnInit() {
    if (!this.isaddmode) {
      await this.loadPageData();
      await this.getEmployeeBankDetailCount(this.employee.id);
    }
    await this.getPermissions();
  }
  //Check For Action permissions
  async getPermissions() {
    let ModuleName = ModuleNameSM.BankDetail;
    let resp: PermissionSM | any =
      await this.accountService.getMyModulePermissions(ModuleName);
    this.viewModel.Permission = resp;
  }
  /**
   * Get Employee Bank Details
   */
  override async loadPageData() {
    try {
      await this._commonService.presentLoading();
      await this._commonService.getCompanyIdFromStorage();
      // Access the company ID from the common service
      const companyId = this._commonService.layoutVM.company.id;
      await this.getEmployeeBankDetailCount(this.employee.id);
      let resp =
        await this.employeeService.getEmployeeBankDetailsByCompanyIdAndEmployeeIdWithOData(
          this.viewModel,
          companyId,
          this.employee.id
        );
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.employeeBankList = resp.successData;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Add Employee Bank Details
   */
  async addEmployeeBankInfo(bankDetailForm: NgForm) {
    this.viewModel.formSubmitted = true;
    try {
      if (bankDetailForm.invalid) {
        await this._commonService.showSweetAlertToast({
          title: "Form Fields Are Not valid !",
          icon: "error",
        });
        return;
      }
      await this._commonService.presentLoading();
      this.viewModel.employeeBank.clientUserId = this.employee.id;
      let resp = await this.employeeService.addEmployeeBankInfo(
        this.viewModel.employeeBank
      );
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this._commonService.ShowToastAtTopEnd(
          "Employee Bank-Info Added Successfully",
          "success"
        );
        this.sendToParent(resp.successData);
        this.viewModel.displayStyle = "none";
        await this.loadPageData();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Update Employee Bank Details
   */
  async updateEmployeeBankInfo(bankDetailForm: NgForm) {
    this.viewModel.formSubmitted = true;
    try {
      if (bankDetailForm.invalid) {
        await this._commonService.showSweetAlertToast({
          title: "Form Fields Are Not valid !",
          icon: "error",
        });
        return;
      }
      await this._commonService.presentLoading();
      let resp = await this.employeeService.updateEmployeeBankByBankId(
        this.viewModel.employeeBank
      );
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this._commonService.ShowToastAtTopEnd(
          "Employee Bank Details Updated Successfully",
          "success"
        );
        this.loadPageData();
        this.viewModel.displayStyle = "none";
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  /**
   * Get Employee  Bank Information by Bank Id
   * @param bankId
   */
  async getEmployeeBankInfoByBankId(bankId: number) {
    try {
      await this._commonService.presentLoading();
      let resp = await this.employeeService.getEmployeeBankByBankId(bankId);
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.employeeBank = resp.successData;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  /**
   * Delete Employee Bank Details By Bank Id
   * @param bankId
   */
  async deleteEmployeeBankByBankId(bankId: number) {
    let deleteEmployeeBankConfirmation =
      await this._commonService.showConfirmationAlert(
        AppConstants.DefaultMessages.EmployeeBankDeleteAlert,
        " ",
        true,
        "warning"
      );
    if (deleteEmployeeBankConfirmation) {
      try {
        await this._commonService.presentLoading();
        let resp = await this.employeeService.deleteEmployeeBankByBankId(
          bankId
        );
        if (resp.isError) {
          await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
        } else {
          this._commonService.ShowToastAtTopEnd(
            resp.successData.deleteMessage,
            "success"
          );
          await this.loadPageData();
        }
      } catch (error) {}
    }
  }

  async skipEmployeeFurther() {
    this.router.navigate(["/employees-list"]);
  }

  sendToParent(employeeBank: ClientEmployeeBankDetailSM) {
    this.newItemEvent.emit(employeeBank);
  }
  /**
   * Edit details or Not
   */
  editDetails() {
    this.isReadonly = !this.isReadonly;
    this.viewModel.showButton = !this.viewModel.showButton;
  }

  /**
   * Open Add Edit Modal
   * @param id
   */
  openEmployeeBankModal(id: number) {
    this.viewModel.employeeBank = new ClientEmployeeBankDetailSM();
    if (id > 0) {
      this.viewModel.editMode = true;
      this.getEmployeeBankInfoByBankId(id);
    } else {
      this.viewModel.editMode = false;
    }
    this.viewModel.displayStyle = "block";
  }
  closePopup(bankDetailForm: NgForm) {
    this.viewModel.displayStyle = "none";
    this.viewModel.formSubmitted = false; // Clear the formSubmitted flag
    if (bankDetailForm) {
      bankDetailForm.reset(); // Reset the form
    }
  }
  async getEmployeeBankDetailCount(id: number) {
    try {
      await this._commonService.presentLoading();
      let resp = await this.employeeService.getEmployeeBankDetailCount(
        this.employee.id
      );
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
  /**this function is used to create an event for pagination */
  async loadPagedataWithPagination(pageNumber: number) {
    if (pageNumber && pageNumber > 0) {
      // this.viewModel.PageNo = pageNumber;
      this.viewModel.pagination.PageNo = pageNumber;
      await this.loadPageData();
    }
  }
}
