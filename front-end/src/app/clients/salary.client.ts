import { Injectable } from "@angular/core";
import { AppConstants } from "src/app-constants";
import { ClientEmployeeCTCDetailSM } from "../service-models/app/v1/client/client-employee-c-t-c-detail-s-m";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { QueryFilter } from "../service-models/foundation/api-contracts/query-filter";
import { IntResponseRoot } from "../service-models/foundation/common-response/int-response-root";
import { StorageService } from "../services/storage.service";
import { BaseApiClient } from "./base-client/base-api.client";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { StorageCache } from "./helpers/storage-cache.helper";
import { BoolResponseRoot } from "../service-models/foundation/common-response/bool-response-root";

@Injectable({
  providedIn: "root",
})
export class SalaryClient extends BaseApiClient {
  constructor(
    storageService: StorageService,
    storageCache: StorageCache,
    commonResponseCodeHandler: CommonResponseCodeHandler
  ) {
    super(storageService, storageCache, commonResponseCodeHandler);
  }
  /** Get Employee Salary By Employee Id oData */
  GetEmployeeSalaryInfoByoData = async (
    queryFilter: QueryFilter,
    employeeId: number
  ): Promise<ApiResponse<ClientEmployeeCTCDetailSM[]>> => {
    let finalUrl = await this.ApplyQueryFilterToUrl(
      `${AppConstants.ApiUrls.EMPLOYEE_SALARY_URL}/odata`,
      queryFilter
    );
    let resp = await this.GetResponseAsync<null, ClientEmployeeCTCDetailSM[]>(
      `${finalUrl}&&$filter=clientUserId eq ${employeeId}`,
      "GET"
    );
    return resp;
  };
  /**Get Employee Salary By Employee */

  GetEmployeeSalaryInfo = async (
    id: number
  ): Promise<ApiResponse<ClientEmployeeCTCDetailSM[]>> => {
    let resp = await this.GetResponseAsync<number, ClientEmployeeCTCDetailSM[]>(
      `${AppConstants.ApiUrls.EMPLOYEE_SALARY_URL}/ClientEmployeeCTCDetailByUserId/${id}`,
      "GET"
    );
    return resp;
  };
  /** Add Employee Salary */
  AddEmployeeSalary = async (
    employeeSalary: ApiRequest<ClientEmployeeCTCDetailSM>
  ): Promise<ApiResponse<ClientEmployeeCTCDetailSM>> => {
    let resp = await this.GetResponseAsync<
      ClientEmployeeCTCDetailSM,
      ClientEmployeeCTCDetailSM
    >(`${AppConstants.ApiUrls.EMPLOYEE_SALARY_URL}`, "POST", employeeSalary);
    return resp;
  };
  /**
   * Get Total Employee Address Count
   * @param empId
   * @returns
   */
  GetEmployeeSalaryCount = async (
    empId: number
  ): Promise<ApiResponse<IntResponseRoot>> => {
    let resp = await this.GetResponseAsync<null, IntResponseRoot>(
      `${AppConstants.ApiUrls.EMPLOYEE_SALARY_URL}/ClientEmployeeCtcDetailCountResponse/${empId}`,
      "GET"
    );
    return resp;
  };
}
