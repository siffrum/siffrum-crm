import { DecimalPipe } from "@angular/common";
import { Component, HostListener, Input, OnDestroy, OnInit } from "@angular/core";
import { NgForm } from "@angular/forms";

import { BaseComponent } from "src/app/components/base.component";
import { CommonService } from "src/app/services/common.service";
import { EmployeeService } from "src/app/services/employee.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { PayrollService } from "src/app/services/payroll.service";
import { AccountService } from "src/app/services/account.service";

import {
  EmployeeSalaryInfoViewModel,
  ExtendedClientEmployeePayrollComponentSM,
  ExtendedClientEmployeeSalarySM,
  UpdateComponentsType,
} from "src/app/view-models/employee-salary-info.viewmodel";

import { ComponentCalculationTypeSM } from "src/app/service-models/app/enums/component-calculation-type-s-m.enum";
import { ComponentPeriodTypeSM } from "src/app/service-models/app/enums/component-period-type-s-m.enum";
import { ModuleNameSM } from "src/app/service-models/app/enums/module-name-s-m.enum";
import { RoleTypeSM } from "src/app/service-models/app/enums/role-type-s-m.enum";

import { PermissionSM } from "src/app/service-models/app/v1/client/permission-s-m";
import { ClientEmployeeCTCDetailSM } from "src/app/service-models/app/v1/client/client-employee-c-t-c-detail-s-m";
import { ClientGenericPayrollComponentSM } from "src/app/service-models/app/v1/client/client-generic-payroll-component-s-m";

