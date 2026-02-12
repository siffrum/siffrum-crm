import { Injectable } from "@angular/core";
import { AppConstants } from "src/app-constants";
import { ClientEmployeeAttendanceSM } from "../service-models/app/v1/client/client-employee-attendance-s-m";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { StorageService } from "../services/storage.service";
import { BaseApiClient } from "./base-client/base-api.client";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { StorageCache } from "./helpers/storage-cache.helper";

@Injectable({
  providedIn: "root",
})
export class EmployeeAttendanceClient extends BaseApiClient {
  constructor(
    storageService: StorageService,
    storageCache: StorageCache,
    commonResponseCodeHandler: CommonResponseCodeHandler
  ) {
    super(storageService, storageCache, commonResponseCodeHandler);
  }
  /** Get all Employees
   * Needs to send Date to get the Statusof Employee So We need to use POST method.
   */
  GetEmployeeAttendance = async (
    date: Date
  ): Promise<ApiResponse<ClientEmployeeAttendanceSM[]>> => {
    let resp = await this.GetResponseAsync<Date, ClientEmployeeAttendanceSM[]>(
      `${
        AppConstants.ApiUrls.EMPLOYEE_ATTENDANCE_URL
      }/mine/EmployeeAttendanceDetail/${date.toISOString()}`,
      "POST"
    );
    return resp;
  };

  /**
   * add employe checkIn time using POST method
   * @param employeeCheckInTime
   * @returns
   */
  AddEmployeeCheckInTime = async (
    employeeCheckInTime: ApiRequest<ClientEmployeeAttendanceSM>
  ): Promise<ApiResponse<ClientEmployeeAttendanceSM>> => {
    let resp = await this.GetResponseAsync<
      ClientEmployeeAttendanceSM,
      ClientEmployeeAttendanceSM
    >(
      `${AppConstants.ApiUrls.EMPLOYEE_ATTENDANCE_URL}/mine/CheckIn`,
      "POST",
      employeeCheckInTime
    );
    return resp;
  };

  /**
   * Update employe checkOut time using PUT method
   * @param employeeCheckOutTime
   * @returns
   */
  updateEmployeeCheckOutTime = async (
    employeeCheckOutTime: ApiRequest<ClientEmployeeAttendanceSM>
  ): Promise<ApiResponse<ClientEmployeeAttendanceSM>> => {
    let resp = await this.GetResponseAsync<
      ClientEmployeeAttendanceSM,
      ClientEmployeeAttendanceSM
    >(
      `${AppConstants.ApiUrls.EMPLOYEE_ATTENDANCE_URL}/mine/CheckOut/${employeeCheckOutTime.reqData.id}`,
      "PUT",
      employeeCheckOutTime
    );
    return resp;
  };
}
