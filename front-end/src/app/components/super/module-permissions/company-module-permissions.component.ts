import { Component, Input, OnInit } from "@angular/core";
import { BaseComponent } from "../../base.component";
import { ModulePermissionsViewModel } from "src/app/view-models/module-permissions.viewmodel";
import { CommonService } from "src/app/services/common.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { ActivatedRoute } from "@angular/router";
import { RoleTypeSM } from "src/app/service-models/app/enums/role-type-s-m.enum";
import { ClientCompanyDetailSM } from "src/app/service-models/app/v1/client/client-company-detail-s-m";
import { AddCompanyWizards } from "src/app/internal-models/common-models";
import { ModulePermissionsService } from "src/app/services/module-permissions.service";


@Component({
    selector: "app-module-permissions",
    templateUrl: "./module-permissions.component.html",
    styleUrls: ["./company-module-permissions.component.scss"],
    standalone: false
})
export class ModulePermissionsComponent
  extends BaseComponent<ModulePermissionsViewModel>
  implements OnInit
{
  /**
   * @dev Musaib
   * here difference b/w clientCompanyDetailid and initialAddModePermissionCompanyDetailId   is when we initially add any company initially we need to use response of
   * newly created company Id  so we get that from shared Service.
   * Add ModulePermisions For Different roles
   */
  //Get response from add company componant using @input
  @Input() company: ClientCompanyDetailSM = new ClientCompanyDetailSM();
  constructor(
    _commonService: CommonService,
    logService: LogHandlerService,
    private activatedRoute: ActivatedRoute,
    private modulePermissionService: ModulePermissionsService
  ) {
    super(_commonService, logService);
    this.viewModel = new ModulePermissionsViewModel();
  }

  async ngOnInit() {
    this.viewModel.listRoles = [
      RoleTypeSM[RoleTypeSM.ClientAdmin],
      RoleTypeSM[RoleTypeSM.ClientEmployee],
    ];
    let Id = Number(this.activatedRoute.snapshot.paramMap.get("Id"));
    this.viewModel.company.id = Id;
    if (Id == undefined) {
      await this._commonService.ShowToastAtTopEnd(
        "Something Went Wrong",
        "error"
      );
    } else {
      this._commonService.layoutVM.isAddMode = false;
      this.viewModel.switchToAddAdminBtn = false;
    }
  }

  /**Get ROLE OF THE  USER */
  getRole() {
    let roleTypeValue: any = RoleTypeSM[this.viewModel.selectedRole];
    if (roleTypeValue === RoleTypeSM.ClientAdmin) {
      this.viewModel.modulePermissionList =
        this.viewModel.adminModulePermissionslist;
      this.viewModel.tableContent = true;
      this.loadPageData();
    }
    if (roleTypeValue === RoleTypeSM.ClientEmployee) {
      this.viewModel.modulePermissionList =
        this.viewModel.EmployeeModulePermissionslist;
      this.viewModel.tableContent = true;
      this.loadPageData();
    }
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
 if (uncheckedPermissions.length > 0) {
  this.viewModel.selectAll = false;

  // Set all individual permissions to false for the unchecked permissions
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
  /**load permission data based on Selected  role type
   * @dev Musaib
   */
  override async loadPageData() {
    try {
      await this._commonService.presentLoading();
      //here we get this.company.id from addCompany component where we create a new company 
      this.viewModel.initialAddModePermissionCompanyDetailId = this.company.id;
      if (this.viewModel.initialAddModePermissionCompanyDetailId > 0) {
        this._commonService.layoutVM.isAddMode = true;
      } else {
        this._commonService.layoutVM.isAddMode = false;

      }
      let clientCompanyDetailId = this._commonService.layoutVM.isAddMode
        ? this.viewModel.initialAddModePermissionCompanyDetailId
        : this.viewModel.company.id;
      let roleTypeEnum: any = RoleTypeSM[this.viewModel.selectedRole];
      let resp = await this.modulePermissionService.getAllModulesAndPermissions(
        clientCompanyDetailId,
        roleTypeEnum
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
  /**
   * Update Permissions for A User
   *@dev Musaib
   */
  async updateModulePermissions() {
    try {
      let roleType = this.viewModel.selectedRole;
      let clientCompanyDetailId = this._commonService.layoutVM.isAddMode
        ? this.viewModel.initialAddModePermissionCompanyDetailId
        : this.viewModel.company.id;
      let modulePermissionsList = this.viewModel.modulePermissionList;
      modulePermissionsList.forEach((item) => {
        item.clientCompanyDetailId = clientCompanyDetailId;
        item.roleType = roleType;
      });
      let resp =
        await this.modulePermissionService.updateModulePermissions(
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
  /**Update Permissions For Selected Roles
   * @dev Musaib
   */
  async savePermissionsForSelectedUsers() {
    try {
      const clientCompanyDetailId = this._commonService.layoutVM.isAddMode
        ? this.viewModel.initialAddModePermissionCompanyDetailId
        : this.viewModel.company.id;
      if (this.viewModel.adminCheckboxChecked) {
        // Save selected permissions for ClientAdmin role
        this.viewModel.modulePermissions.push(
          ...this.viewModel.modulePermissionList.map((item) => ({
            ...item,
            clientCompanyDetailId,
            roleType: RoleTypeSM.ClientAdmin,
          }))
        );
      }
      if (this.viewModel.employeeCheckboxChecked) {
        // Save selected permissions for ClientEmployee role
        this.viewModel.modulePermissions.push(
          ...this.viewModel.modulePermissionList.map((item) => ({
            ...item,
            clientCompanyDetailId,
            roleType: RoleTypeSM.ClientEmployee,
          }))
        );
      }
      let resp = await this.modulePermissionService.updateModulePermissions(
        this.viewModel.modulePermissions
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
        await this._commonService.ShowToastAtTopEnd(
          "Permissions Updated Successfully",
          "success"
        );
        this.loadPageData();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  // nextWizardLocation(wizardLocation: AddCompanyWizards) {
  //   switch (wizardLocation) {
  //     case AddCompanyWizards.addCompanyInfo:
  //       break;
  //     case AddCompanyWizards.addCompanyAddress:
  //       break;
  //     case AddCompanyWizards.addModules:
  //       break;
  //     case AddCompanyWizards.addCompanyAdminDetails:
  //     default:
  //       break;
  //   }
  //   this._commonService.layoutVM.wizardLocation = wizardLocation;
  // }
  async nextWizardLocation(wizardLocation: AddCompanyWizards) {
    this._commonService.nextTablocations(wizardLocation)
  }
}
