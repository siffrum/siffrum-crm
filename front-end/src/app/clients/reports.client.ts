import { Injectable } from "@angular/core";
import { AppConstants } from "src/app-constants";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { StorageService } from "../services/storage.service";
import { BaseApiClient } from "./base-client/base-api.client";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { StorageCache } from "./helpers/storage-cache.helper";
import { LeaveReportRequestSM } from "../service-models/app/v1/client/leave-report-request-s-m";
import { ClientEmployeeLeaveExtendedUserSM } from "../service-models/app/v1/client/client-employee-leave-extended-user-s-m";
import { PayrollTransactionReportSM } from "../service-models/app/v1/client/payroll-transaction-report-s-m";
import { GeneratePayrollTransactionSM } from "../service-models/app/v1/client/generate-payroll-transaction-s-m";
import { ClientUserSM } from "../service-models/app/v1/app-users/client-user-s-m";
import { IntResponseRoot } from "../service-models/foundation/common-response/int-response-root";
import { QueryFilter } from "../service-models/foundation/api-contracts/query-filter";

@Injectable({
  providedIn: "root",
})
export class ReportsClient extends BaseApiClient {
  constructor(
    storageService: StorageService,
    storageCache: StorageCache,
    commonResponseCodeHandler: CommonResponseCodeHandler
  ) {
    super(storageService, storageCache, commonResponseCodeHandler);
  }
  /** Leaves Report Based on Date Time (either monthly, Yearly or Custom) */
  GetLeavesReportByDate = async ( queryFilter: QueryFilter, leaveReportRequest: ApiRequest<LeaveReportRequestSM> ): Promise<ApiResponse<ClientEmployeeLeaveExtendedUserSM[]>> => {
    let resp = await this.GetResponseAsync< LeaveReportRequestSM, ClientEmployeeLeaveExtendedUserSM[]>(
      `${AppConstants.ApiUrls.EMPLOYEE_LEAVES_URL}/my/LeavesReport/${queryFilter.skip}/${queryFilter.top}`,
      "POST",
      leaveReportRequest
    );
    return resp;
  };
  /** Get Total Leaves Count of a Company */
  GetLeaveReportCountOfCompany = async (leaveReportRequest: ApiRequest<LeaveReportRequestSM> ): Promise<ApiResponse<IntResponseRoot>> => {

    let resp = await this.GetResponseAsync<LeaveReportRequestSM, IntResponseRoot>(
      `${AppConstants.ApiUrls.EMPLOYEE_LEAVES_URL}/LeaveReportCount`,
      "POST",
      leaveReportRequest
    );
    return resp;
  };
     /** Get Total Payroll Count of a Company */
     GetPayrollReportCountOfCompany = async (parrollReportRequest: ApiRequest<PayrollTransactionReportSM>): Promise<ApiResponse<IntResponseRoot>> => {
      let resp = await this.GetResponseAsync<PayrollTransactionReportSM, IntResponseRoot>(`${AppConstants.ApiUrls.PAYROLL_TRANSACTION_URL}/PayrollTransactionReportCount`, 'POST',parrollReportRequest);
      return resp;
  }
  /** Payroll Report Based on Date Time (either monthly, Yearly or Custom) */
  GetPayrollReportByDate = async (queryFilter:QueryFilter, payrollReportRequest: ApiRequest<PayrollTransactionReportSM> ): Promise<ApiResponse<GeneratePayrollTransactionSM[]>> => {
    let finalUrl = this.ApplyQueryFilterToUrl(`${AppConstants.ApiUrls.PAYROLL_TRANSACTION_URL}/my/AllPayrollTransactionReport?skip=${queryFilter.skip}&top=${queryFilter.top}`);
    let resp = await this.GetResponseAsync<  PayrollTransactionReportSM,  GeneratePayrollTransactionSM[] >( `${finalUrl}`, "POST", payrollReportRequest);
    return resp;
  };

  GetAllEmployeesOfTheCompany = async (): Promise<
    ApiResponse<ClientUserSM[]>
  > => {
    let resp = await this.GetResponseAsync<null, ClientUserSM[]>(
      `${AppConstants.ApiUrls.EMPLOYEE_URL}/my/AllEmployees`,
      "GET"
    );
    return resp;
  };
}
