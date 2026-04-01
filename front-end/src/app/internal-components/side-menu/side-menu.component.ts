import { Component, OnDestroy, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Subscription } from "rxjs";

import { BaseComponent } from "src/app/components/base.component";
import { RoleTypeSM } from "src/app/service-models/app/enums/role-type-s-m.enum";
import { ModuleNameSM } from "src/app/service-models/app/enums/module-name-s-m.enum";
import { AccountService } from "src/app/services/account.service";
import { CommonService } from "src/app/services/common.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { NotificationStateService } from "src/app/services/notification-state.service";
import { SideMenuService } from "src/app/services/side-menu.service";
import { SqlReportService } from "src/app/services/sql-report.service";
import { SideMenuViewModel } from "src/app/view-models/side-menu.viewmodel";

@Component({
  selector: "app-side-menu",
  templateUrl: "./side-menu.component.html",
  styleUrls: ["./side-menu.component.scss"],
  standalone: false,
})
export class SideMenuComponent
  extends BaseComponent<SideMenuViewModel>
  implements OnInit, OnDestroy
{
  unreadNotificationsCount = 0;
  private unreadSub?: Subscription;

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private sideMenuService: SideMenuService,
    private accountService: AccountService,
    private router: Router,
    private sqlReportService: SqlReportService,
    private notificationStateService: NotificationStateService
  ) {
    super(commonService, logService);
    this.viewModel = new SideMenuViewModel();
  }

  async ngOnInit() {
    await this.getUserProfilePicture();
    await this.getModulePermission();
    await this.getAllSqlReportsForAdmin();

    this.notificationStateService.startPolling();

    this.unreadSub = this.notificationStateService.unreadCount$.subscribe(
      (count) => {
        this.unreadNotificationsCount = count;
      }
    );

    await this.notificationStateService.refreshUnreadCount();
  }

  ngOnDestroy(): void {
    this.unreadSub?.unsubscribe();
  }

  async toggleSubItems(menuItem: any) {
    if (menuItem.moduleName === ModuleNameSM.Reports) {
      menuItem.showSubItems = !menuItem.showSubItems;
    } else {
      this._commonService.layoutVM.sideMenuItems.forEach((item) => {
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

  async getUserProfilePicture() {
    try {
      let resp = await this.sideMenuService.GetUserProfilePictue();
      if (!resp.isError) {
        this.viewModel.userProfilePic = resp.successData;
      }
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  async uploadUserProfilePicture(event: any) {
    try {
      await this._commonService.presentLoading();

      this._commonService
        .convertFileToBase64(event.target.files[0])
        .subscribe(async (base64) => {
          this.viewModel.userProfilePic = base64;

          if (this.viewModel.userProfilePic) {
            let resp = await this.sideMenuService.AddUserProfilePicture(
              this.viewModel.userProfilePic
            );

            if (!resp.isError) {
              await this._commonService.ShowToastAtTopEnd(
                "Profile Picture Updated Successfully",
                "success"
              );
              await this.getUserProfilePicture();
            }
          }
        });
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  async getModulePermission() {
    this._commonService.layoutVM.sideMenuItems.forEach(async (element) => {
      let permission = await this.accountService.getMyModulePermissions(
        element.moduleName
      );

      if (permission) {
        if (permission.isStandAlone) {
          element.permission = permission.isEnabledForClient;
        } else {
          element.permission =
            permission.isEnabledForClient && permission.view;
        }
      }
    });
  }

  async getAllSqlReportsForAdmin() {
    try {
      await this._commonService.presentLoading();
      let resp = await this.sqlReportService.getAllSqlReportsForAdmin();

      if (!resp.isError) {
        this.viewModel.sqlReportList = resp.successData;
      }
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  selectedReportName(selectedReportName: string) {
    const selectedReport = this.viewModel.sqlReportList.find(
      (item) => item.reportName === selectedReportName
    );

    if (selectedReport) {
      this.router.navigate([
        "/sql-report",
        {
          id: selectedReport.id,
          selectedReportName: selectedReport.reportName,
        },
      ]);
      this.hideSideMenu();
    }
  }

  isNotificationsItem(menuItem: any): boolean {
    const name = (menuItem?.itemName || "").toLowerCase().trim();
    return name === "notifications";
  }

  private isDashboardViewItem(menuItem: any): boolean {
    const name = (menuItem?.itemName || "").toLowerCase().trim();
    return ["home", "reports", "analytics", "payroll"].includes(name);
  }

  private getDashboardViewKey(menuItem: any): string {
    const name = (menuItem?.itemName || "").toLowerCase().trim();

    if (name === "home") return "home";
    if (name === "reports") return "reports";
    if (name === "analytics") return "analytics";
    if (name === "payroll") return "payroll";

    return "home";
  }

  getMenuRouterLink(menuItem: any): any[] | string {
    const name = (menuItem?.itemName || "").toLowerCase().trim();

    if (name === "settings") return ["/setting"];
    if (name === "crm") return ["/internal"];
    if (name === "notifications") return ["/notifications"];

    if (this.isDashboardViewItem(menuItem)) {
      return ["/dashboard"];
    }

    return menuItem?.itemRoute;
  }

  getMenuQueryParams(menuItem: any): any {
    const name = (menuItem?.itemName || "").toLowerCase().trim();

    if (name === "settings") return null;
    if (name === "crm") return null;
    if (name === "notifications") return null;

    if (this.isDashboardViewItem(menuItem)) {
      return {
        view: this.getDashboardViewKey(menuItem),
      };
    }

    return null;
  }

  isSuperAdmin(): boolean {
    return (
      this._commonService.layoutVM.tokenRole === RoleTypeSM.SuperAdmin ||
      this._commonService.layoutVM.tokenRole === RoleTypeSM.SystemAdmin
    );
  }

  getBrandLabel(): string {
    return this.isSuperAdmin() ? "Super Admin" : "Internal.";
  }

  getBrandSubLabel(): string {
    return this.isSuperAdmin() ? "Control Center" : "Workspace";
  }

  getDisplayUserName(): string {
    return this._commonService.layoutVM.loggedUserName || (this.isSuperAdmin() ? "Administrator" : "User");
  }
}
