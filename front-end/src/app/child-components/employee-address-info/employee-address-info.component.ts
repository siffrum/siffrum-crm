import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { AppConstants } from "src/app-constants";
import { BaseComponent } from "src/app/components/base.component";
import { ModuleNameSM } from "src/app/service-models/app/enums/module-name-s-m.enum";
import { ClientUserAddressSM } from "src/app/service-models/app/v1/app-users/client-user-address-s-m";
import { ClientUserSM } from "src/app/service-models/app/v1/app-users/client-user-s-m";
import { PermissionSM } from "src/app/service-models/app/v1/client/permission-s-m";
import { AccountService } from "src/app/services/account.service";
import { CommonService } from "src/app/services/common.service";
import { EmployeeService } from "src/app/services/employee.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { EmployeeAddressInfoViewModel } from "src/app/view-models/employee-address-info.viewmodel";
@Component({
    selector: "app-employee-address-info",
    templateUrl: "./employee-address-info.component.html",
    styleUrls: ["./employee-address-info.component.scss"],
    standalone: false
})
export class EmployeeAddressInfoComponent
  extends BaseComponent<EmployeeAddressInfoViewModel>
  implements OnInit {
  @Input() employee: ClientUserSM = new ClientUserSM();
  @Input() isReadonly!: boolean;
  @Input() isaddmode!: boolean;
  @Input() isEditMode!: boolean;
  @Output() newItemEvent = new EventEmitter<ClientUserAddressSM>();
  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private employeeService: EmployeeService,
    private router: Router,
    private accountService: AccountService
  ) {
    super(commonService, logService);
    this.viewModel = new EmployeeAddressInfoViewModel();
  }

  async ngOnInit() {
    if (!this.isaddmode) {
      await this.loadPageData();
    }
    await this.getPermissions();
  }

  /**
   * Get Employee Address
   */
  override async loadPageData() {
    try {
      await this._commonService.presentLoading();
      await this._commonService.getCompanyIdFromStorage();
      // Access the company ID from the common service
      const companyId = this._commonService.layoutVM.company.id;
      await this.getEmployeeAddressCount();
      let resp =
        await this.employeeService.getEmployeeAddressByCompanyIdAndEmployeeIdWithOData(
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
        this.viewModel.employeeAddressList = resp.successData;
        await this.getEmployeeAddressCount();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Check For Action permissions
   */
  async getPermissions() {
    let ModuleName = ModuleNameSM.EmployeeAddress;
    let resp: PermissionSM | any =
      await this.accountService.getMyModulePermissions(ModuleName);
    this.viewModel.Permission = resp;
  }
  /**
   * Add Employee Address Details
   */
  async addEmployeeAddressInfo(addressForm: NgForm) {
    this.viewModel.formSubmitted = true;
    try {
      if (addressForm.invalid) {
        await this._commonService.showSweetAlertToast({
          title: "Form Fields Are Not valid !",
          icon: "error",
        });
        return; // Stop execution here if form is not valid
      }
      await this._commonService.presentLoading();
      this.viewModel.employeeAddress.clientUserId = this.employee.id;
      let resp = await this.employeeService.addEmployeeAddress(
        this.viewModel.employeeAddress
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
          "Employee Address Added Successfully",
          "success"
        );
        this.sendToParent(resp.successData);
        // this.modalService.dismissAll();
        this.viewModel.displayStyle = "none";

        await this.loadPageData();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**Update Employee Address */

  async updateEmployeeAddressInfo(addressForm: NgForm) {
    this.viewModel.formSubmitted = true;
    try {
      if (addressForm.invalid) {
        await this._commonService.showSweetAlertToast({
          title: "Form Fields Are Not valid !",
          icon: "error",
        });
        return;
      }
      await this._commonService.presentLoading();
      let resp = await this.employeeService.updateEmployeeAddressByAddressId(
        this.viewModel.employeeAddress
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
          "Employee Address Details Updated Successfully",
          "success"
        );
        this.viewModel.displayStyle = "none";
        this.loadPageData();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  /**Get employee Address By Id
   * @param addressId
   */

  async getEmployeeAddressByAddressId(addressId: number) {
    try {
      await this._commonService.presentLoading();
      let resp = await this.employeeService.getEmployeeAddressByAddressId(
        addressId
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
        this.viewModel.employeeAddress = resp.successData;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  /**
   * Delete Employee Address By Address Id
   * @param addressId
   */
  async deleteEmployeeAddressByAddressId(addressId: number) {
    let deleteEmployeeAddressConfirmation =
      await this._commonService.showConfirmationAlert(
        AppConstants.DefaultMessages.EmployeeAddressDeleteAlert,
        " ",
        true,
        "warning"
      );
    if (deleteEmployeeAddressConfirmation.value==true) {
      try {
        await this._commonService.presentLoading();
        let resp = await this.employeeService.deleteEmployeeAddressByAddressId(
          addressId
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
          await this.loadPageData();
          this._commonService.ShowToastAtTopEnd(
            resp.successData.deleteMessage,
            "success"
          );
        }
      } catch (error) {
        throw error;
      } finally {
        await this._commonService.dismissLoader();
      }
    }
  }
  /**
   * Open Add Edit Employee Address Modal
   * @param id
   */
  async openEmployeeAddressInfoModal(id: number) {
    this.viewModel.employeeAddress = new ClientUserAddressSM();
    if (id > 0) {
      this.viewModel.editMode = true;
      this.getEmployeeAddressByAddressId(id);
    } else {
      this.viewModel.editMode = false;
    }
    this.viewModel.displayStyle = "block";
  }
  closePopup(addressForm: NgForm) {
    this.viewModel.displayStyle = "none";
    this.viewModel.formSubmitted = false; // Clear the formSubmitted flag
    if (addressForm) {
      addressForm.reset(); // Reset the form
    }
  }
  async skipEmployeeFurther() {
    this.router.navigate(["/employees-list"]);
    this.viewModel.displayStyle = "none";
  }
  sendToParent(employeeAddress: ClientUserAddressSM) {
    this.newItemEvent.emit(employeeAddress);
  }
  editDetails() {
    this.isReadonly = !this.isReadonly;
    this.viewModel.showButton = !this.viewModel.showButton;
  }
  /**
   * Get Employee total Address Count
   * @param id
   */
  async getEmployeeAddressCount() {
    try {
      await this._commonService.presentLoading();
      let resp = await this.employeeService.getEmployeeAddressCount(
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
