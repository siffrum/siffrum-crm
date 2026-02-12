import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../base.component';
import { CommonService } from 'src/app/services/common.service';
import { LogHandlerService } from 'src/app/services/log-handler.service';
import { ReprtsService } from 'src/app/services/reports.service';
import { DateFilterTypeSM } from 'src/app/service-models/app/enums/date-filter-type-s-m.enum';
import { PayrollTransactionReportViewmodel } from 'src/app/view-models/payroll-reports.viewmodel';


@Component({
    selector: 'app-payroll-reports',
    templateUrl: './payroll-reports.component.html',
    styleUrls: ['./payroll-reports.component.scss'],
    standalone: false
})
export class PayrollReportsComponent extends BaseComponent<PayrollTransactionReportViewmodel> implements OnInit {

  constructor(
    _commonService: CommonService,
    logService: LogHandlerService,
    private reportsService:ReprtsService,
  ) {
    super(_commonService, logService);
    this.viewModel = new PayrollTransactionReportViewmodel();
  }


  async ngOnInit() {
    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;
    this.viewModel = await this.InitializePayrollTransactionReportsPage();
    await this.getMonths();
    this.viewModel.selectedMonth="";
    this.getEmployeesOfTheCompany();
    await this._commonService.presentLoading();
    await setTimeout(async () => {
      await this._commonService.dismissLoader();
    },1000);
  }
  async getPayrollReportCountOfCompany() {
    try {
      await this._commonService.presentLoading();
      let resp = await this.reportsService.getPayrollReportCountOfCompany(this.viewModel.PayrollTransactionReportRequest);
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
    } finally {
      await this._commonService.dismissLoader();
    }
  }
/**
 * Download Excel File
 */
// exportToExcel() {
//   const data = this.viewModel.PayrollTransactionReportList; // Replace with your array of objects
//   const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(data);
//   const wb: XLSX.WorkBook = XLSX.utils.book_new();
//   XLSX.utils.book_append_sheet(wb, ws, 'Sheet1'); // Change the sheet name as needed
//   // Generate a blob containing the Excel file data
//   XLSX.writeFile(wb, 'exported_data.xlsx'); // Provide the desired file name
// }
/**
 * get Payroll Transaction  reports
 */
  async getPayrollTransactionReport() {
    try {
      await this._commonService.presentLoading();
      let x: DateFilterTypeSM = parseInt(DateFilterTypeSM[this.viewModel.PayrollTransactionReportRequest.dateFilterType]);
      switch (x) {
        case DateFilterTypeSM.Monthly:
          this.viewModel.PayrollTransactionReportRequest.dateFrom = await this._commonService.getISODateFromMonthYear(this.viewModel.selectedMonth);
          break;

        case DateFilterTypeSM.Yearly:
          this.viewModel.PayrollTransactionReportRequest.dateFrom = await this._commonService.getISODateFromYear(this.viewModel.selectedYear);
          break;

        default:
          break;
      }
      let resp = await this.reportsService.getPayrollReport(this.viewModel,this.viewModel.PayrollTransactionReportRequest);
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        await this._commonService.ShowToastAtTopEnd(resp.errorData.displayMessage, 'error');
      } else {
        this.viewModel.showTable = true;
        this.viewModel.PayrollTransactionReportList = resp.successData;
        this.getPayrollReportCountOfCompany()
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }

  }
  async loadPagedataWithPagination(pageNumber: number) {
    if (pageNumber && pageNumber > 0) {
      // this.viewModel.PageNo = pageNumber;
      this.viewModel.pagination.PageNo = pageNumber;
      await this.getPayrollTransactionReport();
    }
  }
  // async loadPagedataWithPagination(event: any) {
  //   this.viewModel.PageNo = event;
  //   await this.getPayrollTransactionReport();
  // }
  async InitializePayrollTransactionReportsPage(): Promise<PayrollTransactionReportViewmodel> {
    var viewModel = new PayrollTransactionReportViewmodel();
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
    let x: any = DateFilterTypeSM[this.viewModel.PayrollTransactionReportRequest.dateFilterType];
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

async getEmployeesOfTheCompany(){
  try {
    await this._commonService.presentLoading();
      let resp = await this.reportsService.getAllEmployeeOfCompany();
    if (resp.isError) {
      await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
    } else {
      this.viewModel.ClientCompanyEmployeeList = resp.successData;
         // Extract login IDs from successData
      // Extract login IDs and their corresponding IDs from successData
      this.viewModel.loginIds = resp.successData.map((employee) => employee.loginId).filter(Boolean);
;
    }

  } catch (error) {
    throw error;
  } finally {
    await this._commonService.dismissLoader();
  }
}
onLoginIdChange( ) {
  const selectedLoginId = this.viewModel.SelectedEmployeeOfTheCompany.loginId;
 this.viewModel.ClientCompanyEmployeeList.find(employee => employee.loginId === selectedLoginId);
}
}