@Component({
  selector: "app-employee-salary-info",
  templateUrl: "./employee-salary-info.component.html",
  styleUrls: ["./employee-salary-info.component.scss"],
  standalone: false,
})
export class EmployeeSalaryInfoComponent
  extends BaseComponent<EmployeeSalaryInfoViewModel>
  implements OnInit, OnDestroy
{
  @Input() empid!: number;
  @Input() readonly!: boolean;
  @Input() isaddmode!: boolean;

  roleType: any;

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private employeeService: EmployeeService,
    private payrollService: PayrollService,
    private decimalPipe: DecimalPipe,
    private accountService: AccountService
  ) {
    super(commonService, logService);
    this.viewModel = new EmployeeSalaryInfoViewModel();
  }

  // ✅ ESC key closes fullscreen overlay
  @HostListener("document:keydown.escape")
  onEsc() {
    if (this.viewModel?.displayStyle === "block") {
      this.closePopup();
    }
  }

  ngOnDestroy(): void {
    this.lockBodyScroll(false);
  }

  // ✅ Fix for template warning: return safe min date string "YYYY-MM-DD"
  get minStartDate(): string {
    const v = this.viewModel?.startDateUTC;

    if (!v) return ""; // IMPORTANT: keep string (not null)

    // if already string
    if (typeof v === "string") return v.slice(0, 10);

    // if somehow Date slipped in
    try {
      return new Date(v as any).toISOString().slice(0, 10);
    } catch {
      return "";
    }
  }

  async ngOnInit() {
    this.roleType = RoleTypeSM[this._commonService.layoutVM.tokenRole];

    // ✅ FIX: your old line "this.viewModel.hideAddButton;" did nothing
    // Your template uses *ngIf="viewModel.hideAddButton" to show Add button.
    // So: ClientEmployee => hide button (false), others => show (true)
    if (this.roleType === "ClientEmployee") {
      this.viewModel.hideAddButton = false;
    } else {
      this.viewModel.hideAddButton = true;
    }

    if (!this.isaddmode) {
      await this.getEmployeeSalaryCount();
      await this.loadPageData();
    }

    await this.getPermissions();
  }

  private lockBodyScroll(lock: boolean) {
    document.body.style.overflow = lock ? "hidden" : "";
  }

  async getPermissions() {
    const ModuleName = ModuleNameSM.EmployeeCTC;
    const resp: PermissionSM | any =
      await this.accountService.getMyModulePermissions(ModuleName);
    this.viewModel.Permission = resp;
  }

  /** Get Employee CTC Details */
  override async loadPageData() {
    try {
      await this._commonService.presentLoading();

      await this.getEmployeeSalaryCount();
      await this._commonService.getCompanyIdFromStorage();

      const resp = await this.employeeService.getEmployeeSalaryInfoByOdata(
        this.viewModel,
        this.empid
      );

      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData.displayMessage);
        this._commonService.showSweetAlertToast({
          title: "Error!",
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error",
        });
        return;
      }

      this.viewModel.employeeSalaryList = resp.successData;

      // ✅ IMPORTANT FIX: startDateUTC must remain STRING
      const activeSalary = (this.viewModel.employeeSalaryList || []).find(
        (x) => x.currentlyActive === true
      );

      // Make sure string value
      const raw = activeSalary?.startDateUTC;

      if (!raw) {
        this.viewModel.startDateUTC = ""; // ✅ string (not null)
      } else if (typeof raw === "string") {
        this.viewModel.startDateUTC = raw; // keep as-is
      } else {
        // if Date object came
        this.viewModel.startDateUTC = new Date(raw as any).toISOString();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  editDetails() {
    this.viewModel.isReadonly = !this.viewModel.isReadonly;
    this.viewModel.showButton = !this.viewModel.showButton;
  }

  /** Open Fullscreen Modal to Add / View Salary Info */
  async openSalaryInfoModal(ctcId: number) {
    this.viewModel.selectedEmployeeSalary = new ExtendedClientEmployeeSalarySM();
    this.viewModel.listCompomentPeriodType = [];

    this.viewModel.listCompomentPeriodType.push(
      this._commonService.SingleEnumToString(
        ComponentPeriodTypeSM,
        ComponentPeriodTypeSM.Monthly
      ),
      this._commonService.SingleEnumToString(
        ComponentPeriodTypeSM,
        ComponentPeriodTypeSM.Yearly
      )
    );

    this.viewModel.listPayrollCalculationType =
      this._commonService.EnumToStringArray(ComponentCalculationTypeSM);

    this.viewModel.enableComponentsEdit = false;

    if (ctcId > 0) {
      this.viewModel.ctcViewMode = true;

      const resp = await this.payrollService.GetEmployeeSalaryByCTCId(ctcId);
      if (resp?.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: "Error!",
          text: resp?.errorData?.displayMessage || "Unable to load CTC details",
          position: "top-end",
          icon: "error",
        });
        return;
      }

      await this.convertToExtendedClientEmployeeSalarySM(resp.successData);
    } else {
      this.viewModel.ctcViewMode = false;
      this.viewModel.selectedEmployeeSalary = new ExtendedClientEmployeeSalarySM();

      const resp = await this.payrollService.getGenericComponents();
      if (resp?.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: "Error!",
          text:
            resp?.errorData?.displayMessage ||
            "Unable to load payroll components",
          position: "top-end",
          icon: "error",
        });
        return;
      }

      resp.successData.forEach((_genericComp: ClientGenericPayrollComponentSM) => {
        this.viewModel.selectedEmployeeSalary.clientEmployeePayrollComponents.push(
          this.convertToExtendedClientEmployeePayrollComponentSM(_genericComp)
        );
      });
    }

    this.UpdateTotalMonthlyInfo();

    this.viewModel.displayStyle = "block";
    this.lockBodyScroll(true);
  }

  closePopup(salaryInputForm?: NgForm) {
    this.viewModel.displayStyle = "none";
    this.viewModel.formSubmitted = false;
    this.lockBodyScroll(false);

    if (salaryInputForm) {
      salaryInputForm.reset();
    }
  }

  private async convertToExtendedClientEmployeeSalarySM(
    successData: ClientEmployeeCTCDetailSM
  ) {
    this.viewModel.selectedEmployeeSalary = {
      ...successData,
      otherComponents: 0,
      percentageUI: 0,
      ctcMonthly: successData.ctcAmount / 12,
      amountMonthly: 0,
      clientEmployeePayrollComponents: [],
    };

    successData.clientEmployeePayrollComponents.forEach((componentSM) => {
      const _tempComponent: ExtendedClientEmployeePayrollComponentSM =
        this.convertToExtendedClientEmployeePayrollComponentSM(componentSM);

      _tempComponent.amountYearly = componentSM.amountYearly;
      _tempComponent.clientEmployeeCTCDetailId = componentSM.clientEmployeeCTCDetailId;

      this.updateDetailsByYearlyAmount(_tempComponent, false);

      this.viewModel.selectedEmployeeSalary.clientEmployeePayrollComponents.push(
        _tempComponent
      );
    });

    this.viewModel.selectedEmployeeSalary.startDateUTC =
      await this.getFormattedDate(
        this.viewModel.selectedEmployeeSalary.startDateUTC,
        false
      );
  }

  private convertToExtendedClientEmployeePayrollComponentSM(
    genericComponentSM: ClientGenericPayrollComponentSM
  ): ExtendedClientEmployeePayrollComponentSM {
    const _tempComponent: ExtendedClientEmployeePayrollComponentSM = {
      ...genericComponentSM,
      amountMonthlyUI: 0,
      amountYearlyUI: 0,
      percentageUI: genericComponentSM.percentage,
      amountMonthly: 0,
      amountYearly: 0,
      clientEmployeeCTCDetailId: 0,
    };
    return _tempComponent;
  }

  private UpdateTotalMonthlyInfo() {
    this.viewModel.selectedEmployeeSalary.amountMonthly =
      this.viewModel.selectedEmployeeSalary.clientEmployeePayrollComponents.reduce(
        (sum, current) => {
          if (
            current.componentPeriodType ==
            this._commonService.SingleEnumToString(
              ComponentPeriodTypeSM,
              ComponentPeriodTypeSM.Monthly
            )
          ) {
            if (
              current.componentCalculationType ==
              this._commonService.SingleEnumToString(
                ComponentCalculationTypeSM,
                ComponentCalculationTypeSM.Addition
              )
            ) {
              return sum + current.amountMonthly;
            } else {
              return sum - current.amountMonthly;
            }
          }
          return sum;
        },
        0
      );

    this.viewModel.selectedEmployeeSalary.otherComponents =
      this.viewModel.selectedEmployeeSalary.ctcMonthly -
      this.viewModel.selectedEmployeeSalary.amountMonthly;

    this.viewModel.selectedEmployeeSalary.percentageUI =
      (this.viewModel.selectedEmployeeSalary.clientEmployeePayrollComponents.reduce(
        (sum, current) => sum + current.percentageUI,
        0
      ) *
        100) /
      100;

    if (this.viewModel.selectedEmployeeSalary.percentageUI > 100) {
      this.viewModel.showPercentagValidationMessage = true;
      this.viewModel.percentagValidationMessage =
        "percentage can't be greater than 100!";
    } else {
      this.viewModel.showPercentagValidationMessage = false;
      this.viewModel.percentagValidationMessage = "";
    }
  }

  async UpdateComponents(updateType: UpdateComponentsType, componentId: number) {
    this.viewModel.enableComponentsEdit = true;

    switch (updateType) {
      case UpdateComponentsType.CTC: {
        const amt = this.viewModel.selectedEmployeeSalary.ctcAmount;

        // ✅ FIX: old condition was wrong
        if (amt == null || amt <= 0) {
          this.viewModel.enableComponentsEdit = false;
          break;
        }

        this.viewModel.selectedEmployeeSalary.ctcMonthly = amt / 12;

        this.viewModel.selectedEmployeeSalary.clientEmployeePayrollComponents.forEach(
          (element) => {
            if (element.percentage != null && element.percentage > 0) {
              this.UpdateAmountUsingPercentage(element, false);
            } else {
              switch (element.componentPeriodType) {
                case this._commonService.SingleEnumToString(
                  ComponentPeriodTypeSM,
                  ComponentPeriodTypeSM.Monthly
                ): {
                  this.updateDetailsByMonthlyAmount(element, false);
                  break;
                }
                case this._commonService.SingleEnumToString(
                  ComponentPeriodTypeSM,
                  ComponentPeriodTypeSM.Yearly
                ): {
                  // ✅ was bug in your code: you were calling monthly
                  this.updateDetailsByYearlyAmount(element, false);
                  break;
                }
              }
            }
          }
        );
        break;
      }

      case UpdateComponentsType.ComponentAmountMonthly: {
        const item =
          this.viewModel.selectedEmployeeSalary.clientEmployeePayrollComponents.find(
            (x) => x.id == componentId
          );
        if (item) this.updateDetailsByMonthlyAmount(item, true);
        break;
      }

      case UpdateComponentsType.ComponentAmountYearly: {
        const item =
          this.viewModel.selectedEmployeeSalary.clientEmployeePayrollComponents.find(
            (x) => x.id == componentId
          );
        if (item) this.updateDetailsByYearlyAmount(item, true);
        break;
      }

      case UpdateComponentsType.ComponentPercentage: {
        const item =
          this.viewModel.selectedEmployeeSalary.clientEmployeePayrollComponents.find(
            (x) => x.id == componentId
          );
        if (item) this.UpdateAmountUsingPercentage(item, true);
        break;
      }

      case UpdateComponentsType.ComponentCalculationPeriod:
      case UpdateComponentsType.ComponentCalculationType: {
        break;
      }
    }

    this.UpdateTotalMonthlyInfo();
  }

  private updateDetailsByMonthlyAmount(
    payrollComponent: ExtendedClientEmployeePayrollComponentSM,
    useUIamount: boolean
  ) {
    if (useUIamount)
      payrollComponent.amountMonthly = payrollComponent.amountMonthlyUI;

    payrollComponent.amountYearly = payrollComponent.amountMonthly * 12;

    payrollComponent.percentage =
      (payrollComponent.amountYearly /
        this.viewModel.selectedEmployeeSalary.ctcAmount) *
      100;

    this.UIValuesToApproximate(payrollComponent);
  }

  private updateDetailsByYearlyAmount(
    payrollComponent: ExtendedClientEmployeePayrollComponentSM,
    useUIamount: boolean
  ) {
    if (useUIamount)
      payrollComponent.amountYearly = payrollComponent.amountYearlyUI;

    payrollComponent.amountMonthly = payrollComponent.amountYearly / 12;

    payrollComponent.percentage =
      (payrollComponent.amountYearly /
        this.viewModel.selectedEmployeeSalary.ctcAmount) *
      100;

    this.UIValuesToApproximate(payrollComponent);
  }

  private UpdateAmountUsingPercentage(
    payrollComponent: ExtendedClientEmployeePayrollComponentSM,
    useUIpercent: boolean
  ) {
    if (useUIpercent) payrollComponent.percentage = payrollComponent.percentageUI;

    payrollComponent.amountMonthly =
      this.viewModel.selectedEmployeeSalary.ctcMonthly *
      (payrollComponent.percentage / 100);

    payrollComponent.amountYearly =
      this.viewModel.selectedEmployeeSalary.ctcAmount *
      (payrollComponent.percentage / 100);

    this.UIValuesToApproximate(payrollComponent);
  }

  private UIValuesToApproximate(
    payrollComponent: ExtendedClientEmployeePayrollComponentSM
  ) {
    const amtYearly = this.decimalPipe.transform(
      payrollComponent.amountYearly,
      "2.2-2"
    );
    if (amtYearly != null)
      payrollComponent.amountYearlyUI = parseFloat(amtYearly.replace(/,/g, ""));

    const amtMonthly = this.decimalPipe.transform(
      payrollComponent.amountMonthly,
      "2.2-2"
    );
    if (amtMonthly != null)
      payrollComponent.amountMonthlyUI = parseFloat(
        amtMonthly.replace(/,/g, "")
      );

    const y = Math.ceil(payrollComponent.percentage * 100) / 100;
    if (Math.floor(y) == Math.floor(payrollComponent.percentage))
      payrollComponent.percentageUI =
        Math.ceil(payrollComponent.percentage * 100) / 100;
    else
      payrollComponent.percentageUI =
        Math.floor(payrollComponent.percentage * 100) / 100;
  }

  async addEmployeeSalary(salaryInputForm: NgForm) {
    this.viewModel.formSubmitted = true;

    try {
      if (salaryInputForm.invalid) {
        await this._commonService.showSweetAlertToast({
          title: "Form Fields Are Not valid !",
          icon: "error",
        });
        return;
      }

      if (this.viewModel.selectedEmployeeSalary.percentageUI > 100) {
        await this._commonService.showSweetAlertToast({
          title: "percentage can't be greater than 100!",
          icon: "error",
        });
        return;
      }

      // ✅ FIX: old code was (x.id = 0) assignment inside filter (wrong)
      this.viewModel.selectedEmployeeSalary.clientEmployeePayrollComponents =
        this.viewModel.selectedEmployeeSalary.clientEmployeePayrollComponents.map(
          (x: any) => ({ ...x, id: 0 })
        );

      this.viewModel.selectedEmployeeSalary.clientUserId = this.empid;
      this.viewModel.selectedEmployeeSalary.currentlyActive = false;

      const resp = await this.employeeService.addEmployeeSalary(
        this.viewModel.selectedEmployeeSalary
      );

      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: "Error!",
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error",
        });
      } else {
        this._commonService.ShowToastAtTopEnd(
          "Employee Salary Added Successfully",
          "success"
        );

        this.viewModel.displayStyle = "none";
        this.lockBodyScroll(false);

        await this.loadPageData();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  async getEmployeeSalaryCount() {
    try {
      const resp = await this.employeeService.getEmployeeSalaryCount(this.empid);
      if (!resp.isError) {
        this.viewModel.pagination.totalCount = resp.successData.intResponse;
      }
    } catch (e) {}
  }

  async loadPagedataWithPagination(pageNumber: number) {
    if (pageNumber && pageNumber > 0) {
      this.viewModel.pagination.PageNo = pageNumber;
      await this.loadPageData();
    }
  }
}