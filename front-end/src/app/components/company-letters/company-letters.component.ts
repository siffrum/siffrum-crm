import { Component, OnInit } from "@angular/core";
import { AppConstants } from "src/app-constants";
import { DocumentsSM } from "src/app/service-models/app/v1/client/documents-s-m";
import { CommonService } from "src/app/services/common.service";
import { CompanyLetterService } from "src/app/services/company-letter.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { CompanyLetterViewModel } from "src/app/view-models/company-letter.viewmodel";
import { BaseComponent } from "../base.component";
import { ModuleNameSM } from "src/app/service-models/app/enums/module-name-s-m.enum";
import { PermissionSM } from "src/app/service-models/app/v1/client/permission-s-m";
import { AccountService } from "src/app/services/account.service";

@Component({
  selector: "app-company-letters",
  templateUrl: "./company-letters.component.html",
  styleUrls: ["./company-letters.component.scss"],
  standalone: false,
})
export class CompanyLettersComponent
  extends BaseComponent<CompanyLetterViewModel>
  implements OnInit
{
  // ✅ UI helpers (safe)
  searchText: string = "";

  // ✅ IMPORTANT: initialize to [] to avoid template warnings
  filteredCompanyLetterList: any[] = [];

  selectedFileName: string = "";
  uploadError: string = "";
  maxFileSizeMB: number = 10;

  isBusyId: number | null = null;

  // ✅ Right-side placeholders (UI list)
  placeholderList = [
    { label: "Company Name", value: "companyname" },
    { label: "Company Address 1", value: "companyaddress1" },
    { label: "Company Address 2", value: "companyaddress2" },
    { label: "Company Country", value: "companycountry" },
    { label: "Company Pincode", value: "companypincode" },
    { label: "Company Code", value: "companycode" },
    { label: "Company Email", value: "companyemail" },
    { label: "Company Website", value: "companywebsite" },
    { label: "Company Phone", value: "companyphone" },
    { label: "Employee Name", value: "employeename" },
    { label: "Employee Email", value: "employeeemail" },
    { label: "Employee Designation", value: "employeedesignation" },
    { label: "Date of Joining", value: "employeedateofjoining" },
  ];

  constructor(
    _commonService: CommonService,
    logService: LogHandlerService,
    private companyLetterService: CompanyLetterService,
    private accountService: AccountService
  ) {
    super(_commonService, logService);
    this.viewModel = new CompanyLetterViewModel();

    // ✅ ensure list exists for UI even before API loads
    this.viewModel.companyLetterList = [];
    this.filteredCompanyLetterList = [];
  }

  async ngOnInit() {
    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;
    await this.loadPageData();
  }

  // ✅ Bind list to UI filtered list
  private bindListForUI() {
    const base = this.viewModel.companyLetterList || [];
    this.filteredCompanyLetterList = [...base];
    this.applySearch();
  }

  // ✅ Search helpers
  applySearch() {
    const q = (this.searchText || "").trim().toLowerCase();
    const list = this.viewModel.companyLetterList || [];

    if (!q) {
      this.filteredCompanyLetterList = [...list];
      return;
    }

    this.filteredCompanyLetterList = list.filter((x: any) => {
      const name = (x?.name || "").toLowerCase();
      const desc = (x?.description || "").toLowerCase();
      const ext = (x?.extension || "").toLowerCase();
      return name.includes(q) || desc.includes(q) || ext.includes(q);
    });
  }

  clearSearch() {
    this.searchText = "";
    this.applySearch();
  }

  // ✅ Form validation for modal Save button (UI)
  canSave(): boolean {
    const nameOk = (this.viewModel.companyLetter?.name || "").trim().length > 0;
    const fileOk = !!this.viewModel.companyLetter?.letterData;
    return nameOk && fileOk && !this.uploadError;
  }

  //Check For Action Permissions
  async getPermissions() {
    const ModuleName = ModuleNameSM.CompanyLetters;
    const resp: PermissionSM | any =
      await this.accountService.getMyModulePermissions(ModuleName);
    this.viewModel.Permission = resp;
  }

  /**
   * Get All Company Letters
   */
  override async loadPageData() {
    try {
      await this._commonService.presentLoading();

      const resp = await this.companyLetterService.getAllCompanyLetters();
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        await this._commonService.ShowToastAtTopEnd(
          resp.errorData.displayMessage,
          "error"
        );
      } else {
        this.viewModel.companyLetterList = resp.successData || [];
        await this.getPermissions();
        this.bindListForUI();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  //Check if All Company letters form fields filled or Not
  isFormValid(): boolean {
    const lettername = !!(this.viewModel.companyLetter?.name || "").trim();
    const uploadDocument = !!this.viewModel.companyLetter?.letterData;
    return lettername && uploadDocument;
  }

  /**
   * Add New Letter
   */
  async addCompanyLetter() {
    try {
      await this._commonService.presentLoading();

      if (!this.isFormValid()) {
        await this._commonService.showSweetAlertToast({
          title: "Please fill All The Required Fields",
          icon: "error",
        });
        return;
      }

      const resp = await this.companyLetterService.AddCompanyLetter(
        this.viewModel.companyLetter
      );

      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        await this._commonService.ShowToastAtTopEnd(
          resp.errorData.displayMessage,
          "error"
        );
      } else {
        this._commonService.ShowToastAtTopEnd(
          "Company Letter Added Successfully",
          "success"
        );
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
   */
  async getCompanyLetterByLetterId(letterId: number) {
    try {
      this.isBusyId = letterId;
      await this._commonService.presentLoading();

      const resp = await this.companyLetterService.getCompanyLetterById(letterId);
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        await this._commonService.ShowToastAtTopEnd(
          resp.errorData.displayMessage,
          "error"
        );
      } else {
        this._commonService.downloadDocument(
          resp.successData.letterData,
          resp.successData.name,
          resp.successData.extension
        );
      }
    } catch (error) {
      // keep silent like original
    } finally {
      this.isBusyId = null;
      await this._commonService.dismissLoader();
    }
  }

  /**
   * Delete Company Letter By Id
   */
  async deleteCompanyLetterByLetterId(letterId: number) {
    const confirm = await this._commonService.showConfirmationAlert(
      AppConstants.DefaultMessages.LetterDeleteAlertMessage,
      " ",
      true,
      "warning"
    );

    if (!confirm) return;

    try {
      this.isBusyId = letterId;
      await this._commonService.presentLoading();

      const resp = await this.companyLetterService.deleteCompanyLetterById(letterId);
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        await this._commonService.ShowToastAtTopEnd(
          resp.errorData.displayMessage,
          "error"
        );
      } else {
        this._commonService.ShowToastAtTopEnd(resp.successData.deleteMessage, "success");
        await this.loadPageData();
      }
    } catch (error) {
      // keep silent like original
    } finally {
      this.isBusyId = null;
      await this._commonService.dismissLoader();
    }
  }

  /**
   * Upload Document
   */
  uploadDocument(event: any) {
    const fileInput = event.target as HTMLInputElement;
    const file = fileInput?.files?.[0];

    // reset UI helpers
    this.uploadError = "";
    this.selectedFileName = "";

    if (!file) {
      this.viewModel.companyLetter.letterData = "";
      this.viewModel.companyLetter.extension = "";
      return;
    }

    const allowedExtensions = ["doc", "pdf"];
    const fileExtension = file.name.split(".").pop()?.toLowerCase() || "";

    if (!allowedExtensions.includes(fileExtension)) {
      this.uploadError = "Please select a .doc or .pdf file.";
      this._commonService.showSweetAlertToast({
        title: this.uploadError,
        icon: "error",
      });

      this.viewModel.companyLetter.letterData = "";
      this.viewModel.companyLetter.extension = "";
      fileInput.value = "";
      return;
    }

    const maxBytes = this.maxFileSizeMB * 1024 * 1024;
    if (file.size > maxBytes) {
      this.uploadError = `File too large. Max ${this.maxFileSizeMB}MB allowed.`;
      this._commonService.showSweetAlertToast({
        title: this.uploadError,
        icon: "error",
      });

      this.viewModel.companyLetter.letterData = "";
      this.viewModel.companyLetter.extension = "";
      fileInput.value = "";
      return;
    }

    this.selectedFileName = file.name;

    this._commonService.convertFileToBase64(file).subscribe({
      next: (base64) => {
        this.viewModel.companyLetter.extension = fileExtension;
        this.viewModel.companyLetter.letterData = base64;
      },
      error: async () => {
        this.uploadError = "Failed to read file. Please try again.";
        await this._commonService.showSweetAlertToast({
          title: this.uploadError,
          icon: "error",
        });
        this.viewModel.companyLetter.letterData = "";
        this.viewModel.companyLetter.extension = "";
        fileInput.value = "";
      },
    });
  }

  openLetterModalPopUp() {
    this.viewModel.companyLetter = new DocumentsSM();
    this.viewModel.displayStyle = "block";

    this.uploadError = "";
    this.selectedFileName = "";
  }

  closePopup() {
    this.viewModel.displayStyle = "none";
  }
}