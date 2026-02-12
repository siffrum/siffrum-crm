import { Component, OnInit } from "@angular/core";
import { CommonService } from "src/app/services/common.service";
import { CompanyOverviewService } from "src/app/services/company-overview.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import {
  CompanyOverviewViewModel,
  CompanyProfileTabs,
} from "src/app/view-models/company-overview.viewmodel";
import { BaseComponent } from "../base.component";
import { AppConstants } from "src/app-constants";
import { ModuleNameSM } from "src/app/service-models/app/enums/module-name-s-m.enum";
import { PermissionSM } from "src/app/service-models/app/v1/client/permission-s-m";
import { AccountService } from "src/app/services/account.service";
import { NgForm } from "@angular/forms";

@Component({
    selector: "app-company-overview",
    templateUrl: "./company-overview.component.html",
    styleUrls: ["./company-overview.component.scss"],
    standalone: false
})
export class CompanyOverviewComponent
  extends BaseComponent<CompanyOverviewViewModel>
  implements OnInit
{
  constructor(
    _commonService: CommonService,
    logService: LogHandlerService,
    private companyService: CompanyOverviewService,
    private accountService:AccountService
  ) {
    super(_commonService, logService);
    this.viewModel = new CompanyOverviewViewModel();
  }
// * @DEV : Musaib
 async ngOnInit() {
    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;
    let hasOverviewPermission = await this.checkViewPermission(CompanyProfileTabs.Overview);
    let hasAddressPermission = await this.checkViewPermission(CompanyProfileTabs.Address);
    // Set whether each tab should be shown based on permissions
    this.viewModel.showOverviewTab = hasOverviewPermission;
    this.viewModel.showAddressTab = hasAddressPermission;
   await  this.loadPageData();
  }
  // Function to check the 'view' permission for a given tab location
async checkViewPermission(tabLocation: CompanyProfileTabs): Promise<boolean> {
  try {
    let moduleName = tabLocation === CompanyProfileTabs.Overview
      ? ModuleNameSM.CompanyDetail
      : ModuleNameSM.CompanyAddress;

    let resp: PermissionSM | any = await this.accountService.getMyModulePermissions(moduleName);
    return resp.view; // Return true or false based on the 'view' permission
  } catch (error) {
    // Handle error if needed
    return false; // Return false in case of an error
  }
}
/**
 * Get Basic Details Of Company
 */
  override async loadPageData() {
    try {
      await this._commonService.presentLoading();
      let resp = await this.companyService.getCompanyDetails();
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
        this.getCompanyLogo();
        await this.getPermissions();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
    //get permissions
    async getPermissions(){
      let tabLocation =this.viewModel.tabLocation
      if(tabLocation==CompanyProfileTabs.Overview){
        let ModuleName=ModuleNameSM.CompanyDetail
        let resp:PermissionSM | any =await this.accountService.getMyModulePermissions(ModuleName)
        this.viewModel.Permission=resp
      }
      else{
        let ModuleName=ModuleNameSM.CompanyAddress
        let resp:PermissionSM | any =await this.accountService.getMyModulePermissions(ModuleName)
        this.viewModel.Permission=resp
      }
      }
      /**
 * UPDATE Tab Location(Overview/Address)
 * @param tabLocation
 */
  updateTabLocation(tabLocation: CompanyProfileTabs) {
    switch (tabLocation) {
      case CompanyProfileTabs.Address:
        this.getCompanyAddress();
        break;
        case CompanyProfileTabs.Overview:
          this.loadPageData();
          break;
      case CompanyProfileTabs.Overview:
      default:
        break;
    }
    this.viewModel.tabLocation = tabLocation;
  }
  /**Get Company Logo */
  async getCompanyLogo() {
  try {
    let resp = await this.companyService.GetCompanyLogo();
    if (resp.isError) {
      await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
    } else {
      this.viewModel.companyLogo = resp.successData;
    }
  } catch (error) {
    throw error;
  }
  finally {
    await this._commonService.dismissLoader();
  }
}
/**
 * Upload Profile Picture
 * @param event
 */
async uploadCompanyLogo(event: any) {
  try {
    await this._commonService.presentLoading();
    this._commonService.convertFileToBase64(event.target.files[0]).subscribe(async (base64) => {
      let fileName = event.target.files[0].name;
      fileName.split("?")[0].split(".").pop();
      this.viewModel.companyLogo = base64;
      if (this.viewModel.companyLogo !== "" || this.viewModel.companyLogo !== undefined) {
        let resp = await this.companyService.AddCompanyLogo(this.viewModel.companyLogo);
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
            "Company Logo Updated Successfully",
            "success"
            );
            await this.getCompanyLogo(); // fetch the updated picture from the server
          return;
        }
      }
    });
  } catch (error) {
    throw error;
  } finally {
    await this._commonService.dismissLoader();
  }
}

/**
   * Delete Company Logo
   * @DEV : Musaib
   * @returns Success Message
*/
async deleteCompanyLogo() {
  let deleteConfirmation = await this._commonService.showConfirmationAlert(
    AppConstants.DefaultMessages.DeleteCompanyLogo,
    " ",
    true,
    "warning"
  );
  if (deleteConfirmation) {
    try {
      let resp = await this.companyService.DeleteCompanyLogo();
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
          "Deleted logo  Successfully",
          "success"
          );
          this.getCompanyLogo()
        return;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
}
/**
 * Get Company Address Details 
 */
  async getCompanyAddress() {
    try {
      await this._commonService.presentLoading();
      let resp = await this.companyService.getCompanyAddress(this.viewModel.company.id);
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
        await this.getPermissions()
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  /**
   * UPDATE Basic Company Details
   * @returns success
   */
  async updateCompanyDetails(companyOverviewForm:NgForm) {
    this.viewModel.formSubmitted=true;
    try {
      if (companyOverviewForm.invalid){
        await this._commonService.showSweetAlertToast({ title:'Form Fields Are Not valid !', icon: 'error' });
        return; // Stop execution here if form is not valid
      }
      let resp = await this.companyService.updateCompanyDetails(
        this.viewModel.company
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
          "Company Overview Updated Successfully",
          "success"
        );
        this.viewModel.isCompanyDetailsDisabled = true;
        this.viewModel.showCompanyDetailsUpdateButton =false;
        this.loadPageData();
        return;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * UPDATE Company Address Details
   * @returns 
   */
  async updateCompanyAddress(companyAddressForm:NgForm) {
    this.viewModel.formSubmitted=true;
    try {
      if (companyAddressForm.invalid){
        await this._commonService.showSweetAlertToast({ title:'Form Fields Are Not valid !', icon: 'error' });
        return; // Stop execution here if form is not valid
      }
      this.viewModel.companyAddress.clientCompanyDetailId=this.viewModel.company.id;
      this.viewModel.companyAddress.id
      let resp = await this.companyService.updateCompanyAddress(
        this.viewModel.companyAddress
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
          "Updated Successfully",
          "success"
        );
        this.viewModel.isCompanyAddressDisabled = true;
        this.viewModel.showCompanyAddressUpdateButton =false;
        this.loadPageData();
        return;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  editCompanyDetails() {
    this.viewModel.isCompanyDetailsDisabled = !this.viewModel.isCompanyDetailsDisabled;
    this.viewModel.showCompanyDetailsUpdateButton =!this.viewModel.showCompanyDetailsUpdateButton;
  }
  editCompanyAddress() {
    this.viewModel.isCompanyAddressDisabled = !this.viewModel.isCompanyAddressDisabled;
    this.viewModel.showCompanyAddressUpdateButton =!this.viewModel.showCompanyAddressUpdateButton;
  }

}
