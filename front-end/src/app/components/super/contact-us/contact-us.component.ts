import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../base.component';
import { ContactUsViewModel } from 'src/app/view-models/contact-us.viewmodel';
import { CommonService } from 'src/app/services/common.service';
import { LogHandlerService } from 'src/app/services/log-handler.service';
import { ContactUsService } from 'src/app/services/contact-us.service';
import { NgForm } from '@angular/forms';
import { AppConstants } from 'src/app-constants';
import { ContactUsSM } from 'src/app/service-models/app/v1/general/contact-us-s-m';

@Component({
    selector: 'app-contact-us',
    templateUrl: './contact-us.component.html',
    styleUrls: ['./contact-us.component.scss'],
    standalone: false
})
export class ContactUsComponent extends BaseComponent<ContactUsViewModel> implements OnInit  {
 constructor( commonService:CommonService,logService:LogHandlerService, private contactUsService:ContactUsService){
  super(commonService,logService)
  this.viewModel = new ContactUsViewModel();

 }
 async ngOnInit() {

  await this.loadPageData();
}

/**
 * Get All Contact us Details
 */
override async loadPageData() {
  try {
    await this._commonService.presentLoading();
    let resp = await this.contactUsService.getAllcontactUsData();
    if (resp.isError) {
      await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
    } else {
      this.viewModel.contactUsDataList = resp.successData;
    }
  } catch (error) {
    throw error;
  } finally {
    await this._commonService.dismissLoader();
  }
}
async openContactUsModal(id: number) {
  this.viewModel.contactUsObj = new ContactUsSM();
  if (id > 0) {
    this.viewModel.editMode = true;
    this.getCompanycontactUsDetailsById(id);
  } else {
    this.viewModel.editMode = false;
  }
  this.viewModel.displayStyle = "block";
}
closePopup(contactUsForm: NgForm) {
  this.viewModel.displayStyle = "none";
  this.viewModel.formSubmitted = false; // Clear the formSubmitted flag
  if (contactUsForm) {
    contactUsForm.reset(); // Reset the form
  }
}
/**
 * Get Contact Us Details By Id
 * @param id
 */
async getCompanycontactUsDetailsById(id: number) {
  try {
    await this._commonService.presentLoading();
    let resp = await this.contactUsService.getcontactUsById(
      id
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
      this.viewModel.contactUsObj = resp.successData;
    }
  } catch (error) {
    throw error;
  } finally {
    await this._commonService.dismissLoader();
  }
}

/**
 * Delete Contact Us Details
 * @param id
 * @returns 
 */
async deleteContactUsDetails(id: number) {
  let deleteConfirmation = await this._commonService.showConfirmationAlert(
    AppConstants.DefaultMessages.DeleteContactUsDetails,
    " ",
    true,
    "warning"
  );
  if (deleteConfirmation) {
    try {
      let resp = await this.contactUsService.DeletecontactUs(
        id
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
        // this.modalService.dismissAll();
        await this._commonService.ShowToastAtTopEnd(
          "Deleted Contact Details  Successfully",
          "success"
        );
        this.loadPageData();
        return;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
}

}
