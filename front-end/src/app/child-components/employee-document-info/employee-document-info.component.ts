import { Component, Input, OnInit } from "@angular/core";
import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { AppConstants } from "src/app-constants";
import { BaseComponent } from "src/app/components/base.component";
import { EmployeeDocumentTypeSM } from "src/app/service-models/app/enums/employee-document-type-s-m.enum";
import { ModuleNameSM } from "src/app/service-models/app/enums/module-name-s-m.enum";
import { ClientUserSM } from "src/app/service-models/app/v1/app-users/client-user-s-m";
import { ClientEmployeeDocumentSM } from "src/app/service-models/app/v1/client/client-employee-document-s-m";
import { PermissionSM } from "src/app/service-models/app/v1/client/permission-s-m";
import { AccountService } from "src/app/services/account.service";
import { CommonService } from "src/app/services/common.service";
import { DocumentService } from "src/app/services/document.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { EmployeeDocumentInfoViewModel } from "src/app/view-models/employee-document-info.viewmodel";

@Component({
    selector: "app-employee-document-info",
    templateUrl: "./employee-document-info.component.html",
    styleUrls: ["./employee-document-info.component.scss"],
    standalone: false
})
export class EmployeeDocumentInfoComponent
  extends BaseComponent<EmployeeDocumentInfoViewModel>
  implements OnInit
{
  @Input() employee: ClientUserSM = new ClientUserSM();
  @Input() isReadonly!: boolean;
  @Input() isaddmode!: boolean;
  @Input() isEditMode!: boolean;

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private documentService: DocumentService,
    private router: Router,
    private accountService: AccountService
  ) {
    super(commonService, logService);
    this.viewModel = new EmployeeDocumentInfoViewModel();
  }

  async ngOnInit() {
    this.viewModel.documentType = this._commonService.EnumToStringArray(
      EmployeeDocumentTypeSM
    );
    if (!this.isaddmode) {
      await this.getEmployeeDocumentCount(this.employee.id);
      await this.loadPageData();
    }
    await this.getPermissions();
  }

  //Check  For Action  Permissions
  async getPermissions() {
    let ModuleName = ModuleNameSM.EmployeeDocument;
    let resp: PermissionSM | any =
      await this.accountService.getMyModulePermissions(ModuleName);
    this.viewModel.Permission = resp;
  }
  /** Get document details of Employee */
  override async loadPageData() {
    try {
      await this._commonService.presentLoading();
      await this._commonService.getCompanyIdFromStorage();
      // Access the company ID from the common service
      const companyId = this._commonService.layoutVM.company.id;
      await this.getEmployeeDocumentCount(this.employee.id);
      let resp =
        await this.documentService.getEmployeeDocumentsByCompanyIdAndEmployeeIdWithOData(
          this.viewModel,
          companyId,
          this.employee.id
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
        this.viewModel.employeeDocumentsList = resp.successData;
        await this.getEmployeeDocumentCount(this.employee.id);
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  /**
   * Get Employee Documents By Id
   * @param documentId
   */
  async getEmployeeDocumentByDocumentId(documentId: number) {
    try {
      await this._commonService.presentLoading();
      let resp = await this.documentService.getEmployeeDocumentsByDocumentId(
        documentId
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
        this.viewModel.employeeDocument = resp.successData;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Add Employee Documents Info
   */
  async addEmployeeDocumentInfo(documentInfoForm: NgForm) {
    this.viewModel.formSubmitted = true;
    try {
      if (documentInfoForm.invalid) {
        await this._commonService.showSweetAlertToast({
          title: "Form Fields Are Not valid !",
          icon: "error",
        });
        return;
      }
      await this._commonService.presentLoading();
      this.viewModel.employeeDocument.clientUserId = this.employee.id;
      let resp = await this.documentService.addEmployeeDocument(
        this.viewModel.employeeDocument
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
        this._commonService.ShowToastAtTopEnd(
          "Employee Document Added Successfully",
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
   * Update employee documents
   */
  async updateEmployeeDocumentInfo(documentInfoForm: NgForm) {
    this.viewModel.formSubmitted = true;
    try {
      if (documentInfoForm.invalid) {
        await this._commonService.showSweetAlertToast({
          title: "Form Fields Are Not valid !",
          icon: "error",
        });
        return;
      }
      await this._commonService.presentLoading();
      let resp = await this.documentService.updateEmployeeDocumentByDocumentId(
        this.viewModel.employeeDocument
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
        this._commonService.ShowToastAtTopEnd(
          "Employee Document Details Updated Successfully",
          "success"
        );
        await this.loadPageData();
        this.viewModel.displayStyle = "none";
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  /**
   * Delete document
   * @param documentId
   */

  async deleteEmployeeDocumentByDocumentId(documentId: number) {
    let deleteEmployeeDocumentConfirmation =
      await this._commonService.showConfirmationAlert(
        AppConstants.DefaultMessages.EmployeeDocumentDeleteAlert,
        " ",
        true,
        "warning"
      );
    if (deleteEmployeeDocumentConfirmation) {
      try {
        await this._commonService.presentLoading();
        let resp =
          await this.documentService.deleteEmployeeDocumentByDocumentId(
            documentId
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
          this._commonService.ShowToastAtTopEnd(
            resp.successData.deleteMessage,
            "success"
          );
          await this.loadPageData();
        }
      } catch (error) {
      } finally {
        await this._commonService.dismissLoader();
      }
    }
  }
  async downloadEmployeeDocument(
    documentId: number,
    documentData: string,
    documentName: string,
    employeeName: string,
    fileExtension: string
  ) {
    try {
      await this._commonService.presentLoading();
      let resp =
        await this.documentService.doenloadEmployeeDocumentsByDocumentId(
          documentId
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
        this.viewModel.employeeDocument = resp.successData;
        this._commonService.downloadDocument(
          (documentData = this.viewModel.employeeDocument.employeeDocumentPath),
          documentName,
          fileExtension,
          employeeName
        );
        this._commonService.ShowToastAtTopEnd("Downloading....", "success");
        this.loadPageData();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  /** open modal to Add or Update Employee Documents */
  openEmployeeDocumentInfoModal(id: number) {
    this.viewModel.employeeDocument = new ClientEmployeeDocumentSM();
    if (id > 0) {
      this.viewModel.editMode = true;
      this.getEmployeeDocumentByDocumentId(id);
    } else {
      this.viewModel.editMode = false;
    }
    // this.modalService.open(content, { size: "lg", windowClass: "modal-lg" });
    this.viewModel.displayStyle = "block";
  }
  closePopup(documentInfoForm: NgForm) {
    this.viewModel.displayStyle = "none";
    this.viewModel.formSubmitted = false; // Clear the formSubmitted flag
    if (documentInfoForm) {
      documentInfoForm.reset(); // Reset the form
    }
  }
  // Upload Employee Document
  uploadDocument(event: any) {
    const fileInput = event.target as HTMLInputElement;
    const file = fileInput?.files?.[0];
  
    if (!file) {
      // No file selected
      return;
    }
  
    // Check if the file size is more than 20 MB (20 * 1024 * 1024 bytes)
    const maxSize = 20 * 1024 * 1024;
    if (file.size > maxSize) {
      // Show an error message or perform any desired action for exceeding the size limit
      this._commonService.showSweetAlertToast({
        title: "File size should not exceed 20 MB.",
        icon: "error",
      });
  
      // Clear the input to allow the user to select another file
      this.viewModel.employeeDocument.employeeDocumentPath = "";
      return;
    }
  
    const allowedExtensions = ["doc", "pdf"];
    const fileExtension = file.name.split(".").pop()?.toLowerCase() || "";
    if (!allowedExtensions.includes(fileExtension)) {
      // Unsupported file type, show custom error message or perform any desired action
      this._commonService.showSweetAlertToast({
        title: "Please select a .doc or .pdf file.",
        icon: "error",
      });
  
      // Clear the input to allow the user to select another file
      this.viewModel.employeeDocument.employeeDocumentPath = "";
      return;
    }
  
    this._commonService.convertFileToBase64(file).subscribe((base64) => {
      this.viewModel.employeeDocument.extension = fileExtension;
      this.viewModel.employeeDocument.employeeDocumentPath = base64;
    });
  }
  
  async skipEmployeeFurther() {
    this.router.navigate(["/employees-list"]);
  }
  /**
   * Get Employee Document Details Count
   * @param id
   */
  async getEmployeeDocumentCount(empId: number) {
    try {
      await this._commonService.presentLoading();
      let resp = await this.documentService.getEmployeeDocumentCount(empId);
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.pagination.totalCount = resp.successData.intResponse;
      }
    } catch (error) {
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**this function is used to create an event for pagination */
  async loadPagedataWithPagination(pageNumber: number) {
    if (pageNumber && pageNumber > 0) {
      // this.viewModel.PageNo = pageNumber;
      this.viewModel.pagination.PageNo = pageNumber;
      await this.loadPageData();
    }
  }
}
