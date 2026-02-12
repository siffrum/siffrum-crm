import { Component, OnInit } from '@angular/core';
import { ResetPasswordViewModel } from 'src/app/view-models/resetpassword.viewmodel';
import { BaseComponent } from '../../base.component';
import { CommonService } from 'src/app/services/common.service';
import { LogHandlerService } from 'src/app/services/log-handler.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';
import { AppConstants } from 'src/app-constants';
import { NgForm } from '@angular/forms';
import { ResetPasswordRequestSM } from 'src/app/service-models/app/v1/app-users/reset-password-request-s-m';
import { ValidatePasswordLinkStatusSM } from 'src/app/service-models/app/enums/validate-password-link-status-s-m.enum';

@Component({
    selector: 'app-resetpassword',
    templateUrl: './resetpassword.component.html',
    styleUrls: ['./resetpassword.component.scss'],
    standalone: false
})
export class ResetpasswordComponent extends BaseComponent<ResetPasswordViewModel> implements OnInit {
  ValidatePasswordLinkStatusSM = ValidatePasswordLinkStatusSM;

  constructor(commonService: CommonService,
    logService: LogHandlerService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private accountService: AccountService
  ) {
    super(commonService, logService);
    this.viewModel = new ResetPasswordViewModel();
    this._commonService.layoutVM.toogleWrapper = "loginWrapper";
  }
  /**
   * Get [authCode] on page load
   * @returns
   *@Dev Musaib
   */
  async ngOnInit() {
    try {
      await this._commonService.presentLoading();
      let info = this.activatedRoute.snapshot.queryParams['authCode'];
      let resp = await this.accountService.validateAuthCode(info);
      if (resp.isError) {
        this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({ title: resp.errorData.displayMessage, icon: 'error' });
        return;
      } else {
        this.viewModel.validateAuthCode = resp.successData.intResponse;
        if (this.viewModel.validateAuthCode === ValidatePasswordLinkStatusSM.Invalid) {
          this._commonService.showSweetAlertToast({ title: 'Password reset link expired', icon: 'error' });
          this.router.navigate([`${AppConstants.WebRoutes.LOGIN}`]);
          return;
        } else if (this.viewModel.validateAuthCode === ValidatePasswordLinkStatusSM.Valid) {
          this._commonService.showSweetAlertToast({ title: 'Please continue to reset your password', icon: 'success' });
          return;
        }
      }

    } catch (error) {
      this._commonService.showSweetAlertToast({ title: AppConstants.ErrorPrompts.Unknown_Error, icon: 'error' });
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
/**
 * Reset New Password here we are using decode decodeURI() to decode  an [authCode]
 * @param resetPasswordForm
 * @returns
   *@Dev Musaib
 */
  async click_resetPassword(resetPasswordForm: NgForm) {
    this.viewModel.FormSubmitted = true;
    try {
      await this._commonService.presentLoading();
      if (resetPasswordForm.invalid || this.viewModel.resetPasswordDetails.newPassword !== this.viewModel.confirmPassword ) {
        this._commonService.showSweetAlertToast({ title: AppConstants.ErrorPrompts.Password_Wrong, icon: 'error' });
        return;
      }
      let info = this.activatedRoute.snapshot.queryParams['authCode'];
      const resetPasswordRequest: ResetPasswordRequestSM = new ResetPasswordRequestSM();
      resetPasswordRequest.authCode = decodeURI(info) ?? '';
      resetPasswordRequest.newPassword = this.viewModel.resetPasswordDetails.newPassword;
      let resp = await this.accountService.UpdatePassword(resetPasswordRequest);
      if (resp.isError) {
        this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({ title: resp.errorData.displayMessage, icon: 'error' });
        return;
      }
      this.viewModel.resetPasswordDetails = resp.successData;
      this._commonService.showSweetAlertToast({ title: 'Password updated Sucessfully', icon: 'success' });
      this.router.navigate([`${AppConstants.WebRoutes.LOGIN}`]);
    } catch (error) {
      this._commonService.showSweetAlertToast({ title: AppConstants.ErrorPrompts.Unknown_Error, icon: 'error' });
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