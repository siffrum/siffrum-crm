import { Component, OnInit } from '@angular/core';
import { AppConstants } from 'src/app-constants';
import { DocumentsSM } from 'src/app/service-models/app/v1/client/documents-s-m';
import { CommonService } from 'src/app/services/common.service';
import { CompanyLetterService } from 'src/app/services/company-letter.service';
import { LogHandlerService } from 'src/app/services/log-handler.service';
import { CompanyLetterViewModel } from 'src/app/view-models/company-letter.viewmodel';
import { BaseComponent } from '../base.component';
import { ModuleNameSM } from 'src/app/service-models/app/enums/module-name-s-m.enum';
import { PermissionSM } from 'src/app/service-models/app/v1/client/permission-s-m';
import { AccountService } from 'src/app/services/account.service';

@Component({
    selector: 'app-company-letters',
    templateUrl: './company-letters.component.html',
    styleUrls: ['./company-letters.component.scss'],
    standalone: false
})
export class CompanyLettersComponent extends BaseComponent<CompanyLetterViewModel> implements OnInit {
  constructor(
    _commonService: CommonService,
    logService: LogHandlerService,
    private companyLetterService: CompanyLetterService,
    private accountService:AccountService
  ) {
    super(_commonService, logService);
    this.viewModel = new CompanyLetterViewModel();
  }
// * @DEV : Musaib
  async ngOnInit() {
    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;
    await this.loadPageData();
  }
      //Check For Action  Permissions
      async getPermissions(){
        let ModuleName=ModuleNameSM.CompanyLetters
         let resp:PermissionSM | any =await this.accountService.getMyModulePermissions(ModuleName)
         this.viewModel.Permission=resp
        }

/**
 * Get All Company Letters
 *
 */
  override async loadPageData() {
    try {
      await this._commonService.presentLoading();
      let resp = await this.companyLetterService.getAllCompanyLetters();
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        await this._commonService.ShowToastAtTopEnd(resp.errorData.displayMessage, 'error');
      } else {
        this.viewModel.companyLetterList = resp.successData;
        await this.getPermissions();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  //Check if All Company letters form fields filled or Not
  isFormValid():boolean{
    let lettername=!!this.viewModel.companyLetter.name;
    let uploadDocument=!!this.viewModel.companyLetter.letterData;
    return(lettername&&uploadDocument)
  }
/**
 * Add New Letter
 */

  async addCompanyLetter() {
    try {
      await this._commonService.presentLoading();
      if (!this.isFormValid()) {
        // Check if any required field is empty
        await this._commonService.showSweetAlertToast({ title:'Please fill All The Required Fields', icon: 'error' });
       return; // Stop execution here if form is not valid
     }
      let resp = await this.companyLetterService.AddCompanyLetter(this.viewModel.companyLetter);
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        await this._commonService.ShowToastAtTopEnd(resp.errorData.displayMessage, 'error');
      } else {
        this._commonService.ShowToastAtTopEnd("Company Letter Added Successfully", "success");
        this.viewModel.displayStyle = "none";
        await this.loadPageData();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Download Company Letter by Letter id
   * @param letterId 
   */
  async getCompanyLetterByLetterId(letterId: number) {
    try {
      await this._commonService.presentLoading();
      let resp = await this.companyLetterService.getCompanyLetterById(letterId);
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        await this._commonService.ShowToastAtTopEnd(resp.errorData.displayMessage, 'error');
      } else {
        this._commonService.downloadDocument(resp.successData.letterData, resp.successData.name, resp.successData.extension);
      }
    } catch (error) {

    } finally {
      await this._commonService.dismissLoader();
    }

  }
  /**
   * Delete Company Letter By Id
   * @param letterId 
   */
  async deleteCompanyLetterByLetterId(letterId: number) {
    let deleteEmployeeDocumentConfirmation = await this._commonService.showConfirmationAlert(AppConstants.DefaultMessages.LetterDeleteAlertMessage, " ", true, "warning");
    if (deleteEmployeeDocumentConfirmation) {
      try {
        await this._commonService.presentLoading();
        let resp = await this.companyLetterService.deleteCompanyLetterById(letterId);
        if (resp.isError) {
          await this._exceptionHandler.logObject(resp.errorData);
          await this._commonService.ShowToastAtTopEnd(resp.errorData.displayMessage, 'error');
        }
        else {
          this._commonService.ShowToastAtTopEnd(resp.successData.deleteMessage, "success");
          this.loadPageData();
        }
      } catch (error) {

      } finally {
        await this._commonService.dismissLoader();
      }
    }
  }


  // Upload Employee Document
  uploadDocument(event: any) {
    const fileInput = event.target as HTMLInputElement;
    const file = fileInput?.files?.[0];
  
    if (!file) {
      // No file selected, handle this case
      this.viewModel.companyLetter.letterData = '';
      return;
    }
  
    const allowedExtensions = ["doc", "pdf"];
    const fileExtension = file.name.split('.').pop()?.toLowerCase() || '';
    if (!allowedExtensions.includes(fileExtension)) {
      // Unsupported file type, show custom error message or perform any desired action
       this._commonService.showSweetAlertToast({ title:'Please select a .doc or .pdf file.', icon: 'error' });

      // Clear the input to allow the user to select another file
      this.viewModel.companyLetter.letterData = '';
      return;
    }
  
    this._commonService.convertFileToBase64(file).subscribe((base64) => {
      this.viewModel.companyLetter.extension = fileExtension;
      this.viewModel.companyLetter.letterData = base64;
    });
  }
  openLetterModalPopUp() {
    this.viewModel.companyLetter = new DocumentsSM();
    this.viewModel.displayStyle = "block";
  }
  closePopup() {
    this.viewModel.displayStyle = "none";
  }
}
