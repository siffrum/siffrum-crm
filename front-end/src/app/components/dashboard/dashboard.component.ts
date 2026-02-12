import { Component, OnInit, ViewChild } from "@angular/core";
import { CommonService } from "src/app/services/common.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { DashboardViewModel } from "src/app/view-models/dashboard.viewmodel";
import { BaseComponent } from "../base.component";
import { AccountService } from "src/app/services/account.service";
import { AuthGuard } from "src/app/guard/auth.guard";
import { RoleTypeSM } from "src/app/service-models/app/enums/role-type-s-m.enum";
import { ModuleNameSM } from "src/app/service-models/app/enums/module-name-s-m.enum";
import Chart from 'chart.js/auto';
import { AdminDashboardService } from "src/app/services/admin-dashboard.service";

@Component({
    selector: "app-dashboard",
    templateUrl: "./dashboard.component.html",
    styleUrls: ["./dashboard.component.scss"],
    standalone: false
})
export class DashboardComponent
  extends BaseComponent<DashboardViewModel>
  implements OnInit {
  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private adminDashBoardService: AdminDashboardService,
    private accountService: AccountService,
    private authGuard: AuthGuard,

  ) {
    super(commonService, logService);
    this.viewModel = new DashboardViewModel();
  }
  // @DEV : Musaib
  async ngOnInit() {
    this._commonService.layoutVM.showLeftSideMenu = true;
    // this._commonService.layoutVM.toogleWrapper
    this._commonService.layoutVM.toggleSideMenu = 'default';
    this._commonService.layoutVM.toogleWrapper = 'wrapper';

    try {
      let roleType: any = RoleTypeSM[this._commonService.layoutVM.tokenRole];
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
    await setTimeout(async () => {
      await this._commonService.dismissLoader();
    }, 1000);

    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;
    if (!(await this.authGuard.IsTokenValid)) {
      await this._commonService.ShowToastAtTopEnd("Please Login", "warning");
    }
    await this._commonService.dismissLoader();
    await this.getloggedInUser();
    await this.loadPageData();
    this.getModulePermission();
    this.BarChart();
    this.PieChart();
  }
  // Add a property to store the sub-items for the report cards
  //get permissions

  getEmptySlotsCount(): any {
    let itemsWithPermissions = this._commonService.layoutVM.sideMenuItems.filter(item => item.permission);
    let remainingItems = 4 - (itemsWithPermissions.length % 4);
    return remainingItems === 4 ? 0 : remainingItems;
  }
  async getModulePermission(): Promise<ModuleNameSM | any> {
    this._commonService.layoutVM.dashboardItems.forEach(async (element) => {
      let permission = await this.accountService.getMyModulePermissions(
        element.moduleName
      );
      if (permission) {
        if (permission.isStandAlone)
          element.permission = permission.isEnabledForClient;
        else
          element.permission = permission.isEnabledForClient && permission.view;
      }
    });
  }
  /**
   * Get All Data For Dashboard
   */
  override async loadPageData() {
    try {
      await this._commonService.presentLoading();
      let resp =
        await this.adminDashBoardService.getAllDachboardDataItems();
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.adminDashboardVM = resp.successData;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  async getloggedInUser() {
    try {
      await this._commonService.presentLoading();
      let tokenValid = await this.authGuard.IsTokenValid();
      if (!tokenValid) {
        return false;
      } else {
        let user = await this.accountService.getUserFromStorage();
        if (user == "") {
          return false;
        }
        this.viewModel.employee = user;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
    return;
  }

  barChart: any = []
  BarChart(): void {
    // Extract department names and employee counts
    let employeeCounts = this.viewModel.adminDashboardVM.clientCompanyDepartment.map(data => data.employeeCount);
    let departmentNames = this.viewModel.adminDashboardVM.clientCompanyDepartment.map(data => data.departmentName);
    let maxBarsToShow = 10;
    let slicedDepartmentNames = departmentNames.slice(0, maxBarsToShow);
    let slicedEmployeeCounts = employeeCounts.slice(0, maxBarsToShow);
    this.barChart = new Chart('barChart', {
      type: 'bar',
      data: {
        labels: slicedDepartmentNames,
        datasets: [
          {
            label: 'No. Of Employees',
            data: slicedEmployeeCounts,
            backgroundColor: [
              'rgba(255, 99, 132, 0.2)',
              'rgba(255, 159, 64, 0.2)',
              'rgba(255, 205, 86, 0.2)',
              'rgba(75, 192, 192, 0.2)',
              'rgba(54, 162, 235, 0.2)',
              'rgba(153, 102, 255, 0.2)',
              'rgba(201, 203, 207, 0.2)'
            ],
            borderColor: [
              'rgb(255, 99, 132)',
              'rgb(255, 159, 64)',
              'rgb(255, 205, 86)',
              'rgb(75, 192, 192)',
              'rgb(54, 162, 235)',
              'rgb(153, 102, 255)',
              'rgb(201, 203, 207)'
            ],
            borderWidth: 1
          }]

      },
      options: {
        scales: {
          y: {
            type: 'linear', // Ensure you have 'linear' here
            position: 'left',
            beginAtZero: true,
          },
        },
      },
    });
  }
  pieChart: any = []
  PieChart() {
    this.pieChart = new Chart('pieChart', {
      type: 'pie',
      data: {
        labels: ['Present', 'Leave', 'Absent'],
        datasets: [
          {
            label: 'No. of Employees',
            data: [this.viewModel.adminDashboardVM.numberOfEmployeesPresent, this.viewModel.adminDashboardVM.numberOfEmployeeOnLeave, this.viewModel.adminDashboardVM.numberOfEmployeesAbsent],
            backgroundColor: [
              '#2ed8b6',
              '#FFB64D',
              '#FF5370',
            ],
            borderColor: [
              '#2ed8b6',
              '#FFB64D',
              '#FF5370',
            ],
            borderWidth: 1
          }]
      },
      options: {
        scales: {
          y: {
            type: 'linear', // Ensure you have 'linear' here
            position: 'left',
            beginAtZero: true,
          },
        },
      },
    });
  }



}

