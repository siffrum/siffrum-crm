import { Component, Input, OnInit } from "@angular/core";
import { BaseComponent } from "../../base.component";
import { AddCompanyAdminViewModel } from "src/app/view-models/add-company-admin.viewmodel";
import { CommonService } from "src/app/services/common.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { SuperCompanyService } from "src/app/services/super-company.service";
import { ActivatedRoute, Router } from "@angular/router";
import { ClientCompanyDetailSM } from "src/app/service-models/app/v1/client/client-company-detail-s-m";
import { EmployeeStatusSM } from "src/app/service-models/app/enums/employee-status-s-m.enum";
import { RoleTypeSM } from "src/app/service-models/app/enums/role-type-s-m.enum";
import { LoginStatusSM } from "src/app/service-models/app/enums/login-status-s-m.enum";
import { ClientUserSM } from "src/app/service-models/app/v1/app-users/client-user-s-m";
import { ModulePermissionsService } from "src/app/services/module-permissions.service";
import { AppConstants } from "src/app-constants";
import { NgForm } from "@angular/forms";

@Component({
    selector: "app-add-admin",
    templateUrl: "./add-company-admin.component.html",
    styleUrls: ["./add-company-admin.component.scss"],
    standalone: false
})
export class AddCompanyAdminComponent
  extends BaseComponent<AddCompanyAdminViewModel>
  implements OnInit
{
  @Input() isReadOnly!: boolean;
  @Input() company: ClientCompanyDetailSM = new ClientCompanyDetailSM();
  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private activatedRoute: ActivatedRoute,
    private superCompanyService: SuperCompanyService,
    private modulePermissionService:ModulePermissionsService,
    private router: Router
  ) {
    super(commonService, logService);
    this.viewModel = new AddCompanyAdminViewModel();
  }
  async ngOnInit() {
    this.viewModel.initialAddModePermissionCompanyDetailId = this.company.id;
    if (this.viewModel.initialAddModePermissionCompanyDetailId > 0) {
      this.viewModel.addAdminForm = true;
      this._commonService.layoutVM.isAddMode = true;
    } else {
      this.viewModel.addAdminForm = false;
      this.viewModel.permissionTableModal = false;
      this._commonService.layoutVM.isAddMode = false;
    }
    let Id = Number(this.activatedRoute.snapshot.paramMap.get("Id"));
    this.viewModel.company.id = Id;
    if (Id == undefined) {
      await this._commonService.ShowToastAtTopEnd(
        "Something Went Wrong",
        "error"
      );
    } else {
      this.viewModel.permissionTableModal = true;
      this.viewModel.addAdminForm = false;
      this._commonService.layoutVM.isAddMode = false;
    }
    this.getAllCompanyAdminsByCompanyId();
  }

  /**Get Company User list by Comapny Id
   * @params company-id.and queryFilter
   * @return SuccessData.
   */
  async getAllCompanyAdminsByCompanyId() {
    try {
      await this._commonService.presentLoading();
      await this.getCompanyAdminsCountByCompanyId();
      let resp =
        await this.superCompanyService.getAllCompanyAdminsByCompanyIdUsingOdata(
          this.viewModel.company.id,
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
        this.viewModel.companyUserList = resp.successData;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**this function is used to create an event for pagination */
  async loadPagedataWithPagination(pageNumber: number) {
    if (pageNumber && pageNumber > 0) {
      // this.viewModel.PageNo = pageNumber;
      this.viewModel.pagination.PageNo = pageNumber;
      await this.getAllCompanyAdminsByCompanyId();
    }
  }
  /**switch to add admin Form */
  switchToForm() {
    this.viewModel.permissionTableModal = false;
    this.viewModel.addAdminForm = true;
    this.viewModel.addAdmin = new ClientUserSM();
  }
    /**switch to add admin Form */
    back() {
      this.viewModel.permissionTableModal = true;
      this.viewModel.addAdminForm = false;
    }

  /**
   * Create new Client Admin for the company
   * Here we are adding some hard Coded data to the
   *  viz(isEmailConfirmed,isPhoneNumberConfirmed,employeeStatus)
   * @returns success
   * @developer Musaib
   */
  async createNewCompanyAdmin(createEmployeeForm: NgForm) {
    this.viewModel.formSubmitted = true;
    try {
      if (createEmployeeForm.invalid) {
        await this._commonService.showSweetAlertToast({
          title: "Form Fields Are Not valid !",
          icon: "error",
        });
        return; // Stop execution here if form is not valid
      }
      await this._commonService.presentLoading();
      this.viewModel.initialAddModePermissionCompanyDetailId = this.company.id;
      if (this.viewModel.initialAddModePermissionCompanyDetailId > 0) {
        this._commonService.layoutVM.isAddMode = true;
      } else {
        this._commonService.layoutVM.isAddMode = false;
      }
      this.viewModel.addAdmin.clientCompanyDetailId = this._commonService
        .layoutVM.isAddMode
        ? this.viewModel.initialAddModePermissionCompanyDetailId
        : this.viewModel.company.id;
      this.viewModel.addAdmin.isEmailConfirmed = true;
      this.viewModel.addAdmin.isPhoneNumberConfirmed = true;
      this.viewModel.addAdmin.employeeStatus = EmployeeStatusSM.Active;
      this.viewModel.addAdmin.roleType = RoleTypeSM.ClientAdmin;
      this.viewModel.addAdmin.loginId =
        this.viewModel.addAdmin.firstName + this.viewModel.addAdmin.lastName;
      this.viewModel.addAdmin.passwordHash =
        this.viewModel.addAdmin.lastName + this.viewModel.addAdmin.firstName;
      this.viewModel.addAdmin.loginStatus = LoginStatusSM.Enabled;
      let resp = await this.superCompanyService.addNewCompanyAdmin(
        this.viewModel.addAdmin
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
        this._commonService.showInfoOnAlertWindowPopup(
          "success",
          "Added Admin Successfully Please Note the Login Credentials for this User:",
          `Username: ${this.viewModel.addAdmin.loginId}
        Password: ${this.viewModel.addAdmin.passwordHash}`
        );
        this.viewModel.addAdmin = resp.successData;
        this.router.navigate(["/admin/companylist"]);
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  /**Get total ADmin Users in a Comapany.
   * @returns Count
   */
  async getCompanyAdminsCountByCompanyId() {
    try {
      await this._commonService.presentLoading();
      let resp =
        await this.superCompanyService.getAllCompanyAdminsCountByCompanyId(
          this.viewModel.company.id
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
  /**Update Login status of Company Users/three states
   * @param Event
   * @param Id
   * @returns boolean response
   * @developer Musaib
   */
  async updateLoginStatus(event: any, status: ClientUserSM) {
    try {
      if (event == "Enabled") {
        this.viewModel.companyUser.loginStatus = LoginStatusSM.Enabled;
      } else if (event == "Disabled") {
        this.viewModel.companyUser.loginStatus = LoginStatusSM.Disabled;
      } else {
        this.viewModel.companyUser.loginStatus =
          LoginStatusSM.PasswordResetRequired;
      }
      this.viewModel.companyUser.id = status.id;
      let resp = await this.superCompanyService.updateUserLoginStatus(
        this.viewModel.companyUser
      );
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        await this._commonService.ShowToastAtTopEnd(
          resp.errorData.displayMessage,
          "error"
        );
      } else {
        await this._commonService.ShowToastAtTopEnd(
          "Updated Successfully",
          "success"
        );
        await this.getAllCompanyAdminsByCompanyId();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }


  /**
   * Get company Admin by admon Id
   * @param bankId
   */
  async getCompanyAdminsById(adminId: number) {
    try {
      await this._commonService.presentLoading();
      let resp = await this.superCompanyService.getCompanyAdminsByAdminId(
        adminId
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
        this.viewModel.addAdmin = resp.successData;
        this.viewModel.addAdmin.dateOfBirth=await this.getFormattedDate(this.viewModel.addAdmin.dateOfBirth,false)
        this.viewModel.addAdmin.dateOfJoining=await this.getFormattedDate(this.viewModel.addAdmin.dateOfJoining,false)
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  async updateCompanyAdminDetails(editAdminForm: NgForm) {
    this.viewModel.formSubmitted = true;
    try {
      if (editAdminForm.invalid) {
        await this._commonService.showSweetAlertToast({
          title: "Form Fields Are Not valid !",
          icon: "error",
        });
        return; // Stop execution here if form is not valid
      }
      await this._commonService.presentLoading();
      let resp = await this.superCompanyService.updateCompanyAdminDetails(
        this.viewModel.addAdmin
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
          "Admin  Details Updated Successfully",
          "success"
        );
        this.viewModel.addAdmin = resp.successData;
        await this.closePopup();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Open modal to UPDATE Admin Details
   * @param id
   */
  openEditAdminModal(id: number) {
    this.viewModel.addAdmin = new ClientUserSM();
    if (id > 0) {
      this.viewModel.editMode = true;
      this.viewModel.permissionsModal = false;
      this.viewModel.adminDetailsModal = true;
      this.getCompanyAdminsById(id);
    } else {
      this.viewModel.editMode = false;
    }
    this.viewModel.displayStyle = "block";
  }
  /**Delete Company Admin */
  async deleteCompanyAdminByAdminId(AdminId: number) {
    let deleteCompanyAdminConfirmation =
      await this._commonService.showConfirmationAlert(
        AppConstants.DefaultMessages.AdminDeleteAlert,
        " ",
        true,
        "warning"
      );
    if (deleteCompanyAdminConfirmation) {
      try {
        if (this.viewModel.companyUserList.length == 1) {
          return;
        }
        await this._commonService.presentLoading();
        let resp = await this.superCompanyService.deleteCompanyAdminByAdminId(
          AdminId
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
          await this.getAllCompanyAdminsByCompanyId();
        }
      } catch (error) {}
    }
  }
  /** Get Permission of the Company Admin */
  async getModulePermissionsOfAdmin(currentUserId:number) {
    try {
      await this._commonService.presentLoading();
      this.viewModel.initialAddModePermissionCompanyDetailId = this.company.id;
      if (this.viewModel.initialAddModePermissionCompanyDetailId > 0) {
        this._commonService.layoutVM.isAddMode = true;
      } else {
        this._commonService.layoutVM.isAddMode = false;
      }
      let clientCompanyDetailId = this._commonService.layoutVM.isAddMode
        ? this.viewModel.initialAddModePermissionCompanyDetailId
        : this.viewModel.company.id;
      let roleTypeEnum: any = RoleTypeSM.ClientAdmin;
      let userId=currentUserId;
      let resp =
        await this.modulePermissionService.getAllModulesAndPermissionsForUser(
          clientCompanyDetailId,
          roleTypeEnum,userId
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
        this.viewModel.modulePermissionList = resp.successData;
        this.viewModel.selectAll = this.viewModel.modulePermissionList.every(
          (item) =>
            item.isEnabledForClient &&
            item.view &&
            item.add &&
            item.edit &&
            item.delete
        );
        this.viewModel.selectAll = this.viewModel.selectAll;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  closePopup() {
    this.viewModel.displayStyle = "none";
  }
  //Select all Checkboxes
  selectAllClicked() {
    for (let item of this.viewModel.modulePermissionList) {
      item.isEnabledForClient = this.viewModel.selectAll;
      item.view = this.viewModel.selectAll;
      item.add = this.viewModel.selectAll;
      item.view = this.viewModel.selectAll;
      item.edit = this.viewModel.selectAll;
      item.delete = this.viewModel.selectAll;
    }
  }
  /**
   * Check Uncheck Checkboxes
   */
  checkboxClicked() {
    // Check if any individual permission checkbox is unchecked
    let uncheckedPermissions = this.viewModel.modulePermissionList.filter(
      (item) =>
        !item.isEnabledForClient ||
        !item.view ||
        !item.add ||
        !item.edit ||
        !item.delete
    );
    // If there are any unchecked permissions, uncheck the "Select All" checkbox
    this.viewModel.selectAll = uncheckedPermissions.length === 0;
 // If there are any unchecked permissions, uncheck the "Select All" checkbox and set all individual permissions to false
 if (uncheckedPermissions.length > 0) {
  this.viewModel.selectAll = false;

  // Set all individual permissions to false for the unchecked isEnabledForClient
  uncheckedPermissions.forEach((item) => {
    if(!item.isEnabledForClient){
    item.view = false;
    item.add = false;
    item.edit = false;
    item.delete = false;
  }
  });
} else {
  // If all permissions are checked, check the "Select All" checkbox
  this.viewModel.selectAll = true;
}

  }
  /**
   * This method  is used to determine whether the selection
   *  of items in viewModel.modulePermissionList is in an indeterminate state.
   *  It appears to be checking if there are any items that have at
   * least one permission (isEnabledForClient, view, add, edit, delete) enabled or selected.
   * @returns
   */
  isIndeterminate(): boolean {

    const checkedPermissions = this.viewModel.modulePermissionList.filter(
      (item) =>
        item.isEnabledForClient ||
        item.view ||
        item.add ||
        item.edit ||
        item.delete
    );

    // Return true if some permissions are checked but not all
    return (
      checkedPermissions.length > 0 &&
      checkedPermissions.length < this.viewModel.modulePermissionList.length
    );
  }
  /**Open set permissions for Admin Modal */
  openPermissionsModalForAdmin(id: number) {
    this.viewModel.clientUserId = id;
    if (id > 0) {
      this.viewModel.editMode = true;
      this.viewModel.adminDetailsModal = false;
      this.viewModel.permissionsModal = true;
      this.getModulePermissionsOfAdmin(id);
    } else {
      this.viewModel.editMode = false;
    }
    this.viewModel.displayStyle = "block";
  }
  /**
   * Set Permissions for Company Admin
   *@dev Musaib
   */
  async AddOrUpdateModulePermissionsForUser() {
    const userIds = this.viewModel.modulePermissionList.map(
      (item) => item.clientUserId
    );
    const allUserIdsNull = userIds.every((userId) => userId === null);
    if (allUserIdsNull == true) {
      this.addModulePermissionSForUser();
    } else {
      this.updateModulePermissionSForUser();
    }
  }
  /**
   * Add Permissions Company Admin
   */
  async addModulePermissionSForUser() {
    try {
      let roleType = RoleTypeSM.ClientAdmin;
      let clientCompanyDetailId = this._commonService.layoutVM.isAddMode
        ? this.viewModel.initialAddModePermissionCompanyDetailId
        : this.viewModel.company.id;
      let modulePermissionsList = this.viewModel.modulePermissionList;
      modulePermissionsList.forEach((item) => {
        item.clientCompanyDetailId = clientCompanyDetailId;
        item.roleType = roleType;
        item.clientUserId = this.viewModel.clientUserId;
        item.id = 0;
      });
      let resp = await this.modulePermissionService.addModulePermissionsForUser(
        modulePermissionsList
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
        await this.closePopup();
        await this.loadPageData();
        await this._commonService.ShowToastAtTopEnd(
          "Permissions Added Successfully",
          "success"
        );
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  /**
   * Update Permissions Company Admin
   */
  async updateModulePermissionSForUser() {
    try {
      let roleType = RoleTypeSM.ClientAdmin;
      let clientCompanyDetailId = this._commonService.layoutVM.isAddMode
        ? this.viewModel.initialAddModePermissionCompanyDetailId
        : this.viewModel.company.id;
      let modulePermissionsList = this.viewModel.modulePermissionList;
      modulePermissionsList.forEach((item) => {
        item.clientCompanyDetailId = clientCompanyDetailId;
        item.roleType = roleType;
        item.clientUserId = this.viewModel.clientUserId;
      });
      let resp =
        await this.modulePermissionService.updateModulePermissionsForUser(
          modulePermissionsList
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
        await this.closePopup();
        await this.loadPageData();
        await this._commonService.ShowToastAtTopEnd(
          "Permissions Updated Successfully",
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
