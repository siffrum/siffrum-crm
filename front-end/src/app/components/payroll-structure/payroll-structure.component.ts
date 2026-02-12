import { Component, OnInit } from "@angular/core";
import { AppConstants } from "src/app-constants";
import { ComponentPeriodTypeSM } from "src/app/service-models/app/enums/component-period-type-s-m.enum";
import { ClientGenericPayrollComponentSM } from "src/app/service-models/app/v1/client/client-generic-payroll-component-s-m";
import { CommonService } from "src/app/services/common.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { PayrollService } from "src/app/services/payroll.service";
import { GenericPayrollViewModel } from "src/app/view-models/generic-payroll";
import { BaseComponent } from "../base.component";
import { PermissionSM } from "src/app/service-models/app/v1/client/permission-s-m";
import { AccountService } from "src/app/services/account.service";
import { ModuleNameSM } from "src/app/service-models/app/enums/module-name-s-m.enum";
import { NgForm } from "@angular/forms";

@Component({
    selector: "app-payroll-structure",
    templateUrl: "./payroll-structure.component.html",
    styleUrls: ["./payroll-structure.component.scss"],
    standalone: false
})
export class PayrollStructureComponent
  extends BaseComponent<GenericPayrollViewModel>
  implements OnInit
{
  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private payrollService: PayrollService,
    private accountService: AccountService
  ) {
    super(commonService, logService);
    this.viewModel = new GenericPayrollViewModel();
  }
  ngOnInit() {
    this.loadPageData();
  }
  //Check For Actions permissions
  async getPermissions() {
    // this.viewModel.Permission= new PermissionSM()
    let ModuleName = ModuleNameSM.EmployeeGenericPayroll;
    let resp: PermissionSM | any =
      await this.accountService.getMyModulePermissions(ModuleName);
    this.viewModel.Permission = resp;
  }
  /**Get Generic payroll Structure
   * @DEV : Musaib
   */
  override async loadPageData() {
    try {
      await this._commonService.presentLoading();
      this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;
      let resp = await this.payrollService.GetAllGenericPayrollComponents();
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.componentPeriodTypeList =
          this._commonService.EnumToStringArray(ComponentPeriodTypeSM);
        this.viewModel.genericPayrolls = resp.successData;
        this.getPermissions();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**ADD Generic Payroll Component
   * @DEV : Musaib
   */
  async addGenericPayrollComponent(payrollForm: NgForm) {
    this.viewModel.formSubmitted = true;
    try {
      if (payrollForm.invalid) {
        await this._commonService.showSweetAlertToast({
          title: "Form Fields Are Not valid !",
          icon: "error",
        });
        return; // Stop execution here if form is not valid
      }
      // Step 1: Calculate the current sum of percentages
      const currentSum = this.viewModel.genericPayrolls.reduce(
        (sum, obj) => sum + obj.percentage,
        0
      );
      if (currentSum == 100) {
        await this._commonService.showSweetAlertToast({
          title: "Percenrage Cant be Greater than 100",
          icon: "error",
        });
        return;
      }
      await this._commonService.presentLoading();
      let resp = await this.payrollService.AddGenericPayrollComponents(
        this.viewModel.selectedGenericPayroll
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
        await this._commonService.ShowToastAtTopEnd(
          "Added Payroll   Successfully",
          "success"
        );
        this.loadPageData();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  /**Update Generic Payroll component
   * @DEV : Musaib
   * @params Id
   */
  async updateGenericPayrollComponent(payrollForm: NgForm) {
    this.viewModel.formSubmitted = true;
    try {
      if (payrollForm.invalid) {
        await this._commonService.showSweetAlertToast({
          title: "Form Fields Are Not valid !",
          icon: "error",
        });
        return; // Stop execution here if form is not valid
      }
      let resp = await this.payrollService.updateGenericComponent(
        this.viewModel.selectedGenericPayroll
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
        await this._commonService.ShowToastAtTopEnd(
          "Updated Payroll  Successfully",
          "success"
        );
        this.loadPageData();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Delete Selected Generic Payroll
   * @DEV : Musaib
   * @param id
   * @returns Success Message
   */
  async deleteGenericPayrollComponent(id: number) {
    let deleteConfirmation = await this._commonService.showConfirmationAlert(
      AppConstants.DefaultMessages.GenericPayrollComponentDeleteAlert,
      " ",
      true,
      "warning"
    );
    if (deleteConfirmation) {
      try {
        let resp = await this.payrollService.DeleteGenericPayrollComponents(id);
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
            "Deleted Payroll  Successfully",
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

  /**
   *Get GenericPayroll Component By Id
   * @param id
   */
  async getGenericPayrollComponentsById(id: number) {
    try {
      await this._commonService.presentLoading();
      let resp = await this.payrollService.GetGenericPayrollComponentsById(id);
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.selectedGenericPayroll = resp.successData;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**open Modal to Add/update Generic Payroll Component
   * @DEV : Musaib
   */
  async openModal(id: number) {
    this.viewModel.selectedGenericPayroll =
      new ClientGenericPayrollComponentSM();
    if (id > 0) {
      this.getGenericPayrollComponentsById(id);
      this.viewModel.readonlyText = "readonly";
      this.viewModel.showButton = true;
      this.viewModel.editMode=true;
      this.viewModel.addMode=false;
    } else {
      this.viewModel.selectedGenericPayroll =
        new ClientGenericPayrollComponentSM();
      this.viewModel.componentPeriodTypeList =
        this._commonService.EnumToStringArray(ComponentPeriodTypeSM);
      this.viewModel.selectedGenericPayroll.id = 0;
      this.viewModel.readonlyText = "false";
      this.viewModel.showButton = false;
      this.viewModel.editMode=false;
      this.viewModel.addMode=true;
    }
    this.viewModel.displayStyle = "block";
  }
  closePopup(payrollForm: NgForm) {
    this.viewModel.displayStyle = "none";
    this.viewModel.formSubmitted = false; // Clear the formSubmitted flag
    if (payrollForm) {
      payrollForm.reset(); // Reset the form
    }
  }
}
