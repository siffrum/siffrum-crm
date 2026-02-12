import { Component, Input, OnInit } from "@angular/core";
import { NgForm } from "@angular/forms";
import { AppConstants } from "src/app-constants";
import { BaseComponent } from "src/app/components/base.component";
import { LeaveTypeSM } from "src/app/service-models/app/enums/leave-type-s-m.enum";
import { ModuleNameSM } from "src/app/service-models/app/enums/module-name-s-m.enum";
import { ClientUserSM } from "src/app/service-models/app/v1/app-users/client-user-s-m";
import { ClientEmployeeLeaveExtendedUserSM } from "src/app/service-models/app/v1/client/client-employee-leave-extended-user-s-m";
import { PermissionSM } from "src/app/service-models/app/v1/client/permission-s-m";
import { AccountService } from "src/app/services/account.service";
import { CommonService } from "src/app/services/common.service";
import { LeavesService } from "src/app/services/leaves.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { EmployeeLeavesInfoViewModel } from "src/app/view-models/employee-leaves-info.viewmodel";

@Component({
    selector: "app-employee-leaves-info",
    templateUrl: "./employee-leaves-info.component.html",
    styleUrls: ["./employee-leaves-info.component.scss"],
    standalone: false
})
export class EmployeeLeavesInfoComponent
  extends BaseComponent<EmployeeLeavesInfoViewModel>
  implements OnInit
{
  @Input() employee: ClientUserSM = new ClientUserSM();
  @Input() isReadOnly!: boolean;
  @Input() isAdminMode!: boolean;

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private leaveService: LeavesService,
    private accountService: AccountService
  ) {
    super(commonService, logService);
    this.viewModel = new EmployeeLeavesInfoViewModel();
  }

  async ngOnInit() {
    if (this.isAdminMode) {
      this.viewModel.leaveTypes = await this._commonService.EnumToStringArray(
        LeaveTypeSM
      );
      await this.loadAdminPageData();
      await this.getTotalLeavesCountOfCompnay();
    } else {
      this.viewModel.leaveTypes = await this._commonService.EnumToStringArray(
        LeaveTypeSM
      );
      await this.getEmployeeLeavesCount();
      await this.loadPageData();
    }
    this.getPermissions();
  }

  /**
   * Get ALL Employee Leaves
   */
  override async loadPageData() {
    try {
      await this._commonService.presentLoading();
      await this.getEmployeeLeavesCount();
      await this._commonService.getCompanyIdFromStorage();
      // Access the company ID from the common service
      let resp = await this.leaveService.getEmployeeLeavesByEmployeeIdByOdata(
        this.employee.id,
        this.viewModel
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
        this.viewModel.employeeLeavesList = resp.successData;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  //Check For Action permissions
  async getPermissions() {
    let ModuleName = ModuleNameSM.Leave;
    let resp: PermissionSM | any =
      await this.accountService.getMyModulePermissions(ModuleName);
    this.viewModel.Permission = resp;
  }
  /**
   * Get All leaves (Company ) Page Data
   */
  async loadAdminPageData() {
    try {
      await this._commonService.presentLoading();
      await this.getTotalLeavesCountOfCompnay();
      await this._commonService.getCompanyIdFromStorage();
      // Access the company ID from the common service
      // const companyId = this._commonService.layoutVM.company.id;
      let resp = await this.leaveService.getAllCompanyLeavesByOdata(
        this.viewModel
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
        this.viewModel.companyEmployeeLeavesListExtended = resp.successData;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  //Check All required Add Employee form Fileds are filled or Empty;
  isFormValid(): boolean {
    let leaveType = !!this.viewModel.employeeLeaveExtended.leaveType;
    let leaveDateFrom = !!this.viewModel.employeeLeaveExtended.leaveDateFromUTC;
    let leaveTo = !!this.viewModel.employeeLeaveExtended.leaveDateToUTC;
    return leaveType && leaveDateFrom && leaveTo;
  }
  /**
   * Add Employee Leave
   */
  async addEmployeeLeave(leaveForm: NgForm) {
    this.viewModel.formSubmitted = true;
    try {
      if (leaveForm.invalid) {
        await this._commonService.showSweetAlertToast({
          title: "Form Fields Are Not valid !",
          icon: "error",
        });
        return;
      }
      await this._commonService.presentLoading();
      this.viewModel.employeeLeaveExtended.clientUserId = this.employee.id;
      let resp = await this.leaveService.addEmployeeLeave(
        this.viewModel.employeeLeaveExtended
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
          "Employee Leave Added Successfully",
          "success"
        );
        // this.modalService.dismissAll();
        this.viewModel.displayStyle = "none";
        await this.getEmployeeLeavesCount();
        await this.loadPageData();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
/**
 * Update Employee Leave
 * @param employeeLeave
 * @param approval 
 */
  async updateEmployeeLeave(
    employeeLeave: ClientEmployeeLeaveExtendedUserSM,
    approval: boolean | null = null
  ) {
    try {
      await this._commonService.presentLoading();
      if (approval !== null && approval == true ) {
        this.viewModel.isReadOnly
        employeeLeave.isApproved = approval;
        await this.leaveApproval(employeeLeave);
      } else if (approval !== null || approval == false) {
        this.viewModel.isReadOnly
        employeeLeave.isApproved = approval;
        await this.leaveApproval(employeeLeave);
      }
      else{
        this.viewModel.isReadOnly=false;
        await this.leaveApproval(employeeLeave);
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
/**
 * Approve Employee Leave
 * @param employeeLeave
 */
  async leaveApproval(employeeLeave: ClientEmployeeLeaveExtendedUserSM) {
    try {
      await this._commonService.presentLoading();
      let resp = await this.leaveService.updateEmployeeLeaveByLeaveId(
        employeeLeave
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
          "Employee Leave Updated Successfully",
          "success"
        );
        // this.modalService.dismissAll();
        this.viewModel.displayStyle = "none";
        if (this.isAdminMode) {
          await this.loadAdminPageData();
          // await this.getTotalLeavesCountOfCompany();
        } else {
          await this.loadPageData();
          // await this.getSingleEmployeeLeaveCount();
        }
      }
    } catch (error) {
      throw error;
    }
  }

  /**\
   * Get Employee Leave By Leave Id
   */
  async getEmployeeLeaveByleaveId(leaveId: number) {
    try {
      await this._commonService.presentLoading();
      let resp = await this.leaveService.getEmployeeLeaveByLeaveId(leaveId);
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {

        this.viewModel.employeeLeaveExtended = resp.successData;
        this.viewModel.employeeLeaveExtended.leaveDateFromUTC=await this.getFormattedDate(this.viewModel.employeeLeaveExtended.leaveDateFromUTC,false)
        this.viewModel.employeeLeaveExtended.leaveDateToUTC=await this.getFormattedDate(this.viewModel.employeeLeaveExtended.leaveDateToUTC,false)
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Delete Employee Leave By Leave Id
   * @param leaveId
   */

  async deleteEmployeeLeaveByleaveId(leaveId: number) {
    let deleteEmployeeLeaveConfirmation =
      await this._commonService.showConfirmationAlert(
        AppConstants.DefaultMessages.EmployeeLeaveDeleteAlert,
        " ",
        true,
        "warning"
      );
    if (deleteEmployeeLeaveConfirmation) {
      try {
        await this._commonService.presentLoading();
        let resp = await this.leaveService.deleteEmployeeLeaveByLeaveId(
          leaveId
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
          if (this.isAdminMode) {
            await this.loadAdminPageData();
          } else {
            await this.loadPageData();
          }
        }
      } catch (error) {
        throw error;
      } finally {
        await this._commonService.dismissLoader();
      }
    }
  }

  /**
   * Get Total Leave Count Of The Company
   */
  async getTotalLeavesCountOfCompnay() {
    try {
      await this._commonService.presentLoading();
      let resp = await this.leaveService.getLeavesCountOfCompany();
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
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  /**
   * Get Employee Total Leave Count
   */
  async getEmployeeLeavesCount() {
    try {
      await this._commonService.presentLoading();
      let resp = await this.leaveService.getEmployeeLeaveCountByEmployeeId(
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
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  disabledPastPastDates():string{
    let currentDate = new Date();
    let year = currentDate.getFullYear();
    let month = ('0' + (currentDate.getMonth() + 1)).slice(-2);
    let day = ('0' + currentDate.getDate()).slice(-2);
    return `${year}-${month}-${day}`;
  }
  /**
   * Open Modal to Add Leave
   * @param content
   * @param id
   */
  async openEmployeeLeaveModal(id: number) {
    this.viewModel.employeeLeaveExtended =
      new ClientEmployeeLeaveExtendedUserSM();
    if (id > 0) {
      this.viewModel.isAddMode = false;
      await this.getEmployeeLeaveByleaveId(id);
      this.viewModel.showApproveBtn=false;
    } else {
      this.viewModel.isAddMode = true;
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
  /**this function is used to create an event for pagination */
  async loadPagedataWithPagination(pageNumber: number) {
    if (pageNumber && pageNumber > 0) {
      // this.viewModel.PageNo = pageNumber;
      this.viewModel.pagination.PageNo = pageNumber;
         if (this.isAdminMode) {
       await this.loadAdminPageData();
       } else {
      await this.loadPageData();
      }

    }
  }
}
