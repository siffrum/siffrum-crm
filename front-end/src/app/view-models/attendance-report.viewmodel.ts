import { AttendanceItemFields } from "../service-models/app/constants/attendance-item-fields";
import { ClientExcelFileRequestSM } from "../service-models/app/v1/client/client-excel-file-request-s-m";
import { ClientExcelFileResponseSM } from "../service-models/app/v1/client/client-excel-file-response-s-m";
import { ClientExcelFileSummarySM } from "../service-models/app/v1/client/client-excel-file-summary-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class AttendanceReportViewModel extends BaseViewModel {
  override PageTitle: string = "Attendance Report";
  reportFilePath: string = "";
  extension!: string;
  headerRowNumber!: number;
  excelUploadFileRequest: ClientExcelFileRequestSM = new ClientExcelFileRequestSM();
  excelFileResponse: ClientExcelFileResponseSM = new ClientExcelFileResponseSM();
  excelMappingFileRequest: ClientExcelFileResponseSM = new ClientExcelFileResponseSM();
  StaticlabelRequiredFields = AttendanceItemFields;
  attendanceReportSummary: ClientExcelFileSummarySM = new ClientExcelFileSummarySM()
  displayStyle = "none";
  tableClass: string = " ";
  showCard: boolean = false;
  saveButton: boolean = false;
  summaryTableTitle: boolean = false;
  updatedSummaryTableTitle: boolean = false;
  selectedEmployeeName!: number;
  selectedEmployeeCode!: number;
  selectedAttendanceDate!: number;
  selectedCheckIn!: number;
  selectedCheckOut!: number;
  selectedAttendanceStatus!: number;
  showSummaryTable: boolean = false;
  headRow: boolean = true;
  validations = {
    attendanceFilePath: [
      { type: "required", message: "Document path Requierd" },
    ],
    headerRowNumber: [
      { type: "required", message: "Heading Row  Number Requierd" },
      { type: "minlength", message: "Minimum Length is 1 Characters" },
    ],
  }
}

