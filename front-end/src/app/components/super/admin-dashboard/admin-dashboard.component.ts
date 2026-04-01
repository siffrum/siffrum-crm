import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';
import { CommonService } from 'src/app/services/common.service';
import { LogHandlerService } from 'src/app/services/log-handler.service';
import { AdminDashboardViewModel } from 'src/app/view-models/admin-dashboard.viewmodel';
import { BaseComponent } from '../../base.component';
import { CompanyListService } from 'src/app/services/company-list.service';
import { ContactUsService } from 'src/app/services/contact-us.service';
import { SqlReportService } from 'src/app/services/sql-report.service';
import { AppConstants } from 'src/app-constants';

@Component({
    selector: 'app-admin-dashboard',
    templateUrl: './admin-dashboard.component.html',
    styleUrls: ['./admin-dashboard.component.scss'],
    standalone: false
})
export class AdminDashboardComponent extends BaseComponent<AdminDashboardViewModel> implements OnInit {
  stats = [
    { label: 'Companies', value: '0', icon: 'bi bi-buildings', tone: 'blue' },
    { label: 'Contact Requests', value: '0', icon: 'bi bi-chat-left-text', tone: 'amber' },
    { label: 'SQL Reports', value: '0', icon: 'bi bi-bar-chart-line', tone: 'green' },
    { label: 'Admin Modules', value: '5', icon: 'bi bi-grid-1x2-fill', tone: 'violet' },
  ];

  quickLinks = [
    {
      title: 'Add Company',
      description: 'Launch the onboarding wizard for a new client workspace.',
      route: AppConstants.WebRoutes.ADMIN.ADD_COMPANY,
      icon: 'bi bi-building-add',
    },
    {
      title: 'Company Dashboard',
      description: 'Review companies, statuses, and jump into company details.',
      route: AppConstants.WebRoutes.ADMIN.COMPANIES,
      icon: 'bi bi-buildings',
    },
    {
      title: 'SQL Reports',
      description: 'Manage internal reports and analytics queries.',
      route: AppConstants.WebRoutes.ADMIN.SQL,
      icon: 'bi bi-bar-chart-line',
    },
    {
      title: 'Contact Inbox',
      description: 'Handle submitted support and contact requests.',
      route: AppConstants.WebRoutes.ADMIN.CONTACT_US,
      icon: 'bi bi-chat-left-text',
    },
  ];

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private accountService: AccountService,
    private router: Router,
    private companyListService: CompanyListService,
    private contactUsService: ContactUsService,
    private sqlReportService: SqlReportService
  ) {
    super(commonService, logService);
    this.viewModel = new AdminDashboardViewModel();
  }
  async ngOnInit() {
    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;
    await this.getloggedInUser();
    await this.loadDashboardSnapshot();
  }
/**
 * Get logged in user
 * @returns
 */
  async getloggedInUser() {
    try {
      const user = await this.accountService.getUserFromStorage();
      if (user != "") {
        this.viewModel.employee = user;
      }
    } catch (error) {
      throw error;
    }
    return;
  }

  async loadDashboardSnapshot() {
    try {
      await this._commonService.presentLoading();

      const [companyCountResp, contactResp, reportResp] = await Promise.all([
        this.companyListService.getAllCompaniesCount(),
        this.contactUsService.getAllcontactUsData(),
        this.sqlReportService.getAllSqlReportsForAdmin(),
      ]);

      this.stats[0].value = String(companyCountResp.isError ? 0 : companyCountResp.successData?.intResponse ?? 0);
      this.stats[1].value = String(contactResp.isError ? 0 : contactResp.successData?.length ?? 0);
      this.stats[2].value = String(reportResp.isError ? 0 : reportResp.successData?.length ?? 0);

      this.viewModel.recentActivities = [
        {
          title: 'Company Management',
          description: `${this.stats[0].value} companies currently available for review.`,
          date: new Date(),
        },
        {
          title: 'Inbox Monitoring',
          description: `${this.stats[1].value} contact requests are available in the admin inbox.`,
          date: new Date(),
        },
        {
          title: 'Reporting Center',
          description: `${this.stats[2].value} SQL reports are ready for administration.`,
          date: new Date(),
        },
      ];
    } catch (error) {
      await this._exceptionHandler.logObject(error);
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  openAdminRoute(route: string) {
    this.router.navigateByUrl(route);
  }
}
