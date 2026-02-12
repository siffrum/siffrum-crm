import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../base.component';
import { CommonService } from 'src/app/services/common.service';
import { LogHandlerService } from 'src/app/services/log-handler.service';
import { SqlReportService } from 'src/app/services/sql-report.service';
import { DynamicSqlReportsViewModel } from 'src/app/view-models/dynamic-sql-reports.viewmodel';
import { DatePipe } from '@angular/common';
import * as XLSX from 'xlsx';
import { SQLReportCellSM } from 'src/app/service-models/app/v1/client/s-q-l-report-cell-s-m';
import { CellDataType } from 'src/app/service-models/app/enums/cell-data-type.enum';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-dynamic-sql-reports',
    templateUrl: './dynamic-sql-reports.component.html',
    styleUrls: ['./dynamic-sql-reports.component.scss'],
    standalone: false
})
export class DynamicSqlReportsComponent extends BaseComponent<DynamicSqlReportsViewModel> implements OnInit{

  constructor(commonService:CommonService,
    logService:LogHandlerService,
    private sqlReportService:SqlReportService,
    private datePipe: DatePipe,
    private route: ActivatedRoute
    ){
    super(commonService, logService);
  this.viewModel=new DynamicSqlReportsViewModel()
  }
  /**
   * @Dev Musaib
   */
async ngOnInit() {
  this.grtRouteIdeFromSideMenuReportItem()
await this._commonService.presentLoading();
  setTimeout(async () => {
    await this._commonService.dismissLoader();
  }, 1000);
}

grtRouteIdeFromSideMenuReportItem(){
 //GET Id From SideMenu Report Item.
 this.route.params.subscribe((params) => {
  if(params){
    this.viewModel.sqlReportObj.id = params['id']; // Use square brackets to access 'id'
    // Now you can use the id in this component;
    this.viewModel.PageTitle=params['selectedReportName']
    this.loadPagedataWithPagination(this.viewModel.pagination.PageNo);
    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;

  }
  else{
    this._commonService.showSweetAlertToast({
      title: 'Error!',
      text: 'Opps! Something Went Wrong',
      position: "top-end",
      icon: "error"
    });
  }
});
}
//next page
nextPage() {
    this.viewModel.pagination.PageNo++;
    this.loadPagedataWithPagination(this.viewModel.pagination.PageNo++)
}
//previous page
previousPage() {
    this.viewModel.pagination.PageNo--;
    this.loadPagedataWithPagination(this.viewModel.pagination.PageNo--)
}
/**
 * Load Page With PAGE No.
 * @param event
 */
async loadPagedataWithPagination(event: any) {

  this.viewModel.pagination.PageNo = event;
  await this.getlSqlReportBySelectedReportName(this.viewModel.pagination.PageNo)
  if(this.viewModel.pagination.PageNo==1){
    this.viewModel.showPreviuosBtn=false;
   }
   else{
     this.viewModel.showPreviuosBtn=true;
   }
}
/**
 * Get Sql Report By SelectedReport Name And Id
 */
async getlSqlReportBySelectedReportName(pageNo:number) {
  try {
    await this._commonService.presentLoading();
    let resp =
      await this.sqlReportService.getlSqlReportBySelectedReportName(this.viewModel.sqlReportObj.id,pageNo);
    if (resp.isError) {
      await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
    } else {
      this.viewModel.sqlReportDataModelObj = resp.successData;

    }
  } catch (error) {
    throw error;
  } finally {
    await this._commonService.dismissLoader();
  }
}
/**
 * Format Cell VAlue?
 * @param cell
 * @returns
 */
formatCellValue(cell: SQLReportCellSM) {
  if (!cell.cellValue) return "NA";
  var dataType: any = CellDataType[cell.cellDataType];
  switch (dataType) {
    case CellDataType.String:
      return cell.cellValue || "NA";
    case CellDataType.Boolean:
      return cell.cellValue ? "☑" : "☐";
    case CellDataType.Number:
      return cell.cellValue || "NA";
    case CellDataType.Date:
      return this.datePipe.transform(cell.cellValue.toString(), "yyyy-MM-dd");
    case CellDataType.Unknown:
    default:
      return cell.cellValue; // Handle other data types as needed
  }
}

/**
 * Download Sql Report as Excel File
 */

exportToExcel() {
  try {
    let headerRow = this.getHtmlTableHeader(); // Get table header data
    let dataRows = this.getHtmlTableData(); // Get table body data
    let ws: XLSX.WorkSheet = XLSX.utils.aoa_to_sheet([headerRow, ...dataRows]);
    // Style the header row (make it blue)
    let wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
    let fileName = `${this._commonService.layoutVM.PageTitle}.xlsx`;
    // Generate a blob containing the Excel file data
    XLSX.writeFile(wb, fileName);
  } catch (error) {
    console.error("Error exporting Excel:", error);
  }
}

/**
 * Get Sql Table Header
 * @returns
 */
getHtmlTableHeader() {
  let headerRow = this.viewModel.sqlReportDataModelObj.reportDataColumns.map(column => column.cellColumnName);
  return headerRow;
}
/**
 * Get Sql Report Data
 * @returns
 */
getHtmlTableData() {
  return this.viewModel.sqlReportDataModelObj.reportDataRows.map(row => row.reportDataCells.map(cell => this.formatCellValue(cell)));
}
}
