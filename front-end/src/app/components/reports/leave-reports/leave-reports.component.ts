import { Component, OnInit } from '@angular/core';
import { DateFilterTypeSM } from 'src/app/service-models/app/enums/date-filter-type-s-m.enum';
import { CommonService } from 'src/app/services/common.service';
import { LogHandlerService } from 'src/app/services/log-handler.service';
import { LeavesReportViewmodel } from 'src/app/view-models/leaves-report.viewmodel';
import { BaseComponent } from '../../base.component';
import { ReprtsService } from 'src/app/services/reports.service';


@Component({
    selector: 'app-leave-reports',
    templateUrl: './leave-reports.component.html',
    styleUrls: ['./leave-reports.component.scss'],
    standalone: false
})


export class LeaveReportsComponent extends BaseComponent<LeavesReportViewmodel> implements OnInit {

  constructor(
    _commonService: CommonService,
    logService: LogHandlerService,
    private reportsService:ReprtsService,
  ) {
    super(_commonService, logService);
    this.viewModel = new LeavesReportViewmodel();
  }


  async ngOnInit() {
    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;
    this.viewModel = await this.InitializeLeaveReportsPage();
    await this.getMonths();
    this.viewModel.selectedMonth="";
    await this._commonService.presentLoading();
    await setTimeout(async () => {
      await this._commonService.dismissLoader();
    },1000);
  }
/**
 * get leave reports Of The  Company
   *@Dev Musaib
 *
 */

  async getLeavesReport() {
    try {
      await this._commonService.presentLoading();
      let x: DateFilterTypeSM = parseInt(DateFilterTypeSM[this.viewModel.leaveReportRequest.dateFilterType]);
      switch (x) {
        case DateFilterTypeSM.Monthly:
          this.viewModel.leaveReportRequest.dateFrom = await this._commonService.getISODateFromMonthYear(this.viewModel.selectedMonth);
          break;

        case DateFilterTypeSM.Yearly:
          this.viewModel.leaveReportRequest.dateFrom = await this._commonService.getISODateFromYear(this.viewModel.selectedYear);
          break;

        default:
          break;
      }
      let resp = await this.reportsService.getLeavesReport(this.viewModel.leaveReportRequest,this.viewModel);
      this.getLeavesReportCountOfCompany()
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        await this._commonService.ShowToastAtTopEnd(resp.errorData.displayMessage, 'error');
      } else {
        this.viewModel.showTable = true;
        this.viewModel.leavesReportList = resp.successData;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }

  }
    /**
   * Get Leaves Report  Count Of The company
   */
    async getLeavesReportCountOfCompany() {
      try {
        await this._commonService.presentLoading();
        let resp = await this.reportsService.getLeavesReportCountOfCompany(this.viewModel.leaveReportRequest);
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
        throw error;
      } finally {
        await this._commonService.dismissLoader();
      }
    }
    /**this function is used to create an event for pagination */
    async loadPagedataWithPagination(pageNumber: number) {
      if (pageNumber && pageNumber > 0) {
        // this.viewModel.PageNo = pageNumber;
        this.viewModel.pagination.PageNo = pageNumber;
        await this.getLeavesReport();
      }
    }
    // async loadPagedataWithPagination(event: any) {
    //   this.viewModel.PageNo = event;
    //   await this.getLeavesReport()
    // }
    //
  async InitializeLeaveReportsPage(): Promise<LeavesReportViewmodel> {
    var viewModel = new LeavesReportViewmodel();
    viewModel.dateFilterTypeList = await this._commonService.EnumToStringArray(DateFilterTypeSM);
    return viewModel;
  }
  async getMonths() {
    var date = new Date();
    var months = [],
      monthNames = [
        "January",
        "February",
        "March",
        "April",
        "May",
        "June",
        "July",
        "August",
        "September",
        "October",
        "November",
        "December",
      ];
    for (var i = 0; i < 12; i++) {
      months.push(monthNames[date.getMonth()] + ' ' + date.getFullYear());
      date.setMonth(date.getMonth() - 1);
    }
    this.viewModel.months = months;
  }
  async getYears() {
    this.viewModel.selectedYear = new Date().getFullYear();
    for (let year = this.viewModel.selectedYear; year >= 2010; year--) {
      this.viewModel.years.push(year);
    }
  }


  async selectRequest() {
    let x: any = DateFilterTypeSM[this.viewModel.leaveReportRequest.dateFilterType];
    switch (x) {
      case DateFilterTypeSM.Monthly:
        this.viewModel.showMonthlyDropdown = true;
        this.viewModel.showYearlyDropdown = false;
        this.viewModel.showCustomCalendar = false;
        await this.getMonths()
        break;

      case DateFilterTypeSM.Yearly:
        this.viewModel.showYearlyDropdown = true;
        this.viewModel.showMonthlyDropdown = false;
        this.viewModel.showCustomCalendar = false;
        await this.getYears();
        break;

      case DateFilterTypeSM.Custom:
        this.viewModel.showYearlyDropdown = false;
        this.viewModel.showMonthlyDropdown = false;
        this.viewModel.showCustomCalendar = true;
        break;
    }
  }

}
