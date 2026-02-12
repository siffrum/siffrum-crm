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
  constructor
    (
      commonService: CommonService,
      logService: LogHandlerService,
      private accountService: AccountService,
    ) {
    super(commonService, logService)
    this.viewModel = new ForgotPasswordViewModel();
    this._commonService.layoutVM.toogleWrapper = "loginWrapper";

  }

  ngOnInit(): void {

  }
  /**
   *
   * @param ForgotPasswordForm
   * @returns
     *@Dev Musaib
   * 
   */
  async click_forgotPassword(ForgotPasswordForm: NgForm) {
    this.viewModel.FormSubmitted = true;
    try {
      await this._commonService.presentLoading();
      if (ForgotPasswordForm.invalid)
        return;
      let resp = await this.accountService.Send_forgotPassword(this.viewModel.forgotPasswordDetails);
      if (resp.isError) {
        this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({ title: resp.errorData.displayMessage, icon: 'error' });
        return;
      }
      this.viewModel.forgotPasswordDetails = resp.successData;
      // this.activeModal.close();
      this._commonService.showSweetAlertToast({ icon: 'info', title: 'Information', text: "Reset Password Link has been sent Successfully" });
    } catch (error) {
      this._commonService.showSweetAlertToast({ title: AppConstants.ErrorPrompts.Unknown_Error, icon: 'error' });
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
}