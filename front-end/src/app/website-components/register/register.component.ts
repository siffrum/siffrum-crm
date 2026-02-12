import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../components/base.component';
import { CommonService } from 'src/app/services/common.service';
import { LogHandlerService } from 'src/app/services/log-handler.service';
import { RegisterViewModel } from 'src/app/view-models/register.viewmodel';
import { NgForm } from '@angular/forms';
import { SuperCompanyService } from 'src/app/services/super-company.service';
import { EmployeeStatusSM } from 'src/app/service-models/app/enums/employee-status-s-m.enum';
import { RoleTypeSM } from 'src/app/service-models/app/enums/role-type-s-m.enum';
import { LoginStatusSM } from 'src/app/service-models/app/enums/login-status-s-m.enum';
import { Router } from '@angular/router';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.scss'],
    standalone: false
})
export class RegisterComponent extends BaseComponent<RegisterViewModel> implements OnInit {
constructor(commonService:CommonService,logService:LogHandlerService,private superCompanyService:SuperCompanyService,private router:Router){
  super(commonService,logService);
  this.viewModel=new RegisterViewModel();
}
async ngOnInit(){
  this._commonService.layoutVM.toogleWrapper = "loginWrapper";
  this._commonService.layoutVM.showLeftSideMenu = false;
  await this._commonService.presentLoading();
  await setTimeout(async () => {
    await this._commonService.dismissLoader();
  }, 500);
}

  /**Add Company with  Details for new company
   * @return Success
   * @developer Musaib
   */
  async registerNewCompanyAndAdminDetails(companyOverviewForm:NgForm) {
    this.viewModel.formSubmitted=true;
    try {
      if (companyOverviewForm.invalid){
        await this._commonService.showSweetAlertToast({ title:'Form Fields Are Not valid !', icon: 'error' });
        return; // Stop execution here if form is not valid
      }
     await this._commonService.presentLoading();
      let resp = await this.superCompanyService.registerNewCompanyDetails(
        this.viewModel.AddCompanyDetail
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
        if(resp.successData!==null && resp.isError==false ){
          this.viewModel.AddCompanyDetail = resp.successData;
            await this.registerNewCompanyAdmin()
        }

      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  /**
   * register new Client Admin for the company
   * Here we are adding some hard Coded data to the
   *  viz(isEmailConfirmed,isPhoneNumberConfirmed,employeeStatus)
   * @returns success
   * @developer Musaib
   */
  async registerNewCompanyAdmin() {
    this.viewModel.formSubmitted = true;
    try {
      await this._commonService.presentLoading();

      this.viewModel.addAdmin.clientCompanyDetailId =this.viewModel.AddCompanyDetail.id
      this.viewModel.addAdmin.isEmailConfirmed = true;
      this.viewModel.addAdmin.isPhoneNumberConfirmed = true;
      this.viewModel.addAdmin.employeeStatus = EmployeeStatusSM.Active;
      this.viewModel.addAdmin.roleType = RoleTypeSM.ClientAdmin;
      this.viewModel.addAdmin.loginId =
        this.viewModel.addAdmin.firstName + this.viewModel.addAdmin.lastName;
      this.viewModel.addAdmin.passwordHash =
        this.viewModel.addAdmin.lastName + this.viewModel.addAdmin.firstName;
      this.viewModel.addAdmin.loginStatus = LoginStatusSM.Enabled;
      let resp = await this.superCompanyService.registerNewCompanyAdmin(
        this.viewModel.addAdmin
      );
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this._commonService.showInfoOnAlertWindowPopup(
          "success",
          "Added Admin Successfully Please Note the Login Credentials for this User:",
          `RoleType:ClientAdmin
           CompanyCode:${this.viewModel.AddCompanyDetail.companyCode}
           Username: ${this.viewModel.addAdmin.loginId}
           Password: ${this.viewModel.addAdmin.passwordHash}
           `
        );
        this.viewModel.addAdmin = resp.successData;
        this.router.navigate(["/login"]);
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }


}
