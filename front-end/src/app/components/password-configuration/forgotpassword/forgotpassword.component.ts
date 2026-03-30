import { Component, OnInit } from "@angular/core";
import { NgForm } from "@angular/forms";
import { AppConstants } from "src/app-constants";
import { CommonService } from "src/app/services/common.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { AccountService } from "src/app/services/account.service";
import { ForgotPasswordViewModel } from "src/app/view-models/forgotpassword.viewmodel";
import { BaseComponent } from "../../base.component";

@Component({
  selector: "app-forgotpassword",
  templateUrl: "./forgotpassword.component.html",
  styleUrls: ["./forgotpassword.component.scss"],
  standalone: false,
})
export class ForgotpasswordComponent
  extends BaseComponent<ForgotPasswordViewModel>
  implements OnInit
{
  isLoading: boolean = false;
  isSent: boolean = false;
  isDark: boolean = false;

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private accountService: AccountService
  ) {
    super(commonService, logService);
    this.viewModel = new ForgotPasswordViewModel();
    this._commonService.layoutVM.toogleWrapper = "loginWrapper";
  }

  async ngOnInit(): Promise<void> {
    await this._commonService.loadDefaultTheme();
    this.syncThemeFromDom();
  }

  private syncThemeFromDom(): void {
    this.isDark =
      document.documentElement.classList.contains("dark") ||
      document.body.classList.contains("dark");
  }

  toggleTheme(): void {
    this.isDark = !this.isDark;
    localStorage.setItem("internal_theme", this.isDark ? "dark" : "light");
    document.documentElement.classList.toggle("dark", this.isDark);
    document.body.classList.toggle("dark", this.isDark);
  }

  goBack(): void {
    window.history.back();
  }

  async click_forgotPassword(forgotPasswordForm: NgForm): Promise<void> {
    this.viewModel.FormSubmitted = true;
    this.isSent = false;

    try {
      if (forgotPasswordForm.invalid) {
        Object.values(forgotPasswordForm.controls).forEach((control) => {
          control.markAsTouched();
        });
        return;
      }

      this.isLoading = true;
      await this._commonService.presentLoading();

      // Swagger requires expiry in request body
      this.viewModel.forgotPasswordDetails.expiry = new Date();

      const resp = await this.accountService.Send_forgotPassword(
        this.viewModel.forgotPasswordDetails
      );

      if (!resp || resp.isError) {
        this._exceptionHandler.logObject(resp?.errorData);
        await this._commonService.showSweetAlertToast({
          title: resp?.errorData?.displayMessage || "Failed to send reset link",
          icon: "error",
        });
        return;
      }

      this.viewModel.forgotPasswordDetails = resp.successData;
      this.isSent = true;

      await this._commonService.showSweetAlertToast({
        icon: "success",
        title: "Reset link sent successfully",
      });
    } catch (error) {
      await this._commonService.showSweetAlertToast({
        title: AppConstants.ErrorPrompts.Unknown_Error,
        icon: "error",
      });
      throw error;
    } finally {
      this.isLoading = false;
      await this._commonService.dismissLoader();
    }
  }

  resend(): void {
    this.isSent = false;
    this.viewModel.FormSubmitted = false;
  }
}