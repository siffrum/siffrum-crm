import { Component, OnInit } from '@angular/core';
import { ForgotPasswordViewModel } from 'src/app/view-models/forgotpassword.viewmodel';
import { BaseComponent } from '../../base.component';
import { CommonService } from 'src/app/services/common.service';
import { LogHandlerService } from 'src/app/services/log-handler.service';
import { AccountService } from 'src/app/services/account.service';
import { NgForm } from '@angular/forms';
import { AppConstants } from 'src/app-constants';

@Component({
  selector: 'app-forgotpassword',
  templateUrl: './forgotpassword.component.html',
  styleUrls: ['./forgotpassword.component.scss'],
  standalone: false
})
export class ForgotpasswordComponent extends BaseComponent<ForgotPasswordViewModel> implements OnInit {

  /** UI flags */
  isLoading: boolean = false;
  isSent: boolean = false;

  /** Theme state (used only for the optional toggle button + [ngClass]) */
  isDark: boolean = false;

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private accountService: AccountService,
  ) {
    super(commonService, logService);
    this.viewModel = new ForgotPasswordViewModel();

    // keep wrapper same as login
    this._commonService.layoutVM.toogleWrapper = "loginWrapper";
  }

  async ngOnInit(): Promise<void> {
    // Apply SAME theme logic as landing/login
    await this._commonService.loadDefaultTheme();

    // Sync local theme state from global DOM class
    this.syncThemeFromDom();
  }

  /** Reads theme from global DOM (html/body). */
  private syncThemeFromDom(): void {
    this.isDark =
      document.documentElement.classList.contains('dark') ||
      document.body.classList.contains('dark');
  }

  /** Optional theme toggle (if you keep the toggle button in HTML). */
  toggleTheme(): void {
    this.isDark = !this.isDark;

    // keep your key
    localStorage.setItem('internal_theme', this.isDark ? 'dark' : 'light');

    // apply globally so ALL pages match
    document.documentElement.classList.toggle('dark', this.isDark);
    document.body.classList.toggle('dark', this.isDark);
  }

  goBack(): void {
    window.history.back();
  }

  async click_forgotPassword(ForgotPasswordForm: NgForm) {
    this.viewModel.FormSubmitted = true;
    this.isSent = false;

    try {
      this.isLoading = true;

      await this._commonService.presentLoading();

      if (ForgotPasswordForm.invalid) return;

      let resp = await this.accountService.Send_forgotPassword(this.viewModel.forgotPasswordDetails);

      if (resp.isError) {
        this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: resp.errorData.displayMessage,
          icon: 'error'
        });
        return;
      }

      this.viewModel.forgotPasswordDetails = resp.successData;
      this.isSent = true;

      this._commonService.showSweetAlertToast({
        icon: 'info',
        title: 'Information',
        text: "Reset Password Link has been sent Successfully"
      });

    } catch (error) {
      this._commonService.showSweetAlertToast({
        title: AppConstants.ErrorPrompts.Unknown_Error,
        icon: 'error'
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
