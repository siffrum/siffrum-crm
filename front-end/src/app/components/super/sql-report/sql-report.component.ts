import { Component, OnInit } from "@angular/core";
import { BaseComponent } from "../../base.component";
import { SqlReportViewModel } from "src/app/view-models/sql-report.viewmodel";
import { CommonService } from "src/app/services/common.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { SqlReportService } from "src/app/services/sql-report.service";
import { NgForm } from "@angular/forms";
import { SQLReportMasterSM } from "src/app/service-models/app/v1/client/s-q-l-report-master-s-m";
import { AppConstants } from "src/app-constants";
import { CellDataType } from "src/app/service-models/app/enums/cell-data-type.enum";
import { DatePipe } from "@angular/common";
import { SQLReportCellSM } from "src/app/service-models/app/v1/client/s-q-l-report-cell-s-m";

@Component({
    selector: "app-sql-report",
    templateUrl: "./sql-report.component.html",
    styleUrls: ["./sql-report.component.scss"],
    standalone: false
})
export class SqlReportComponent
  extends BaseComponent<SqlReportViewModel>
  implements OnInit
{
  /**@Dev Musaib */
  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private sqlReportService: SqlReportService,
    private datePipe: DatePipe
  ) {
    super(commonService, logService);
    this.viewModel = new SqlReportViewModel();
  }

  async ngOnInit() {
    await this.loadPageData();
  this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;

  }
  /**
   * This method initializes the SqlReportComponent and
   *  loads page data by calling the 'getAllSqlReportList' method from the 'SqlReportService'.
   *  If an error occurs, it logs the error and displays a toast message.
   * If successful, it populates the 'sqlReportMasterList' in the view model.
   */
  override async loadPageData() {
    try {
      await this._commonService.presentLoading();
      let resp = await this.sqlReportService.getAllSqlReportList();
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.sqlReportMasterList = resp.successData;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * This method is responsible for opening the SQL query modal.
   *  It determines whether to edit, add, or preview a query based on the 'id' and 'queryString' parameters.
   *  It updates the view model accordingly.
   * @param id
   * @param queryString
   */
  openSqlQueryModal(id: number, queryString: String) {
    this.viewModel.sqlReportMaster = new SQLReportMasterSM();
    if (id > 0 && queryString == "") {
      this.viewModel.editMode = true;
      this.viewModel.addMode = false;
      this.viewModel.preViewTable = false;
      this.viewModel.form = true;
      this.getSqlQueryReportDetailsById(id);
    } else if (id == 0 && queryString == "") {
      this.viewModel.addMode = true;
      this.viewModel.editMode = false;
      this.viewModel.preViewTable = false;
      this.viewModel.form = true;
    } else if (id == 0 && queryString != "") {
      this.getSqlQueryReportDetailsByQuery(queryString);
      this.viewModel.preViewTable = true;
      this.viewModel.form = false;
    }
    this.viewModel.displayStyle = "block";
  }
  /**
   * This method closes the SQL query popup and resets the form if provided.
   *  It sets 'formSubmitted' to false.
   * @param sqlQueryForm
   */
  closePopup(sqlQueryForm: NgForm) {
    this.viewModel.displayStyle = "none";
    this.viewModel.formSubmitted = false; // Clear the formSubmitted flag
    if (sqlQueryForm) {
      sqlQueryForm.reset(); // Reset the form
    }
  }
  closeTablePopUp() {
    this.viewModel.displayStyle = "none";
  }
  /**
   * This method adds a new SQL query using the 'addSqlQuery' method from the 'SqlReportService'.
   *  It handles form validation, displays loading, logs errors,
   * and shows toast messages. If successful, it hides the popup and reloads the page data.
   * @param sqlQueryForm
   * @returns
   */
  async addSqlQuery(sqlQueryForm: NgForm) {
    this.viewModel.formSubmitted = true;
    try {
      if (sqlQueryForm.invalid) {
        await this._commonService.showSweetAlertToast({
          title: "Form Fields Are Not valid !",
          icon: "error",
        });
        return;
      }
      await this._commonService.presentLoading();
      let resp = await this.sqlReportService.addSqlQuery(
        this.viewModel.sqlReportMaster
      );
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this._commonService.ShowToastAtTopEnd(
          "Query Added Successfully",
          "success"
        );
        this.viewModel.displayStyle = "none";
        await this.loadPageData();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  /**
   * This method retrieves SQL query details by ID using the 'getQueryDetailsById' method from the 'SqlReportService'.
   *  It displays loading, logs errors, and shows toast messages.
   *  If successful, it updates the 'sqlReportMaster' in the view model.
   * @param id
   */
  async getSqlQueryReportDetailsById(id: number) {
    try {
      await this._commonService.presentLoading();
      let resp = await this.sqlReportService.getQueryDetailsById(id);
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.sqlReportMaster = resp.successData;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * This method updates an existing SQL query using the 'updateSqlQueryById' method from the 'SqlReportService'.
   *  It handles form validation, displays loading, logs errors, and shows toast messages.
   *  If successful, it hides the popup, reloads the page data, and updates the view model.
   * @param sqlQueryForm
   * @returns
   */
  async updateSqlQuery(sqlQueryForm: NgForm) {
    {
      this.viewModel.formSubmitted = true;
      try {
        if (sqlQueryForm.invalid) {
          await this._commonService.showSweetAlertToast({
            title: "Form Fields Are Not valid !",
            icon: "error",
          });
          return;
        }
        await this._commonService.presentLoading();
        let resp = await this.sqlReportService.updateSqlQueryById(
          this.viewModel.sqlReportMaster
        );
        if (resp.isError) {
          await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
        } else {
          this._commonService.ShowToastAtTopEnd(
            "Sql Query Updated Successfully",
            "success"
          );
          await this.loadPageData();
          this.viewModel.displayStyle = "none";
        }
      } catch (error) {
        throw error;
      } finally {
        await this._commonService.dismissLoader();
      }
    }
  }
  /**
   * This method deletes an SQL query by ID using the 'deleteSqlQueryById' method from the 'SqlReportService'.
   *  It shows a confirmation alert and handles loading, error logging, and toast messages.
   * If confirmed, it reloads the page data.
   * @param sqlQueryId
   */
  async deleteSqlQueryById(sqlQueryId: number) {
    let DeleteSqlQueryConfirmation =
      await this._commonService.showConfirmationAlert(
        AppConstants.DefaultMessages.DeleteSqlQuery,
        " ",
        true,
        "warning"
      );
    if (DeleteSqlQueryConfirmation) {
      try {
        await this._commonService.presentLoading();
        let resp = await this.sqlReportService.deleteSqlQueryById(sqlQueryId);
        if (resp.isError) {
          await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
        } else {
          this._commonService.ShowToastAtTopEnd(
            resp.successData.deleteMessage,
            "success"
          );
          await this.loadPageData();
        }
      } catch (error) {
      } finally {
        await this._commonService.dismissLoader();
      }
    }
  }

  /**
   * Preview Selected Query Data
   * This method retrieves SQL query details by query string using the 'getQueryPreviewByQuerystring' method from the 'SqlReportService'.
   *  It displays loading, logs errors, and shows toast messages.
   *  If successful, it updates the 'sqlReportDataModel' in the view model.
   * @param queryString
   */
  async getSqlQueryReportDetailsByQuery(queryString: String) {
    try {
      await this._commonService.presentLoading();
      let resp = await this.sqlReportService.getQueryPreviewByQuerystring(
        queryString
      );
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.sqlReportDataModel = resp.successData;
        console.log(this.viewModel.sqlReportDataModel);
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

    getProperty(row: any, columnName: string): any {
    const key = Object.keys(row).find(
      (k) => k.toLowerCase() === columnName.toLowerCase()
    );
    if (!key) return 'NA';

    const value = row[key];
    // Check for empty string, null, or undefined
    return value === null || value === undefined || value === '' ? 'NA' : value;
  }
  formatDate(value: string | number | Date | boolean): string {
    if (value instanceof Date) {
      return value.toLocaleDateString(); // You can customize the date format here
    } else if (typeof value === "string" || typeof value === "number") {
      const parsedDate = new Date(value);
      if (!isNaN(parsedDate.getTime())) {
        return parsedDate.toLocaleDateString(); // You can customize the date format here
      }
    }
    return value ? "True" : "False";
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
}
