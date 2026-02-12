import { Injectable } from "@angular/core";
import { BaseApiClient } from "./base-client/base-api.client";
import { StorageService } from "../services/storage.service";
import { StorageCache } from "./helpers/storage-cache.helper";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { ClientExcelFileRequestSM } from "../service-models/app/v1/client/client-excel-file-request-s-m";
import { ClientExcelFileResponseSM } from "../service-models/app/v1/client/client-excel-file-response-s-m";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { AppConstants } from "src/app-constants";
import { ClientExcelFileSummarySM } from "../service-models/app/v1/client/client-excel-file-summary-s-m";
import { ClientEmployeeAttendanceExtendedUserSM } from "../service-models/app/v1/client/client-employee-attendance-extended-user-s-m";

@Injectable({
    providedIn: "root",
  })
  export class AttendanceReportClient extends BaseApiClient {
    constructor(
      storageService: StorageService,
      storageCache: StorageCache,
      commonResponseCodeHandler: CommonResponseCodeHandler
    ) {
      super(storageService, storageCache, commonResponseCodeHandler);
    }
    /**
      * @DEV Musaib
     * @param excelFile
     * @returns
     */
    UploadAttendanceExcel = async (excelFile: ApiRequest<ClientExcelFileRequestSM>): Promise<ApiResponse<ClientExcelFileResponseSM>> => {
      let resp = await this.GetResponseAsync<ClientExcelFileRequestSM, ClientExcelFileResponseSM>(`${AppConstants.ApiUrls.EMPLOYEE_ATTENDANCE_URL}/AttendanceHeadingRow`, "POST", excelFile);
      return resp;
    };
/**
 * @DEV Musaib
 * @param headers
 * @returns 
 */
    saveSelectedHeaders= async (headers: ApiRequest<ClientExcelFileResponseSM>): Promise<ApiResponse<ClientExcelFileSummarySM>> => {
      let resp = await this.GetResponseAsync<ClientExcelFileResponseSM, ClientExcelFileSummarySM>(`${AppConstants.ApiUrls.EMPLOYEE_ATTENDANCE_URL}/AddEmployeeAttendanceDataFromExcel`, "POST", headers);
      return resp;
    };
/**
 * @Dev Musaib
 * @param summaryData
 * @returns 
 */
    addAttendanceSummaryData= async (summaryData: ApiRequest<ClientEmployeeAttendanceExtendedUserSM[]>): Promise<ApiResponse<ClientExcelFileSummarySM>> => {
      let resp = await this.GetResponseAsync<ClientEmployeeAttendanceExtendedUserSM[], ClientExcelFileSummarySM>(`${AppConstants.ApiUrls.EMPLOYEE_ATTENDANCE_URL}/AddAttendanceDataFromSummary`, "POST", summaryData);
      return resp;
    };
}