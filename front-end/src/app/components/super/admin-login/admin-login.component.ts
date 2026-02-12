import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { AppConstants } from "src/app-constants";
import { AuthGuard } from "src/app/guard/auth.guard";
import { RoleTypeSM } from "src/app/service-models/app/enums/role-type-s-m.enum";
import { LoginUserSM } from "src/app/service-models/app/v1/app-users/login/login-user-s-m";
import { AccountService } from "src/app/services/account.service";
import { CommonService } from "src/app/services/common.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { StorageService } from "src/app/services/storage.service";
import { AdminLoginViewModel } from "src/app/view-models/admin-login.viemodel";
import { BaseComponent } from "../../base.component";
import { ClientThemeSM } from "src/app/service-models/app/v1/client/client-theme-s-m";
import { SettingService } from "src/app/services/setting.service";

@Component({
    selector: "app-admin-login",
    templateUrl: "./admin-login.component.html",
    styleUrls: ["./admin-login.component.scss"],
    standalone: false
})
export class AdminLoginComponent
  extends BaseComponent<AdminLoginViewModel>
  implements OnInit {

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private loginService: AccountService,
    private router: Router,
    private authGuard: AuthGuard,
    private storageService: StorageService,
  ) {
    super(commonService, logService);
    this.viewModel = new AdminLoginViewModel();
  }

  async ngOnInit() {
    this._commonService.layoutVM.showLeftSideMenu = false;
    this._commonService.layoutVM.toogleWrapper = "loginWrapper";
    await this._commonService.loadDefaultTheme();
    // this.viewModel.listRoles = this._commonService.EnumToStringArray(RoleTypeSM);
    // this._exceptionHandler.logObject(this.viewModel);
    let userValid = await this.authGuard.IsTokenValid();
    if (userValid) {
      let user: LoginUserSM | "" = await this.storageService.getFromStorage(
        AppConstants.DbKeys.LOGIN_ADMIN
      );
      this.router.navigate(['admin/dashboard']);
      if (user !== "") {
        this.viewModel.tokenRequest.loginId = user.loginId;
        this.viewModel.tokenRequest.password =
          await this.storageService.getFromStorage(
            AppConstants.DbKeys.PASSWORD
          );
      }
    }
  }
  /**Super Admin login
   * @returns LOGIN Success
   * @developer Musaib
   */

  async login() {
    try {
      await this._commonService.presentLoading();
      this.viewModel.tokenRequest.roleType = RoleTypeSM.SuperAdmin;
      let resp = await this.loginService.generateAdminLoginToken(
        this.viewModel.tokenRequest,
        this.viewModel
      );
      if (resp.isError) {
        this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else if (resp.successData.accessToken != null) {
        this._commonService.layoutVM.showLeftSideMenu = true;
        this._commonService.layoutVM.toogleWrapper = "wrapper";
        this._commonService.layoutVM.loginUser = resp.successData.loginUserDetails.loginId
        this.router.navigate([AppConstants.WebRoutes.ADMIN.DASHBOARD]);
        await this._commonService.ShowToastAtTopEnd(
          "Login Successful",
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
 * Show/hide password
 * @developer Musaib

 */
  async togglePassword() {
    this.viewModel.hide = !this.viewModel.hide;
    if (this.viewModel.eyeDefault == "default") {
      this.viewModel.eyeDefault = "eyeChange";
    } else {
      this.viewModel.eyeDefault = "default";
    }
    return;
  }
}
