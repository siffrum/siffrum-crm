import {
  AfterViewInit,
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
} from "@angular/core";
import { Router } from "@angular/router";
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
  implements OnInit, AfterViewInit, OnDestroy
{
  @ViewChild("barCanvas") barCanvas?: ElementRef<HTMLCanvasElement>;
  @ViewChild("pieCanvas") pieCanvas?: ElementRef<HTMLCanvasElement>;
  @ViewChild("lineCanvas") lineCanvas?: ElementRef<HTMLCanvasElement>;

  barChart: Chart | null = null;
  pieChart: Chart | null = null;
  lineChart: Chart | null = null;

  private viewReady = false;
  private dataReady = false;

  todayLabel = "";

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private adminDashBoardService: AdminDashboardService,
    private accountService: AccountService,
    private authGuard: AuthGuard,
    private router: Router
  ) {
    super(commonService, logService);
    this.viewModel = new DashboardViewModel();
  }

  // --------- Safe getters (no template crashes) ----------
  get adminsCount(): number {
    return this.viewModel.adminDashboardVM?.numberOfAdmins ?? 0;
  }
  get employeesCount(): number {
    return this.viewModel.adminDashboardVM?.numberOfEmployees ?? 0;
  }

  get leavesApproved(): number {
    return this.viewModel.adminDashboardVM?.numberOfLeavesApproved ?? 0;
  }
  get leavesRejected(): number {
    return this.viewModel.adminDashboardVM?.numberOfLeavesRejected ?? 0;
  }
  get leavesPending(): number {
    return this.viewModel.adminDashboardVM?.numberOfLeavesPending ?? 0;
  }
  get leavesTotal(): number {
    return this.leavesApproved + this.leavesRejected + this.leavesPending;
  }

  get presentCount(): number {
    return this.viewModel.adminDashboardVM?.numberOfEmployeesPresent ?? 0;
  }
  get leaveCount(): number {
    return this.viewModel.adminDashboardVM?.numberOfEmployeeOnLeave ?? 0;
  }
  get absentCount(): number {
    return this.viewModel.adminDashboardVM?.numberOfEmployeesAbsent ?? 0;
  }
  get attendanceTotal(): number {
    return this.presentCount + this.leaveCount + this.absentCount;
  }

  get hasDepartments(): boolean {
    const d = this.viewModel.adminDashboardVM?.clientCompanyDepartment;
    return Array.isArray(d) && d.length > 0;
  }

  async ngOnInit() {
    this.todayLabel = new Date().toLocaleDateString(undefined, {
      weekday: "short",
      month: "short",
      day: "2-digit",
    });

    this._commonService.layoutVM.showLeftSideMenu = true;
    this._commonService.layoutVM.toggleSideMenu = "default";
    this._commonService.layoutVM.toogleWrapper = "wrapper";
    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle || "Dashboard";

    // Theme
    try {
      const roleType: any = RoleTypeSM[this._commonService.layoutVM.tokenRole];
      if (roleType === "ClientAdmin" || roleType === "ClientEmployee") {
        await this._commonService.applyThemeGlobally();
      } else {
        await this._commonService.loadDefaultTheme();
      }
    } catch (e) {
      console.error("Theme error:", e);
    }

    // Auth
    const tokenValid = await this.authGuard.IsTokenValid();
    if (!tokenValid) {
      await this._commonService.ShowToastAtTopEnd("Please Login", "warning");
      return;
    }

    await this.getloggedInUser();
    await this.loadPageData();
  }

  ngAfterViewInit(): void {
    this.viewReady = true;
    this.tryRenderCharts();
  }

  ngOnDestroy(): void {
    this.destroyCharts();
  }

  refresh(): void {
    this.loadPageData();
  }

  go(path: string): void {
    this.router.navigate([path]);
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

      this.dataReady = true;
      this.tryRenderCharts();
    } catch (error) {
      console.error(error);
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  private tryRenderCharts(): void {
    if (!this.viewReady || !this.dataReady) return;

    // render after Angular paints
    setTimeout(() => {
      this.renderBar();
      this.renderPie();
      this.renderLine();
    }, 60);
  }

  private destroyCharts(): void {
    if (this.barChart) { this.barChart.destroy(); this.barChart = null; }
    if (this.pieChart) { this.pieChart.destroy(); this.pieChart = null; }
    if (this.lineChart) { this.lineChart.destroy(); this.lineChart = null; }
  }

  // ==============================
  // BAR: Employees by department
  // ==============================
  private renderBar(): void {
    if (!this.barCanvas?.nativeElement) return;

    const dept = this.viewModel.adminDashboardVM?.clientCompanyDepartment || [];
    if (!Array.isArray(dept) || dept.length === 0) {
      if (this.barChart) { this.barChart.destroy(); this.barChart = null; }
      return;
    }

    const top = dept.slice(0, 10);
    const labels = top.map((d: any) => d.departmentName || "Dept");
    const values = top.map((d: any) => Number(d.employeeCount ?? 0));

    if (this.barChart) this.barChart.destroy();

    this.barChart = new Chart(this.barCanvas.nativeElement, {
      type: "bar",
      data: {
        labels,
        datasets: [
          {
            label: "Employees",
            data: values,
            borderWidth: 1,
            borderRadius: 10,
            maxBarThickness: 44,
          },
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { display: false },
          tooltip: { enabled: true },
        },
        scales: {
          x: {
            grid: { display: false },
            ticks: { maxRotation: 0, autoSkip: true },
          },
          y: {
            beginAtZero: true,
            ticks: { precision: 0 },
          },
        },
      },
    });
  }

  // ==============================
  // PIE: Attendance split
  // ==============================
  private renderPie(): void {
    if (!this.pieCanvas?.nativeElement) return;

    const present = this.presentCount;
    const leave = this.leaveCount;
    const absent = this.absentCount;

    if (this.pieChart) this.pieChart.destroy();

    this.pieChart = new Chart(this.pieCanvas.nativeElement, {
      type: "doughnut",
      data: {
        labels: ["Present", "Leave", "Absent"],
        datasets: [
          {
            data: [present, leave, absent],
            borderWidth: 2,
            hoverOffset: 6,
          },
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        cutout: "62%",
        plugins: {
          legend: { position: "bottom" },
        },
      },
    });
  }

  // ==============================
  // LINE: Leaves trend (demo)
  // ==============================
  private renderLine(): void {
    if (!this.lineCanvas?.nativeElement) return;

    // demo trend - replace with API later
    const labels = ["Aug", "Sep", "Oct", "Nov", "Dec", "Jan"];
    const data = [
      Math.max(0, this.leavesPending - 2),
      Math.max(0, this.leavesPending - 1),
      this.leavesPending,
      this.leavesPending + 1,
      Math.max(0, this.leavesPending - 1),
      this.leavesPending + 2,
    ];

    if (this.lineChart) this.lineChart.destroy();

    this.lineChart = new Chart(this.lineCanvas.nativeElement, {
      type: "line",
      data: {
        labels,
        datasets: [
          {
            label: "Pending leaves",
            data,
            tension: 0.35,
            fill: true,
            pointRadius: 4,
            borderWidth: 2,
          },
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { display: false },
          tooltip: { enabled: true },
        },
        scales: {
          x: { grid: { display: false } },
          y: { beginAtZero: true, ticks: { precision: 0 } },
        },
      },
    });
  }
}