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
   * Needs to send Date to get the Status of Employee So We need to use POST method.
   * Tries /mine/EmployeeAttendanceDetail first, falls back to main GET endpoint
   */
  GetEmployeeAttendance = async (
    date: Date
  ): Promise<ApiResponse<ClientEmployeeAttendanceSM[]>> => {
    if (!date || !(date instanceof Date)) {
      throw new Error('Invalid date parameter provided to GetEmployeeAttendance');
    }
    try {
      // Try /mine/EmployeeAttendanceDetail endpoint (partial)
      let resp = await this.GetResponseAsync<Date, ClientEmployeeAttendanceSM[]>(
        `${
          AppConstants.ApiUrls.EMPLOYEE_ATTENDANCE_URL
        }/mine/EmployeeAttendanceDetail/${date.toISOString()}`,
        "POST"
      );
      return resp;
    } catch (mineError) {
      console.warn('Partial endpoint /mine/EmployeeAttendanceDetail failed, trying fallback GET:', mineError);
      try {
        // Fallback to main GET endpoint (done/stable)
        let resp = await this.GetResponseAsync<null, ClientEmployeeAttendanceSM[]>(
          `${AppConstants.ApiUrls.EMPLOYEE_ATTENDANCE_URL}`,
          "GET"
        );
        console.log('Attendance fetch succeeded using fallback GET endpoint');
        return resp;
      } catch (fallbackError) {
        console.error('Both attendance fetch endpoints failed:', fallbackError);
        throw fallbackError;
      }
    }
  };

  /**
   * add employe checkIn time using POST method
   * Tries /mine/CheckIn first, falls back to /CheckIn if partial endpoint fails
   * @param employeeCheckInTime
   * @returns
   */
  AddEmployeeCheckInTime = async (
    employeeCheckInTime: ApiRequest<ClientEmployeeAttendanceSM>
  ): Promise<ApiResponse<ClientEmployeeAttendanceSM>> => {
    if (!employeeCheckInTime || !employeeCheckInTime.reqData) {
      throw new Error('Invalid check-in data provided to AddEmployeeCheckInTime');
    }
    try {
      // Try /mine/CheckIn endpoint (partial)
      let resp = await this.GetResponseAsync<
        ClientEmployeeAttendanceSM,
        ClientEmployeeAttendanceSM
      >(
        `${AppConstants.ApiUrls.EMPLOYEE_ATTENDANCE_URL}/mine/CheckIn`,
        "POST",
        employeeCheckInTime
      );
      return resp;
    } catch (mineError) {
      console.warn('Partial endpoint /mine/CheckIn failed, trying fallback /CheckIn:', mineError);
      try {
        // Fallback to /CheckIn endpoint (done/stable)
        let resp = await this.GetResponseAsync<
          ClientEmployeeAttendanceSM,
          ClientEmployeeAttendanceSM
        >(
          `${AppConstants.ApiUrls.EMPLOYEE_ATTENDANCE_URL}/CheckIn`,
          "POST",
          employeeCheckInTime
        );
        console.log('Check-in succeeded using fallback endpoint');
        return resp;
      } catch (fallbackError) {
        console.error('Both check-in endpoints failed:', fallbackError);
        throw fallbackError;
      }
    }
  };

  /**
   * Update employe checkOut time using PUT method
   * Tries /mine/CheckOut first, falls back to /CheckOut if partial endpoint fails
   * @param employeeCheckOutTime
   * @returns
   */
  updateEmployeeCheckOutTime = async (
    employeeCheckOutTime: ApiRequest<ClientEmployeeAttendanceSM>
  ): Promise<ApiResponse<ClientEmployeeAttendanceSM>> => {
    if (!employeeCheckOutTime || !employeeCheckOutTime.reqData || !employeeCheckOutTime.reqData.id) {
      throw new Error('Invalid check-out data provided: ID is required for updateEmployeeCheckOutTime');
    }
    try {
      // Try /mine/CheckOut endpoint (partial)
      let resp = await this.GetResponseAsync<
        ClientEmployeeAttendanceSM,
        ClientEmployeeAttendanceSM
      >(
        `${AppConstants.ApiUrls.EMPLOYEE_ATTENDANCE_URL}/mine/CheckOut/${employeeCheckOutTime.reqData.id}`,
        "PUT",
        employeeCheckOutTime
      );
      return resp;
    } catch (mineError) {
      console.warn('Partial endpoint /mine/CheckOut failed, trying fallback /CheckOut:', mineError);
      try {
        // Fallback to /CheckOut endpoint (done/stable)
        let resp = await this.GetResponseAsync<
          ClientEmployeeAttendanceSM,
          ClientEmployeeAttendanceSM
        >(
          `${AppConstants.ApiUrls.EMPLOYEE_ATTENDANCE_URL}/CheckOut/${employeeCheckOutTime.reqData.id}`,
          "PUT",
          employeeCheckOutTime
        );
        console.log('Check-out succeeded using fallback endpoint');
        return resp;
      } catch (fallbackError) {
        console.error('Both check-out endpoints failed:', fallbackError);
        throw fallbackError;
      }
    }
  };

  /**
   * Get attendance report count
   * @param reportRequest - Report request with filters
   * @returns ApiResponse<number>
   */
  getAttendanceReportCount = async (
    reportRequest: ApiRequest<any>
  ): Promise<ApiResponse<number>> => {
    if (!reportRequest || !reportRequest.reqData) {
      throw new Error('Report request data is required');
    }
    try {
      let resp = await this.GetResponseAsync<any, number>(
        `${AppConstants.ApiUrls.EMPLOYEE_ATTENDANCE_URL}/AttendanceReportCount`,
        "POST",
        reportRequest
      );
      return resp;
    } catch (error) {
      console.error('Error fetching attendance report count:', error);
      throw error;
    }
  };

  /**
   * Get employee attendance report with pagination
   * @param skip - Skip records
   * @param top - Take records
   * @param reportRequest - Report request with filters
   * @returns ApiResponse<ClientEmployeeAttendanceSM[]>
   */
  getEmployeeAttendanceReport = async (
    skip: number,
    top: number,
    reportRequest: ApiRequest<any>
  ): Promise<ApiResponse<ClientEmployeeAttendanceSM[]>> => {
    if (skip < 0 || top <= 0) {
      throw new Error('Skip must be >= 0 and top must be > 0');
    }
    if (!reportRequest || !reportRequest.reqData) {
      throw new Error('Report request data is required');
    }
    try {
      let resp = await this.GetResponseAsync<any, ClientEmployeeAttendanceSM[]>(
        `${AppConstants.ApiUrls.EMPLOYEE_ATTENDANCE_URL}/my/EmployeeAttendanceReport/${skip}/${top}`,
        "POST",
        reportRequest
      );
      return resp;
    } catch (error) {
      console.error('Error fetching employee attendance report:', error);
      throw error;
    }
  };

  /**
   * Get attendance report by client user ID with pagination
   * @param userId - Client user ID
   * @param skip - Skip records
   * @param top - Take records
   * @param reportRequest - Report request with filters
   * @returns ApiResponse<ClientEmployeeAttendanceSM[]>
   */
  getAttendanceReportByUserId = async (
    userId: number,
    skip: number,
    top: number,
    reportRequest: ApiRequest<any>
  ): Promise<ApiResponse<ClientEmployeeAttendanceSM[]>> => {
    if (!userId || userId <= 0) {
      throw new Error('Valid user ID is required');
    }
    if (skip < 0 || top <= 0) {
      throw new Error('Skip must be >= 0 and top must be > 0');
    }
    if (!reportRequest || !reportRequest.reqData) {
      throw new Error('Report request data is required');
    }
    try {
      let resp = await this.GetResponseAsync<any, ClientEmployeeAttendanceSM[]>(
        `${AppConstants.ApiUrls.EMPLOYEE_ATTENDANCE_URL}/my/EmployeeAttendanceReportByClientUserId/${userId}/${skip}/${top}`,
        "POST",
        reportRequest
      );
      return resp;
    } catch (error) {
      console.error('Error fetching attendance report by user ID:', error);
      throw error;
    }
  };

  /**
   * Delete employee attendance record
   * @param attendanceId - Attendance record ID
   * @returns ApiResponse<any>
   */
  deleteEmployeeAttendance = async (
    attendanceId: number
  ): Promise<ApiResponse<any>> => {
    if (!attendanceId || attendanceId <= 0) {
      throw new Error('Valid attendance ID is required to delete record');
    }
    try {
      let resp = await this.GetResponseAsync<number, any>(
        `${AppConstants.ApiUrls.EMPLOYEE_ATTENDANCE_URL}/my/EmployeeAttendance/${attendanceId}`,
        "DELETE"
      );
      return resp;
    } catch (error) {
      console.error('Error deleting employee attendance:', error);
      throw error;
    }
  };
}
