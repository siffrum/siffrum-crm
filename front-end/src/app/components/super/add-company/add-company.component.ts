import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { ClientCompanyDetailSM } from "src/app/service-models/app/v1/client/client-company-detail-s-m";
import { CommonService } from "src/app/services/common.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { SuperCompanyService } from "src/app/services/super-company.service";
import { AddCompanyViewModel } from "src/app/view-models/add-company.viewmodel";
import { BaseComponent } from "../../base.component";
import axios from 'axios';
import { AddCompanyWizards } from "src/app/internal-models/common-models";
import { NgForm } from "@angular/forms";
interface Country {
  name: string;
}
@Component({
    selector: "app-add-company",
    templateUrl: "./add-company.component.html",
    styleUrls: ["./add-company.component.scss"],
    standalone: false
})

export class AddCompanyComponent
  extends BaseComponent<AddCompanyViewModel>
  implements OnInit
{
  //*@Dev Musaib
  companyInfo: ClientCompanyDetailSM = new ClientCompanyDetailSM();
  @Input() isReadOnly!: boolean;
  @Output() newItemEvent = new EventEmitter<ClientCompanyDetailSM>();
  countries: Country[] = [];
  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private activatedRoute: ActivatedRoute,
    private superCompanyService: SuperCompanyService,
  ) {
    super(commonService, logService);
    this.viewModel = new AddCompanyViewModel();
  }
  company = 'company';
  async ngOnInit() {
    this.viewModel.PageTitle = this.viewModel.PageTitle;
    //Get id from Company-List Component
    const Id = Number(this.activatedRoute.snapshot.paramMap.get("Id"));
    this.viewModel.AddCompanyDetail.id = Id;
    if (Id == undefined) {
      await this._commonService.ShowToastAtTopEnd(
        "Something Went Wrong",
        "error"
      );
    }
    axios.get('https://restcountries.com/v3.1/all')
    .then(response => {
      this.countries = response.data;
    })
    .catch(error => {
      console.error('Error fetching countries:', error);
    });
    this.nextWizardLocation(AddCompanyWizards.addCompanyInfo);
  }
  /**Add Company with  Details for new company
   * @return Success
   * @developer Musaib
   */
  async addNewCompanyDetails(companyOverviewForm:NgForm) {
    this.viewModel.formSubmitted=true;
    try {
      if (companyOverviewForm.invalid){
        await this._commonService.showSweetAlertToast({ title:'Form Fields Are Not valid !', icon: 'error' });
        return; // Stop execution here if form is not valid
      }
     await this._commonService.presentLoading();
      this.viewModel.AddCompanyDetail.isEnabled = true;
      let resp = await this.superCompanyService.addNewCompanyDetails(
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
        await this._commonService.ShowToastAtTopEnd(
          "Added Company details Successfully",
          "success"
        );
        this.viewModel.AddCompanyDetail = resp.successData;
        this.nextWizardLocation(1);

      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }


  /**Add Company Address
   * @returns success
   * @developer Musaib
   */

  async addNewCompanyAddress(companyAddressForm:NgForm) {
    this.viewModel.formSubmitted=true;
    try {
      if (companyAddressForm.invalid){
        await this._commonService.showSweetAlertToast({ title:'Form Fields Are Not valid !', icon: 'error' });
        return; // Stop execution here if form is not valid
      }
      await this._commonService.presentLoading();
      this.viewModel.AddCompanyAddress.clientCompanyDetailId =
        this.viewModel.AddCompanyDetail.id;
      let resp = await this.superCompanyService.addCompanyAddress(
        this.viewModel.AddCompanyAddress
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
          "Added Company details Successfully",
          "success"
        );
        this.viewModel.AddCompanyAddress = resp.successData;
       this.nextWizardLocation(2);

        return;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**move to next tab location */
  nextWizardLocation(wizardLocation: AddCompanyWizards) {
    switch (wizardLocation) {
      case AddCompanyWizards.addCompanyAdminDetails:
        // await this.getCompanyAddressByCompanyId();
        break;
      case AddCompanyWizards.addCompanyAddress:
        // await this.getCompanyModulesAndFeaturesByCompanyId();
        break;
      case AddCompanyWizards.addModules:
        // await this.getAllCompanyAdminsByCompanyId();
        break;
      case AddCompanyWizards.addCompanyInfo:
      default:
        break;
    }
    this._commonService.layoutVM.wizardLocation = wizardLocation;
  }
  /**disable adding future dates */
  disableFutureDates() {
    this.viewModel.disabledDate = new Date().toISOString().split("T")[0];
  }
}
