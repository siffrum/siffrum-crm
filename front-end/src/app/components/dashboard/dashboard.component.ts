import { Component, OnInit } from "@angular/core";
import Chart from "chart.js/auto";

import { BaseComponent } from "../base.component";
import { CommonService } from "src/app/services/common.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { DashboardViewModel } from "src/app/view-models/dashboard.viewmodel";
import { AccountService } from "src/app/services/account.service";
import { AuthGuard } from "src/app/guard/auth.guard";
import { RoleTypeSM } from "src/app/service-models/app/enums/role-type-s-m.enum";
import { AdminDashboardService } from "src/app/services/admin-dashboard.service";

@Component({
  selector: "app-dashboard",
  templateUrl: "./dashboard.component.html",
  styleUrls: ["./dashboard.component.scss"],
  standalone: false,
})
export class DashboardComponent
  extends BaseComponent<DashboardViewModel>
  implements OnInit
{
  barChart: Chart | null = null;
  pieChart: Chart | null = null;

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private adminDashBoardService: AdminDashboardService,
    private accountService: AccountService,
    private authGuard: AuthGuard
  ) {
    super(commonService, logService);
    this.viewModel = new DashboardViewModel();
  }

  async ngOnInit() {
    this._commonService.layoutVM.showLeftSideMenu = true;
    this._commonService.layoutVM.toggleSideMenu = "default";
    this._commonService.layoutVM.toogleWrapper = "wrapper";

    try {
      const roleType: any = RoleTypeSM[this._commonService.layoutVM.tokenRole];
      if (roleType === "ClientAdmin" || roleType === "ClientEmployee") {
        await this._commonService.applyThemeGlobally();
      } else {
        await this._commonService.loadDefaultTheme();
      }
    } catch (error) {
      console.error("Error loading theme:", error);
    }

    // Loader
    await this._commonService.presentLoading();
    setTimeout(async () => {
      await this._commonService.dismissLoader();
    }, 800);

    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;

    const tokenValid = await this.authGuard.IsTokenValid();
    if (!tokenValid) {
      await this._commonService.ShowToastAtTopEnd("Please Login", "warning");
      return;
    }

    await this.getloggedInUser();
    await this.loadPageData();
  }

  // ==============================
  // GET LOGGED USER (NO TS ERROR)
  // ==============================
  async getloggedInUser(): Promise<boolean> {
    try {
      const tokenValid = await this.authGuard.IsTokenValid();
      if (!tokenValid) return false;

      const user: any = await this.accountService.getUserFromStorage();
      if (user == null || user === "") return false;

      if (typeof user === "string") {
        const raw = user.trim();
        if (!raw) return false;

        try {
          const parsed = JSON.parse(raw);
          (this.viewModel as any).employee = parsed;
          return true;
        } catch {
          (this.viewModel as any).employee = { userName: raw } as any;
          return true;
        }
      }

      (this.viewModel as any).employee = user;
      return true;
    } catch (error) {
      console.error(error);
      return false;
    }
  }

  // ==============================
  // LOAD DASHBOARD DATA
  // ==============================
  override async loadPageData() {
    try {
      await this._commonService.presentLoading();

      const resp = await this.adminDashBoardService.getAllDachboardDataItems();
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: "Error!",
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error",
        });
        return;
      }

      this.viewModel.adminDashboardVM = resp.successData;

      // Render charts AFTER DOM paints
      setTimeout(() => {
        this.BarChart();
        this.PieChart();
      }, 150);
    } catch (error) {
      console.error(error);
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  // ==============================
  // BAR CHART
  // ==============================
  BarChart(): void {
    const dept = this.viewModel.adminDashboardVM?.clientCompanyDepartment;
    if (!dept || !dept.length) return;

    const labels = dept.slice(0, 10).map((d: any) => d.departmentName);
    const values = dept.slice(0, 10).map((d: any) => d.employeeCount);

    if (this.barChart) {
      this.barChart.destroy();
      this.barChart = null;
    }

    this.barChart = new Chart("barChart", {
      type: "bar",
      data: {
        labels,
        datasets: [
          {
            label: "No. Of Employees",
            data: values,
            borderWidth: 1,
          },
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        scales: {
          y: { beginAtZero: true },
        },
      },
    });
  }

  // ==============================
  // PIE CHART
  // ==============================
  PieChart(): void {
    const vm = this.viewModel.adminDashboardVM;
    if (!vm) return;

    const present = vm.numberOfEmployeesPresent ?? 0;
    const leave = vm.numberOfEmployeeOnLeave ?? 0;
    const absent = vm.numberOfEmployeesAbsent ?? 0;

    if (this.pieChart) {
      this.pieChart.destroy();
      this.pieChart = null;
    }

    this.pieChart = new Chart("pieChart", {
      type: "pie",
      data: {
        labels: ["Present", "Leave", "Absent"],
        datasets: [
          {
            data: [present, leave, absent],
            borderWidth: 1,
          },
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
      },
    });
  }
}