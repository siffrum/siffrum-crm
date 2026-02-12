import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { EmployeeStatusSM } from "src/app/service-models/app/enums/employee-status-s-m.enum";
import { LoginStatusSM } from "src/app/service-models/app/enums/login-status-s-m.enum";
import { RoleTypeSM } from "src/app/service-models/app/enums/role-type-s-m.enum";
import { ClientUserSM } from "src/app/service-models/app/v1/app-users/client-user-s-m";
import { CommonService } from "src/app/services/common.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { SuperCompanyService } from "src/app/services/super-company.service";
import {
  SuperCompanyOverviewViewModel,
  SuperCompanyProfileTabs,
} from "src/app/view-models/super-company-overview.viewmodel";
import { BaseComponent } from "../../base.component";
import { DatePipe } from "@angular/common";
import { format } from "date-fns";
import { AddCompanyWizards } from "src/app/internal-models/common-models";

@Component({
    selector: "app-super-company-overview",
    templateUrl: "./super-company-overview.component.html",
    styleUrls: ["./super-company-overview.component.scss"],
    providers: [DatePipe],
    standalone: false
})
export class SuperCompanyOverviewComponent
  extends BaseComponent<SuperCompanyOverviewViewModel>
  implements OnInit
{
  constructor(
    _commonService: CommonService,
    logService: LogHandlerService,
    private superCompanyService: SuperCompanyService,
    private activatedRoute: ActivatedRoute,
    public datePipe: DatePipe
  ) {
    super(_commonService, logService);
    this.viewModel = new SuperCompanyOverviewViewModel();
  }
  async ngOnInit() {
    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;
    //Get id from Company-List Component
    const Id = Number(this.activatedRoute.snapshot.paramMap.get("Id"));
    this.viewModel.company.id = Id;
    if (Id == undefined) {
      await this._commonService.ShowToastAtTopEnd(
        "Something Went Wrong",
        "error"
      );
    } else {
      await this.loadPageData();
    }

    this.nextWizardLocation(0)
  }

  /**Load Company Basic Info/ Overview data
   * @param companyid
   * @developer Musaib
   */
  override async loadPageData() {
    try {
      await this._commonService.presentLoading();
      let resp = await this.superCompanyService.getCompanyDetailsByCompanyId(
        this.viewModel.company.id
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
        this.viewModel.company = resp.successData;
        this.viewModel.company.companyDateOfEstablishment=await this.getFormattedDate(this.viewModel.company.companyDateOfEstablishment,false)
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**Update tab Location Company Overview/Address/Module/UserInfo/AddNewUser */


  async nextWizardLocation(wizardLocation: AddCompanyWizards) {
    this._commonService.nextTablocations(wizardLocation);
    if(this._commonService.layoutVM.wizardLocation==1){
      this.getCompanyAddressByCompanyId();
     }
  }
  /**Get Company Address By id
   * @param companyId
   * @developer Musaib
   */
  async getCompanyAddressByCompanyId() {
    try {
      await this._commonService.presentLoading();
      let resp = await this.superCompanyService.getCompanyAddressByCompanyId(
        this.viewModel.company.id
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
        this.viewModel.companyAddress = resp.successData;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
}

