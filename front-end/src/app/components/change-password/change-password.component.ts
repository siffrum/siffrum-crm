import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../base.component';
import { ChangePasswordViewModel } from 'src/app/view-models/change-password.viewmodel';
import { CommonService } from 'src/app/services/common.service';
import { LogHandlerService } from 'src/app/services/log-handler.service';
import { AccountService } from 'src/app/services/account.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-change-password',
    templateUrl: './change-password.component.html',
    styleUrls: ['./change-password.component.scss'],
    standalone: false
})
export class ChangePasswordComponent extends BaseComponent<ChangePasswordViewModel> implements OnInit{
  constructor(  commonService: CommonService,
    logService: LogHandlerService,
    private accoutService:AccountService,
    private router:Router){
    super(commonService, logService);
    this.viewModel = new ChangePasswordViewModel()
  }
  ngOnInit(): void {
    this.loadPageData();
  }

  override async loadPageData() {
    try {
      await this._commonService.presentLoading();
      let resp = await this.accoutService.getUserName();
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.updateCredentials = resp.successData;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  //Check Form Validations
  isFormValid():boolean{
    let oldPassword=!!this.viewModel.updateCredentials.oldPassword;
    let newPassword=!!this.viewModel.newPassword;
    let confirmPassword=!!this.viewModel.updateCredentials.newPassword
    return(oldPassword&&newPassword&&confirmPassword)
  }
  /**
   * Save Password
   * @returns Succes
   */
  async saveNewPassword() {
    try {
      if (!this.isFormValid()) {
        // Check if any required field is empty
        await this._commonService.showSweetAlertToast({ title:'Please fill All The Required Fields', icon: 'error' });
       return; // Stop execution here if form is not valid
     }
     if(this.viewModel.newPassword !=this.viewModel.updateCredentials.newPassword){
      await this._commonService.showSweetAlertToast({ title:'New Password And Confirm Password Does Not Match', icon: 'error' });
      this.viewModel.newPassword='';
      this.viewModel.updateCredentials.newPassword='';
      return
     }
      let resp = await this.accoutService.changePassword(
        this.viewModel.updateCredentials
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
          "Password Updated Successfully",
          "success"
        );
        this.accoutService.logoutUser();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Show/Hide password
   * @returns
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
