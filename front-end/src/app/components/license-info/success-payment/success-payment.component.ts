import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../base.component';
import { LicenseViewModel, PaymentStatusInfo } from 'src/app/view-models/license-info.viewmodel';
import { CommonService } from 'src/app/services/common.service';
import { LogHandlerService } from 'src/app/services/log-handler.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AppConstants } from 'src/app-constants';
import { LicenseInfoService } from 'src/app/services/license-info.service';
import { differenceInMinutes } from 'date-fns';

@Component({
    selector: 'app-success-payment',
    templateUrl: './success-payment.component.html',
    styleUrls: ['./success-payment.component.scss'],
    standalone: false
})
export class SuccessPaymentComponent extends BaseComponent<LicenseViewModel> implements OnInit {

  constructor(commonService:CommonService,logService:LogHandlerService,
    private activatedRoute: ActivatedRoute,
    private router: Router,private licenseInfoService:LicenseInfoService){
super(commonService,logService)
this.viewModel = new LicenseViewModel()
  }

ngOnInit(): void {
  this._commonService.layoutVM.toogleWrapper = "loginWrapper";
  this._commonService.layoutVM.showLeftSideMenu=false;
  let info = this.activatedRoute.snapshot.paramMap.get("info");
  if (info == undefined || info == null) {
    this.router.navigate([AppConstants.WebRoutes.UNAUTHORIZED]);
  } else {
    try {
      let details: PaymentStatusInfo = JSON.parse(this._commonService.decrypt(atob(info)));
      if (details == null)
        this.router.navigate([AppConstants.WebRoutes.UNAUTHORIZED]);
      this.viewModel.paymentInfo = details;
      this.loadPageData();
    } catch (error) {
      this._exceptionHandler.logObject(error);
    }
  }
}
override async loadPageData(): Promise<void> {
  try {
    await this._commonService.presentLoading();
    const paymentDate = this.viewModel.paymentInfo.paymentDate;
    const currentTime = new Date();

    const timeDifference = differenceInMinutes(currentTime, paymentDate);

    if (timeDifference > 10) {
      // problem
    }
    let resp = await this.licenseInfoService.getLicenseByLicenseId(this.viewModel.paymentInfo.licenseId);
    if (resp.isError) {
      await this._exceptionHandler.logObject(resp.errorData);
      this._commonService.showSweetAlertToast({ title: resp.errorData.displayMessage, icon: 'error' });
      return;
    } else {
      this.viewModel.LicenseTypeObj = resp.successData;
    }
  }
  catch (error) {
    throw error;
  } finally {
    await this._commonService.dismissLoader();
  }
}
}
