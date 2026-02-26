import { Component, OnInit } from "@angular/core";
import { AppConstants } from "src/app-constants";
import { CommonService } from "src/app/services/common.service";
import { EmployeeService } from "src/app/services/employee.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { EmployeesListViewModel } from "src/app/view-models/employees-list.viewmodel";
import { BaseComponent } from "../base.component";
import { PermissionSM } from "src/app/service-models/app/v1/client/permission-s-m";
import { ModuleNameSM } from "src/app/service-models/app/enums/module-name-s-m.enum";
import { AccountService } from "src/app/services/account.service";

@Component({
  selector: "app-employees-list",
  templateUrl: "./employees-list.component.html",
  styleUrls: ["./employees-list.component.scss"],
  standalone: false,
})
export class EmployeesListComponent
  extends BaseComponent<EmployeesListViewModel>
  implements OnInit
{
  // ✅ Local UI filter (no API change)
  searchText = "";
  roleFilter = "";
  sortBy: "nameAsc" | "nameDesc" | "dojAsc" | "dojDesc" | "codeAsc" | "codeDesc" =
    "nameAsc";

  filteredEmployees: any[] = [];
  roleOptions: string[] = [];

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private employeeService: EmployeeService,
    private accountService: AccountService
  ) {
    super(commonService, logService);
    this.viewModel = new EmployeesListViewModel();
  }

  async ngOnInit(): Promise<void> {
    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;

    await this.loadPageData();
    await this.getPermissions();

    // ✅ ensure filtered list is ready on first load too
    this.applyLocalFilter();
  }

  // ✅ Check permissions
  async getPermissions(): Promise<void> {
    const moduleName = ModuleNameSM.EmployeeDirectory;
    const resp: PermissionSM | any =
      await this.accountService.getMyModulePermissions(moduleName);
    this.viewModel.Permission = resp;
  }

  override async loadPageData(): Promise<void> {
    try {
      await this._commonService.presentLoading();

      // count + companyId + list
      await this.getEmployeeCountOfCompany();
      await this._commonService.getCompanyIdFromStorage();

      const companyId = this._commonService.layoutVM.company.id;

      const resp = await this.employeeService.getAllEmployeeByCompanyIdAndOdata(
        this.viewModel,
        companyId
      );

      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: "Error!",
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error",
        });
      } else {
        this.viewModel.employeesList = resp.successData;

        // ✅ update local filtered list whenever data changes
        this.applyLocalFilter();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  async deleteEmployee(employeeId: number): Promise<void> {
    const deleteEmployeeConfirmation =
      await this._commonService.showConfirmationAlert(
        AppConstants.DefaultMessages.EmployeeDeleteAlert,
        " ",
        true,
        "warning"
      );

    if (!deleteEmployeeConfirmation) return;

    try {
      await this._commonService.presentLoading();

      const resp = await this.employeeService.deleteEmployee(employeeId);

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
          resp.successData.deleteMessage,
          "success"
        );
        await this.loadPageData(); // reload list
      }
    } catch (error) {
      // optional: log if you want
      // await this._exceptionHandler.logObject(error);
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  /**
   * GET Total Count Of Employees
   */
  async getEmployeeCountOfCompany(): Promise<void> {
    try {
      await this._commonService.presentLoading();

      const resp = await this.employeeService.getEmployeeCountOfCompany();

      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: "Error!",
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error",
        });
      } else {
        this.viewModel.pagination.totalCount = resp.successData.intResponse;
      }
    } catch (error) {
      // optional: log
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  async loadPagedataWithPagination(pageNumber: number): Promise<void> {
    if (pageNumber && pageNumber > 0) {
      this.viewModel.pagination.PageNo = pageNumber;
      await this.loadPageData();
    }
  }

  // ===========================
  // ✅ Local filter/sort helpers
  // ===========================

  applyLocalFilter(): void {
    const list = (this.viewModel?.employeesList || []).slice();

    // Roles list
    const roles = Array.from(
      new Set(
        list
          .map((x: any) => (x?.roleType || "").toString())
          .filter((x: string) => !!x)
      )
    );
    this.roleOptions = roles.sort((a, b) => a.localeCompare(b));

    const q = (this.searchText || "").trim().toLowerCase();

    let result = list.filter((x: any) => {
      // role filter
      if (this.roleFilter && (x?.roleType || "").toString() !== this.roleFilter)
        return false;

      // text search
      if (!q) return true;

      const hay = [
        x?.employeeCode,
        x?.roleType,
        x?.loginId,
        x?.emailId,
        x?.phoneNumber,
        x?.designation,
      ]
        .map((v: any) => (v ?? "").toString().toLowerCase())
        .join(" | ");

      return hay.includes(q);
    });

    const toTime = (d: any) => {
      const t = new Date(d).getTime();
      return isNaN(t) ? 0 : t;
    };

    result.sort((a: any, b: any) => {
      const aName = (a?.loginId || "").toString();
      const bName = (b?.loginId || "").toString();
      const aCode = (a?.employeeCode || "").toString();
      const bCode = (b?.employeeCode || "").toString();
      const aDoj = toTime(a?.dateOfJoining);
      const bDoj = toTime(b?.dateOfJoining);

      switch (this.sortBy) {
        case "nameAsc":
          return aName.localeCompare(bName);
        case "nameDesc":
          return bName.localeCompare(aName);
        case "codeAsc":
          return aCode.localeCompare(bCode);
        case "codeDesc":
          return bCode.localeCompare(aCode);
        case "dojAsc":
          return aDoj - bDoj;
        case "dojDesc":
          return bDoj - aDoj;
        default:
          return 0;
      }
    });

    this.filteredEmployees = result;
  }

  clearSearch(): void {
    this.searchText = "";
    this.applyLocalFilter();
  }

  trackByEmpId = (_: number, item: any) => item?.id;
}