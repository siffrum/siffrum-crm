import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';

import { BaseComponent } from '../../components/base.component';
import { CommonService } from 'src/app/services/common.service';
import { LogHandlerService } from 'src/app/services/log-handler.service';
import { SuperCompanyService } from 'src/app/services/super-company.service';
import { ThemeService } from 'src/app/services/theme.service';

import { RegisterViewModel } from 'src/app/view-models/register.viewmodel';
import { EmployeeStatusSM } from 'src/app/service-models/app/enums/employee-status-s-m.enum';
import { RoleTypeSM } from 'src/app/service-models/app/enums/role-type-s-m.enum';
import { LoginStatusSM } from 'src/app/service-models/app/enums/login-status-s-m.enum';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  standalone: false
})
export class RegisterComponent extends BaseComponent<RegisterViewModel> implements OnInit {

  step = 1;

  // Date constraints
  maxDob!: string; // today - 14 years
  minDob!: string; // today - 80 years
  maxDoj!: string; // today
  minDoj!: string; // today - 5 years (edit if you want)

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private superCompanyService: SuperCompanyService,
    private router: Router,
    public theme: ThemeService
  ) {
    super(commonService, logService);
    this.viewModel = new RegisterViewModel();
  }

  async ngOnInit() {
    this._commonService.layoutVM.toogleWrapper = "loginWrapper";
    this._commonService.layoutVM.showLeftSideMenu = false;

    this.initDateLimits();

    await this._commonService.presentLoading();
    setTimeout(async () => {
      await this._commonService.dismissLoader();
    }, 350);
  }

  private initDateLimits() {
    const today = new Date();
    const iso = (d: Date) => d.toISOString().slice(0, 10);

    const dobMax = new Date(today);
    dobMax.setFullYear(dobMax.getFullYear() - 14);

    const dobMin = new Date(today);
    dobMin.setFullYear(dobMin.getFullYear() - 80);

    const dojMax = new Date(today);

    const dojMin = new Date(today);
    dojMin.setFullYear(dojMin.getFullYear() - 5);

    this.maxDob = iso(dobMax);
    this.minDob = iso(dobMin);
    this.maxDoj = iso(dojMax);
    this.minDoj = iso(dojMin);
  }

  // ---- Wizard navigation ----
  goStep(n: number, form?: NgForm) {
    // prevent jumping forward without validating current step
    if (n > this.step) {
      if (!this.canProceed(this.step, form)) return;
    }
    this.viewModel.formSubmitted = false;
    this.step = n;
  }

  next(form: NgForm) {
    if (!this.canProceed(this.step, form)) return;
    this.viewModel.formSubmitted = false;
    this.step = Math.min(3, this.step + 1);
  }

  prev() {
    this.viewModel.formSubmitted = false;
    this.step = Math.max(1, this.step - 1);
  }

  private canProceed(step: number, form?: NgForm): boolean {
    this.viewModel.formSubmitted = true;

    // If Angular form exists, let it mark controls as touched
    if (form?.controls) {
      Object.values(form.controls).forEach(c => c.markAsTouched());
    }

    if (step === 1) {
      const c = this.viewModel.AddCompanyDetail;
      return !!(
        c?.companyCode &&
        c?.name &&
        c?.companyDateOfEstablishment &&
        c?.companyWebsite &&
        c?.companyContactEmail &&
        c?.companyMobileNumber
      );
    }

    if (step === 2) {
      const a = this.viewModel.addAdmin;
      return !!(
        a?.firstName &&
        a?.lastName &&
        a?.emailId &&
        a?.phoneNumber &&
        a?.dateOfBirth &&
        a?.dateOfJoining
      );
    }

    return true;
  }

  // ---- Submit ----
  async registerNewCompanyAndAdminDetails(companyOverviewForm: NgForm) {
    this.viewModel.formSubmitted = true;

    try {
      // validate steps before submit
      if (!this.canProceed(1, companyOverviewForm) || !this.canProceed(2, companyOverviewForm)) {
        await this._commonService.showSweetAlertToast({ title: 'Please complete required fields.', icon: 'error' });
        return;
      }

      await this._commonService.presentLoading();

      const resp = await this.superCompanyService.registerNewCompanyDetails(this.viewModel.AddCompanyDetail);

      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        await this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
        return;
      }

      if (resp.successData) {
        this.viewModel.AddCompanyDetail = resp.successData;
        await this.registerNewCompanyAdmin();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  async registerNewCompanyAdmin() {
    this.viewModel.formSubmitted = true;

    try {
      await this._commonService.presentLoading();

      this.viewModel.addAdmin.clientCompanyDetailId = this.viewModel.AddCompanyDetail.id;
      this.viewModel.addAdmin.isEmailConfirmed = true;
      this.viewModel.addAdmin.isPhoneNumberConfirmed = true;
      this.viewModel.addAdmin.employeeStatus = EmployeeStatusSM.Active;
      this.viewModel.addAdmin.roleType = RoleTypeSM.ClientAdmin;

      // ⚠️ your current logic makes username/password predictable.
      // Leaving it because you use it, but you should replace with real password creation later.
      this.viewModel.addAdmin.loginId = (this.viewModel.addAdmin.firstName + this.viewModel.addAdmin.lastName).replace(/\s/g, '');
      this.viewModel.addAdmin.passwordHash = (this.viewModel.addAdmin.lastName + this.viewModel.addAdmin.firstName).replace(/\s/g, '');
      this.viewModel.addAdmin.loginStatus = LoginStatusSM.Enabled;

      const resp = await this.superCompanyService.registerNewCompanyAdmin(this.viewModel.addAdmin);

      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        await this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
        return;
      }

      this._commonService.showInfoOnAlertWindowPopup(
        "success",
        "Admin created. Note these credentials:",
        `RoleType: ClientAdmin
CompanyCode: ${this.viewModel.AddCompanyDetail.companyCode}
Username: ${this.viewModel.addAdmin.loginId}
Password: ${this.viewModel.addAdmin.passwordHash}`
      );

      this.viewModel.addAdmin = resp.successData;
      this.router.navigate(['/login']);
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
}
