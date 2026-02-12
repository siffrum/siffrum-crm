import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { BaseComponent } from "src/app/components/base.component";
import { ModuleNameSM } from "src/app/service-models/app/enums/module-name-s-m.enum";
import { RoleTypeSM } from "src/app/service-models/app/enums/role-type-s-m.enum";
import { AccountService } from "src/app/services/account.service";
import { CommonService } from "src/app/services/common.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { SideMenuService } from "src/app/services/side-menu.service";
import { SqlReportService } from "src/app/services/sql-report.service";
import { SideMenuViewModel } from "src/app/view-models/side-menu.viewmodel";
@Component({
    selector: "app-side-menu",
    templateUrl: "./side-menu.component.html",
    styleUrls: ["./side-menu.component.scss"],
    standalone: false
})
export class SideMenuComponent
  extends BaseComponent<SideMenuViewModel>
  implements OnInit {
  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private sideMenuService: SideMenuService,
    private accountService: AccountService,
    private router: Router,
    private sqlReportService: SqlReportService
  ) {
    super(commonService, logService);
    this.viewModel = new SideMenuViewModel();
    this._commonService.layoutVM.loggedUserName;
  }
  async ngOnInit() {
    await this.getUserProfilePicture();
    await this.getModulePermission();
    await this.getAllSqlReportsForAdmin();
  }

  // Function to filter the "Reports" sub-items

  async toggleSubItems(menuItem: any) {
    // Toggle the "Reports" sub-items
    if (menuItem.moduleName === ModuleNameSM.Reports) {
      menuItem.showSubItems = !menuItem.showSubItems;
    } else {
      // Hide the "Reports" sub-items if another menu item is clicked
      this._commonService.layoutVM.sideMenuItems.forEach(item => {
        if (item.moduleName === ModuleNameSM.Reports) {
          item.showSubItems = false;
        }
      });

    }
  }

  hideSideMenu() {
    this._commonService.layoutVM.toggleSideMenu = "default";
    this._commonService.layoutVM.toogleWrapper = "wrapper";
  }

  // reDirectToDashboard() {
  //   let role = this._commonService.layoutVM.tokenRole;
  //   if (role == RoleTypeSM.ClientAdmin || role == RoleTypeSM.ClientEmployee) {
  //     this.router.navigate(["/dasboard"]);
  //   } else if (
  //     role == RoleTypeSM.SuperAdmin ||
  //     role == RoleTypeSM.SystemAdmin
  //   ) {
  //     this.router.navigate(["admin/dasboard"]);
  //   } else {
  //     this.router.navigate(["**"]);
  //   }
  // }
  /**Get User profile picture */
  async getUserProfilePicture() {
    try {

      let resp = await this.sideMenuService.GetUserProfilePictue();
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.userProfilePic = resp.successData;
      }
    }
    catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Upload Profile Picture
   * @param event
   */
  async uploadUserProfilePicture(event: any) {
    try {
      await this._commonService.presentLoading();
      this._commonService
        .convertFileToBase64(event.target.files[0])
        .subscribe(async (base64) => {
          let fileName = event.target.files[0].name;
          fileName.split("?")[0].split(".").pop();
          this.viewModel.userProfilePic = base64;
          if (
            this.viewModel.userProfilePic !== "" ||
            this.viewModel.userProfilePic !== undefined
          ) {
            let resp = await this.sideMenuService.AddUserProfilePicture(
              this.viewModel.userProfilePic
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
                "Profile Picture Updated Successfully",
                "success"
              );
              await this.getUserProfilePicture(); // fetch the updated picture from the server
              return;
            }
          }
        });
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  hasAttendanceModule(): boolean {
    return this._commonService.layoutVM.sideMenuItems.some(menuItem => menuItem.itemName === 'Attendance');
  }
  async getModulePermission(): Promise<ModuleNameSM | any> {
    this._commonService.layoutVM.sideMenuItems.forEach(async (element) => {
      let permission = await this.accountService.getMyModulePermissions(
        element.moduleName
      );
      if (permission) {
        if (permission.isStandAlone)
          element.permission = permission.isEnabledForClient;
        else
          element.permission = permission.isEnabledForClient && permission.view;
      }
    });
  }
  /**
   * @Dev Musaib
   * Get SQL Report for ADMIN By Selected Report Name;
   *
   */
  async getAllSqlReportsForAdmin() {
    try {
      await this._commonService.presentLoading();
      let resp =
        await this.sqlReportService.getAllSqlReportsForAdmin();
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.sqlReportList = resp.successData;
        // console.log(this.viewModel.sqlReportList)
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  selectedReportName(selectedReportName: string) {
    const selectedReport = this.viewModel.sqlReportList.find(
      (item) => item.reportName === selectedReportName
    );
    if (selectedReport) {
      this.viewModel.sqlReportObj.id = selectedReport.id;
      this.router.navigate(['/sql-report', { id: this.viewModel.sqlReportObj.id, selectedReportName: selectedReport.reportName }]);
    }
  }
}
