import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { AccountsClient } from '../clients/accounts.client';
import { StorageService } from './storage.service';
import { Router } from '@angular/router';
import { CommonService } from './common.service';
import { AttendanceReportClient } from '../clients/attendance-report.client';
import { ClientExcelFileRequestSM } from '../service-models/app/v1/client/client-excel-file-request-s-m';
import { ApiRequest } from '../service-models/foundation/api-contracts/base/api-request';
import { ClientExcelFileResponseSM } from '../service-models/app/v1/client/client-excel-file-response-s-m';
import { ApiResponse } from '../service-models/foundation/api-contracts/base/api-response';
import { ClientExcelFileSummarySM } from '../service-models/app/v1/client/client-excel-file-summary-s-m';
import { ClientEmployeeAttendanceExtendedUserSM } from '../service-models/app/v1/client/client-employee-attendance-extended-user-s-m';

@Injectable({
  providedIn: 'root'
})
export class AttendanceReportService extends BaseService {
  constructor( private attendanceReportClient:AttendanceReportClient) {
    super();
  }
  /**
   * @DEV Musaib
   * Upload Excel File and Get Excel File Headers in Response
   * @param excelFile
   * @returns 
   */
  async uploadAttendanceExcelFile(excelFile:ClientExcelFileRequestSM): Promise<ApiResponse<ClientExcelFileResponseSM>> {
    let apiRequest = new ApiRequest<ClientExcelFileRequestSM>();
    apiRequest.reqData =excelFile;
    return await this.attendanceReportClient.UploadAttendanceExcel(apiRequest);
  }
  /**
   *  @DEV Musaib
   * Save selected Mapped Headers From Excel file
   * @param headers
   * @returns 
   */

  async saveSelectedHeaders(headers:ClientExcelFileResponseSM): Promise<ApiResponse<ClientExcelFileSummarySM>> {
    let apiRequest = new ApiRequest<ClientExcelFileResponseSM>();
    apiRequest.reqData =headers;
    return await this.attendanceReportClient.saveSelectedHeaders(apiRequest);
  }
  /**
    @DEV Musaib
  * Save Attendance summary Table and get updated Summary table data/
   * @param summaryData
   * @returns 
   */
  async addAttendanceSummaryData(summaryData:ClientEmployeeAttendanceExtendedUserSM[]): Promise<ApiResponse<ClientExcelFileSummarySM>> {
    let apiRequest = new ApiRequest<ClientEmployeeAttendanceExtendedUserSM[]>();
    apiRequest.reqData =summaryData;
    return await this.attendanceReportClient.addAttendanceSummaryData(apiRequest);
  }
}
