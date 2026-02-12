import { Component, OnInit } from "@angular/core";
import { BaseComponent } from "../../base.component";
import { AttendanceReportViewModel } from "src/app/view-models/attendance-report.viewmodel";
import { CommonService } from "src/app/services/common.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { AttendanceReportService } from "src/app/services/attendance-report.service";
import { ClientExcelFileSummarySM } from "src/app/service-models/app/v1/client/client-excel-file-summary-s-m";

@Component({
    selector: "app-attendance-report",
    templateUrl: "./attendance-report.component.html",
    styleUrls: ["./attendance-report.component.scss"],
    standalone: false
})
export class AttendanceReportComponent
  extends BaseComponent<AttendanceReportViewModel>
  implements OnInit {
  /**@DEV Musaib */
  constructor(
    _commonService: CommonService,
    logService: LogHandlerService,
    private attendanceReportService: AttendanceReportService
  ) {
    super(_commonService, logService);
    this.viewModel = new AttendanceReportViewModel();
    this.viewModel.excelMappingFileRequest.excelFieldMapping = new Map<string, number>();
  }
  ngOnInit() {
    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;
  }
  // Upload Employee Document and Convert To Base64 String
  uploadDocument(event: any) {
    const fileInput = event.target as HTMLInputElement;

    const file = fileInput?.files?.[0];
    if (!file) {
      // No file selected
      return;
    }
    console.log(file);
    const allowedExtensions = ["xlsx", "xls"];
    const fileExtension = file.name.split(".").pop()?.toLowerCase() || "";
    if (!allowedExtensions.includes(fileExtension)) {
      // Unsupported file type, show custom error message or perform any desired action
      this._commonService.showSweetAlertToast({
        title: "Please select a .doc or .pdf file.",
        icon: "error",
      });
      // Clear the input to allow the user to select another file
      this.viewModel.excelUploadFileRequest.uploadFile = "";
      return;
    }
    this._commonService.convertFileToBase64(file).subscribe((base64) => {
      this.viewModel.extension = fileExtension;
      this.viewModel.excelUploadFileRequest.uploadFile = base64;
    });
  }
  /**
   * Upload Attendance excel File
   * Enter Header row
   * @returns
   */
  async uploadAttendanceExcelFile() {
    try {
      // Check if any required field is empty
      let uploadFile = !!this.viewModel.excelUploadFileRequest.uploadFile;
      let headerRowNumber = !!this.viewModel.excelUploadFileRequest.headerRow;
      if (!uploadFile || !headerRowNumber) {
        // Check if upload file  is empty
        await this._commonService.showSweetAlertToast({
          title: "Please Fill All Required Fields",
          icon: "error",
        });
        return; // Stop execution
      }
      await this._commonService.presentLoading();

      let resp = await this.attendanceReportService.uploadAttendanceExcelFile(
        this.viewModel.excelUploadFileRequest
      );
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.displayStyle = "none";
        this.viewModel.excelFileResponse = resp.successData;
        await this._commonService.ShowToastAtTopEnd(
          "Uploaded File Successfully",
          "success"
        );
        this.openCompanyAttendanceMappingModal();
        return;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  //Check if all required fields are Filled or Emplty
  isFormValid(): boolean {
    let employeeCode = !!this.viewModel.selectedEmployeeCode;
    let attendanceDate = !!this.viewModel.selectedAttendanceDate;
    let CheckIn = !!this.viewModel.selectedCheckIn;
    let CheckOut = !!this.viewModel.selectedCheckOut;
    let AttendanceStatus = !!this.viewModel.selectedAttendanceStatus;
    let EmployeeName = !!this.viewModel.selectedEmployeeName;
    return (employeeCode && attendanceDate && CheckIn && CheckOut && AttendanceStatus && EmployeeName);
  }

  //Separate method to set up the excelFieldMapping dictionary
  private setupExcelFieldMapping() {
    this.viewModel.excelMappingFileRequest.excelFieldMapping = new Map<string, number>();

    this.viewModel.excelMappingFileRequest.excelFieldMapping.set(
      this.viewModel.StaticlabelRequiredFields.EmployeeName,
      this.viewModel.selectedEmployeeName
    );
    this.viewModel.excelMappingFileRequest.excelFieldMapping.set(
      this.viewModel.StaticlabelRequiredFields.EmployeeCode,
      this.viewModel.selectedEmployeeCode
    );
    this.viewModel.excelMappingFileRequest.excelFieldMapping.set(
      this.viewModel.StaticlabelRequiredFields.AttendanceDate,
      this.viewModel.selectedAttendanceDate
    );
    this.viewModel.excelMappingFileRequest.excelFieldMapping.set(
      this.viewModel.StaticlabelRequiredFields.CheckIn,
      this.viewModel.selectedCheckIn
    );
    this.viewModel.excelMappingFileRequest.excelFieldMapping.set(
      this.viewModel.StaticlabelRequiredFields.CheckOut,
      this.viewModel.selectedCheckOut
    );
    this.viewModel.excelMappingFileRequest.excelFieldMapping.set(
      this.viewModel.StaticlabelRequiredFields.AttendanceStatus,
      this.viewModel.selectedAttendanceStatus
    );
  }
  /**
   * @Dev Musaib
   *save and map excel Attendance Headers With Required Labels Then save
   * @returns
   */
  async saveAttendanceData() {
    try {
      if (!this.isFormValid()) {
        await this._commonService.showSweetAlertToast({
          title: "Please Select All The Required Fields",
          icon: "error",
        });
        return;
      }

      await this._commonService.presentLoading();

      // Call the setupExcelFieldMapping method to set up the dictionary
      await this.setupExcelFieldMapping();
      // Define fieldMappingJSON as a dictionary with string keys and number values
      let fieldMappingJSON: { [key: string]: number } = {};
      this.viewModel.excelMappingFileRequest.excelFieldMapping.forEach((value, key) => {
        fieldMappingJSON[key] = value;
      });

      let dictionaryObj: any = fieldMappingJSON;

      this.viewModel.excelMappingFileRequest.excelFieldMapping = dictionaryObj;
      this.viewModel.excelMappingFileRequest.fileName = this.viewModel.excelFileResponse.fileName;
      this.viewModel.excelMappingFileRequest.headerRow = this.viewModel.excelUploadFileRequest.headerRow;

      let resp = await this.attendanceReportService.saveSelectedHeaders(
        this.viewModel.excelMappingFileRequest
      );

      if (resp.isError) {
        await this.handleErrorResponse(resp.errorData);
      } else {
        if (resp.successData.attendanceSummary.length == 0) {
          this.viewModel.showSummaryTable = false;
          this.viewModel.headRow = true;
          this._commonService.ShowToastAtTopEnd(
            "Data Saved Successfully !Error Data Not Found",
            "success"
          );
        } else {
          this.handleSuccessDataResponse(resp.successData);
        }
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }


  //Error Handler
  async handleErrorResponse(errorData: any) {
    await this._exceptionHandler.logObject(errorData);
    this._commonService.showSweetAlertToast({
      title: 'Error!',
      text: errorData.displayMessage,
      position: "top-end",
      icon: "error"
    });
  }
  //Handle success data of attendance report summary
  handleSuccessDataResponse(successData: ClientExcelFileSummarySM) {
    this.viewModel.attendanceReportSummary = {
      attendanceSummary: successData.attendanceSummary,
      fromDate: successData.fromDate,
      toDate: successData.toDate,
      totalRecordsCount: successData.totalRecordsCount,
      employeeRecordsAddedCount: successData.employeeRecordsAddedCount,
      numberOfRecordsNotAdded: successData.numberOfRecordsNotAdded,
    };
    this.viewModel.showSummaryTable = true;
    this.viewModel.headRow = false;
    this.viewModel.displayStyle = "none";
    this.viewModel.summaryTableTitle = true;
    this.viewModel.updatedSummaryTableTitle = false;
    //add css class
    this.viewModel.tableClass = "col-lg-9 col-md-8 col-sm-12 tableFixHead";
    this.viewModel.saveButton = true;
    this.viewModel.showCard = true;
    this._commonService.ShowToastAtTopEnd(
      "Summary Report of Uploaded Excel File",
      "success"
    );
  }
  /**
   * @Dev Musaib
   * Save Attendance summary Table and get updated Summary table data/
   * @returns Updated Attendance Summary Table
   */
  async saveAttendanceSummaryDataToReCheckErrors() {
    try {
      this._commonService.presentLoading()
      let resp = await this.attendanceReportService.addAttendanceSummaryData(this.viewModel.attendanceReportSummary.attendanceSummary)
      if (resp.isError) {
        await this.handleErrorResponse(resp.errorData);
      } else {
        this.viewModel.attendanceReportSummary.attendanceSummary = []
        let updatedSummaryResp = resp.successData
        if (updatedSummaryResp.attendanceSummary.length == 0) {
          this.viewModel.headRow = true;
          this.viewModel.showSummaryTable = false;
          await this._commonService.ShowToastAtTopEnd(
            "No Error Found",
            "success"
          );
        }
        else {
          this.viewModel.attendanceReportSummary.attendanceSummary = updatedSummaryResp.attendanceSummary;
          this.viewModel.summaryTableTitle = false;
          this.viewModel.updatedSummaryTableTitle = true;
          this.viewModel.tableClass = "col-lg-12 col-md-12 col-sm-12 tableFixHead"
          this.viewModel.headRow = false;
          this.viewModel.showCard = false;
          this.viewModel.saveButton = false;
          await this._commonService.ShowToastAtTopEnd(
            "Updated Summary File",
            "success"
          );
        }
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  // open Modal
  async openCompanyAttendanceMappingModal() {
    this.viewModel.displayStyle = "block";
  }
  //
  closePopup() {
    this.viewModel.displayStyle = "none";
  }
}
