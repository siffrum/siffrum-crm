import { Component, OnInit } from "@angular/core";
import { BaseComponent } from "../base.component";
import { CompanyDepartmentViewModel } from "src/app/view-models/company-department.viewmodel";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { CommonService } from "src/app/services/common.service";
import { CompanyDepartmentService } from "src/app/services/company-departments.service";
import { AccountService } from "src/app/services/account.service";
import { PermissionSM } from "src/app/service-models/app/v1/client/permission-s-m";
import { ModuleNameSM } from "src/app/service-models/app/enums/module-name-s-m.enum";
import { ClientCompanyDepartmentSM } from "src/app/service-models/app/v1/client/client-company-department-s-m";
import { AppConstants } from "src/app-constants";
import { NgForm } from "@angular/forms";

@Component({
    selector: "app-company-departments",
    templateUrl: "./company-departments.component.html",
    styleUrls: ["./company-departments.component.scss"],
    standalone: false
})
export class CompanyDepartmentsComponent
  extends BaseComponent<CompanyDepartmentViewModel>
  implements OnInit
{
  constructor(
    _commonService: CommonService,
    logService: LogHandlerService,
    private accountService: AccountService,
    private companyDepartmentService: CompanyDepartmentService
  ) {
    super(_commonService, logService);
    this.viewModel = new CompanyDepartmentViewModel();
  }

  async ngOnInit() {
    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;
    await this.loadPageData();
  }
  //Check For Action permissions
  async getPermissions() {
    let ModuleName = ModuleNameSM.CompanyDepartment;
    let resp: PermissionSM | any =
      await this.accountService.getMyModulePermissions(ModuleName);
    this.viewModel.Permission = resp;
  }
  /**
   * Get All Company Departments
   */
  override async loadPageData() {
    try {
      await this._commonService.presentLoading();
      let resp = await this.companyDepartmentService.getAllCompanyDepartments();
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.departmentList = resp.successData;
        this.getPermissions();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Generate Unique Department Code
   */
  generateDepartmentCode(): void {
    // Generate a random 6-digit number
    const random6DigitNumber = Math.floor(100000 + Math.random() * 900000);
    this.viewModel.department.departmentCode = `${random6DigitNumber}`;
  }
  /**
   * And Validate if Code Exceeds more than or less than 6 digits
   * @param event
   */
  validateDepartmentCode(event: any): void {
    // Ensure the input value is exactly 6 digits
    const inputValue = event.target.value;
    if (inputValue.length !== 6 || isNaN(Number(inputValue))) {
      // If the input is not a 6-digit number, reset it to the generated code
      this.generateDepartmentCode();
    } else {
      // Set the validated input value
      this.viewModel.department.departmentCode = `${inputValue}`;
    }
  }

  /**
   * Get Company Department Details By Company Id
   */
  async getCompanyDepartmentDetailsById(id: number) {
    try {
      await this._commonService.presentLoading();
      let resp = await this.companyDepartmentService.getCompanyDepartmentById(
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
        this.viewModel.department = resp.successData;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  /**open Modal to Add/update Company Department
   * @DEV : Musaib
   */

  async openCompanyDepartmentModal(id: number) {
    this.viewModel.department = new ClientCompanyDepartmentSM();
    if (id > 0) {
      this.viewModel.editMode = true;
      this.getCompanyDepartmentDetailsById(id);
    } else {
      this.viewModel.editMode = false;
    }
    // this.modalService.open(content, { size: "lg", windowClass: "modal-lg" });
    this.viewModel.displayStyle = "block";
  }
  closePopup(departmentForm: NgForm) {
    this.viewModel.displayStyle = "none";
    this.viewModel.formSubmitted = false; // Clear the formSubmitted flag
    if (departmentForm) {
      departmentForm.reset(); // Reset the form
    }
  }

  /**ADD Attendance Department
   * @DEV : Musaib
   */
  async addCompanyDepartment(departmentForm: NgForm) {
    this.viewModel.formSubmitted = true;
    try {
      if (departmentForm.invalid) {
        await this._commonService.showSweetAlertToast({
          title: "Form Fields Are Not valid !",
          icon: "error",
        });
        return; // Stop execution here if form is not valid
      }
      await this._commonService.presentLoading();
      this.viewModel.department.clientCompanyDetailId = 1;
      // this.viewModel.company.id;
      let resp =
        await this.companyDepartmentService.addNewCompanyDepartmentDetails(
          this.viewModel.department
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
        this.viewModel.displayStyle = "none";
        this.loadPageData();
        await this._commonService.ShowToastAtTopEnd(
          "Added Company Department   Successfully",
          "success"
        );

        return;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Update Company Department
   * @param id
   * @returns success
   * @developer Musaib
   */

  async updateCompanyDepartmentDetails(departmentForm: NgForm) {
    this.viewModel.formSubmitted = true;
    try {
      if (departmentForm.invalid) {
        await this._commonService.showSweetAlertToast({
          title: "Form Fields Are Not valid !",
          icon: "error",
        });
        return; // Stop execution here if form is not valid
      }
      let resp =
        await this.companyDepartmentService.updateCompanyDepartmentDetails(
          this.viewModel.department
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
        this.viewModel.displayStyle = "none";
        this.loadPageData();
        await this._commonService.ShowToastAtTopEnd(
          "Updated   Successfully",
          "success"
        );
        return;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Delete Selected Department
   * @DEV : Musaib
   * @param id
   * @returns Success Message
   */
  async deleteCompanyDepartment(id: number) {
    let deleteConfirmation = await this._commonService.showConfirmationAlert(
      AppConstants.DefaultMessages.DeleteCompanyDepartment,
      " ",
      true,
      "warning"
    );
    if (deleteConfirmation) {
      try {
        let resp = await this.companyDepartmentService.DeleteCompanyDepartment(
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
            "Deleted Department Successfully",
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
