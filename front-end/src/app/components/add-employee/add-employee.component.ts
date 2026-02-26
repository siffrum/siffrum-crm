import { Component, OnInit, HostListener } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";

import { ClientUserAddressSM } from "src/app/service-models/app/v1/app-users/client-user-address-s-m";
import { ClientUserSM } from "src/app/service-models/app/v1/app-users/client-user-s-m";
import { ClientEmployeeBankDetailSM } from "src/app/service-models/app/v1/client/client-employee-bank-detail-s-m";

import { CommonService } from "src/app/services/common.service";
import { EmployeeService } from "src/app/services/employee.service";
import { LogHandlerService } from "src/app/services/log-handler.service";

import { AddEmployeeViewModel } from "src/app/view-models/add-employee.viewmodel";
import { BaseComponent } from "../base.component";

@Component({
  selector: "app-add-employee",
  templateUrl: "./add-employee.component.html",
  styleUrls: ["./add-employee.component.scss"],
  standalone: false,
})
export class AddEmployeeComponent
  extends BaseComponent<AddEmployeeViewModel>
  implements OnInit
{
  // Wizard total steps: 0..4
  readonly totalSteps = 5;

  // Main model passed to child components
  employeeInfo: ClientUserSM = new ClientUserSM();

  // Keep these so we can control "Next" enable rules (optional but useful)
  lastSavedEmployeeId = 0;
  lastSavedAddressId = 0;
  lastSavedBankId = 0;

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private employeeService: EmployeeService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {
    super(commonService, logService);
    this.viewModel = new AddEmployeeViewModel();
  }

  async ngOnInit() {
    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;

    // Decide Add vs Edit mode based on route param
    const idParam = this.activatedRoute.snapshot.paramMap.get("Id");
    const Id = Number(idParam);

    // If no Id => ADD mode
    if (!Id || Id === 0) {
      this.viewModel.isAddMode = true;
      // start at step 0
      this.viewModel.wizardLocation = 0;
      return;
    }

    // EDIT mode
    this.viewModel.isAddMode = false;
    this.viewModel.wizardLocation = 0;
    await this.getEmployee(Id);
    this.lastSavedEmployeeId = this.employeeInfo?.id || 0;
  }

  // Close on ESC (nice UX)
  @HostListener("document:keydown.escape", ["$event"])
  onEsc(_event: KeyboardEvent) {
    // If you later add a modal, handle here.
  }

  /* --------------------------
   * Child Outputs (unchanged)
   * -------------------------- */

  async recieveEmployeeFromChild(employee: ClientUserSM) {
    if (employee && employee.id > 0) {
      await this.getEmployee(employee.id);
      this.lastSavedEmployeeId = employee.id;
      this.nextWizardLocation();
    }
  }

  async recieveAddressFromChild(employeeAddress: ClientUserAddressSM) {
    if (employeeAddress && employeeAddress.id > 0) {
      this.lastSavedAddressId = employeeAddress.id;
      this.nextWizardLocation();
    }
  }

  async recieveBankFromChild(employeeBank: ClientEmployeeBankDetailSM) {
    if (employeeBank && employeeBank.id > 0) {
      this.lastSavedBankId = employeeBank.id;
      this.nextWizardLocation();
    }
  }

  /* --------------------------
   * Data Load
   * -------------------------- */

  /**
   * Get Employee Details
   * @param employeeId
   */
  async getEmployee(employeeId: number) {
    try {
      await this._commonService.presentLoading();
      const resp = await this.employeeService.getEmployeeByEmployeeId(employeeId);

      if (resp?.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: "Error!",
          text: resp.errorData?.displayMessage || "Something went wrong",
          position: "top-end",
          icon: "error",
        });
      } else {
        this.employeeInfo = resp.successData;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  /* --------------------------
   * Wizard Controls
   * -------------------------- */

  nextWizardLocation() {
    const next = this.viewModel.wizardLocation + 1;
    if (next <= this.totalSteps - 1) {
      this.viewModel.wizardLocation = next;
    }
  }

  previousWizardLocation() {
    const prev = this.viewModel.wizardLocation - 1;
    if (prev >= 0) {
      this.viewModel.wizardLocation = prev;
    }
  }

  /**
   * Allow clicking steps only up to current step (no jumping ahead).
   * If you want to allow jumping back freely, this already does it.
   */
  goToStep(stepIndex: number) {
    if (stepIndex < 0 || stepIndex > this.totalSteps - 1) return;
    if (stepIndex <= this.viewModel.wizardLocation) {
      this.viewModel.wizardLocation = stepIndex;
    }
  }

  /**
   * Controls the manual "Next" button (if you use it).
   * Keeps your original flow (child emit moves next), but allows manual next if safe.
   */
  canGoNext(): boolean {
    const step = this.viewModel.wizardLocation;

    // Step 0 requires employee saved (id > 0)
    if (step === 0) return (this.employeeInfo?.id || 0) > 0 || this.lastSavedEmployeeId > 0;

    // Step 1 can require address saved (if you want strict gating)
    // return this.lastSavedAddressId > 0;
    if (step === 1) return true;

    // Step 2 can require bank saved (if you want strict gating)
    // return this.lastSavedBankId > 0;
    if (step === 2) return true;

    // Steps 3 -> 4: allow
    if (step === 3) return true;

    return false;
  }

  /**
   * Finish wizard action (optional).
   * You can navigate to list page or details page.
   */
  async finishWizard() {
    await this._commonService.ShowToastAtTopEnd("Employee wizard completed", "success");

    // Example navigation (change route to your actual list route)
    // this.router.navigate(["/employees-list"]);

    // Or go back:
    // window.history.back();
  }
}