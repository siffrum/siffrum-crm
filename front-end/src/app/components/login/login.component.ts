import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { RoleTypeSM } from "src/app/service-models/app/enums/role-type-s-m.enum";
import { AccountService } from "src/app/services/account.service";
import { CommonService } from "src/app/services/common.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { LoginViewModel } from "src/app/view-models/login.viewmodel";
import { BaseComponent } from "../base.component";
import { LicenseInfoService } from "src/app/services/license-info.service";
import { AppConstants } from "src/app-constants";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.scss"],
  standalone: false
})
export class LoginComponent extends BaseComponent<LoginViewModel> implements OnInit {
  /**
   *@Dev Musaib
   */

  isDark = false;

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private accountService: AccountService,
    private router: Router,
    private licenseInfoService: LicenseInfoService
  ) {
    super(commonService, logService);
    this.viewModel = new LoginViewModel();
  }

  async ngOnInit(): Promise<void> {
    this._commonService.layoutVM.toogleWrapper = "loginWrapper";
    this._commonService.layoutVM.showLeftSideMenu = false;

    // ✅ Apply SAME theme as landing page first
    await this._commonService.loadDefaultTheme();

    // ✅ Now sync the page state from actual global theme class
    this.isDark =
      document.documentElement.classList.contains("dark") ||
      document.body.classList.contains("dark");

    await this._commonService.presentLoading();
    await setTimeout(async () => {
      await this._commonService.dismissLoader();
    }, 1000);

    this.viewModel.listRoles = this._commonService
      .EnumToStringArray(RoleTypeSM)
      .filter((item) => item === "ClientAdmin" || item === "ClientEmployee");

    this._exceptionHandler.logObject(this.viewModel);
  }

  // ✅ Theme toggle (updates localStorage + global class so it matches landing)
  toggleTheme(): void {
    this.isDark = !this.isDark;
    localStorage.setItem("internal_theme", this.isDark ? "dark" : "light");

    document.documentElement.classList.toggle("dark", this.isDark);
    document.body.classList.toggle("dark", this.isDark);
  }

  goBack(): void {
    window.history.back();
  }

  isFormValid(): boolean {
    let companyCode = !!this.viewModel.tokenRequest.companyCode;
    let selectRole = !!this.viewModel.tokenRequest.roleType;
    let loginId = !!this.viewModel.tokenRequest.loginId;
    let password = !!this.viewModel.tokenRequest.password;
    return companyCode && selectRole && loginId && password;
  }

  async login() {
    try {
      if (!this.isFormValid()) {
        await this._commonService.showSweetAlertToast({
          title: "Please fill All The Required Fields",
          icon: "error"
        });
        return;
      }

      await this._commonService.presentLoading();

      let resp = await this.accountService.generateToken(
        this.viewModel.tokenRequest,
        this.viewModel
      );

      if (resp.isError) {
        this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: resp.errorData.displayMessage,
          icon: "error"
        });
      } else if (resp.successData.accessToken != null) {
        let loginStatus = resp.successData.loginUserDetails.loginStatus.toString();

        if (loginStatus == "PasswordResetRequired") {
          this.router.navigate(["/changePassword"]);
        } else {
          this._commonService.layoutVM.showLeftSideMenu = true;
          this._commonService.layoutVM.toogleWrapper = "wrapper";
          this._commonService.layoutVM.loggedUserName = resp.successData.loginUserDetails.loginId;

          let mineUserActiveLicenseResponse =
            await this.licenseInfoService.getUserMineActiveLicenseInfo();

          this.viewModel.mineUserActiveLicense = mineUserActiveLicenseResponse.successData;

          if (this.viewModel.mineUserActiveLicense != null) {
            this.router.navigate([AppConstants.WebRoutes.DASHBOARD]);
            await this._commonService.ShowToastAtTopEnd("Login Successful", "success");
          } else if (this.viewModel.mineUserActiveLicense == null) {
            this.router.navigate([AppConstants.WebRoutes.LICENSE]);
            await this._commonService.ShowToastAtTopEnd("Buy License First", "info");
          }
        }
      }
    } catch (error) {
      this._commonService.showInfoOnAlertWindowPopup(
        "error",
        "Login Not Successfull Please Check Login Credentials",
        `Check credentials `
      );
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  async togglePassword() {
    this.viewModel.hide = !this.viewModel.hide;
    if (this.viewModel.eyeDefault == "default") {
      this.viewModel.eyeDefault = "eyeChange";
    } else {
      this.viewModel.eyeDefault = "default";
    }
    return;
  }

  async openForgotPasswordModal() {
    this.router.navigate(["/forgotPassword"]);
  }
}
