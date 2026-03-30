import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { EmployeeAttendanceClient } from '../clients/employee-attendance.client';
import { ClientEmployeeAttendanceSM } from '../service-models/app/v1/client/client-employee-attendance-s-m';
import { ApiRequest } from '../service-models/foundation/api-contracts/base/api-request';
import { ApiResponse } from '../service-models/foundation/api-contracts/base/api-response';

@Injectable({
  providedIn: 'root'
})
export class EmployeeAttendanceService extends BaseService {

  constructor(private employeeAttendanceClient: EmployeeAttendanceClient) {
    super();
  }

  /**
   * Get employee attendance for a specific date
   * @param date Date
   * @returns ApiResponse<ClientEmployeeAttendanceSM[]>
   */
  async getEmployeeAttendance(date: Date): Promise<ApiResponse<ClientEmployeeAttendanceSM[]>> {
    if (!date || !(date instanceof Date)) {
      throw new Error('Valid date is required to fetch employee attendance');
    }
    return await this.employeeAttendanceClient.GetEmployeeAttendance(date);
  }

  /**
   * Add employee check-in time
   * @param attendance ClientEmployeeAttendanceSM
   * @returns ApiResponse<ClientEmployeeAttendanceSM>
   */
  async addEmployeeCheckInTime(attendance: ClientEmployeeAttendanceSM): Promise<ApiResponse<ClientEmployeeAttendanceSM>> {
    if (!attendance) {
      throw new Error('Employee attendance data is required for check-in');
    }
    const apiRequest = new ApiRequest<ClientEmployeeAttendanceSM>();
    apiRequest.reqData = attendance;
    return await this.employeeAttendanceClient.AddEmployeeCheckInTime(apiRequest);
  }

  /**
   * Update employee check-out time and status
   * @param attendance ClientEmployeeAttendanceSM
   * @returns ApiResponse<ClientEmployeeAttendanceSM>
   */
  async updateEmployeeCheckOutTime(attendance: ClientEmployeeAttendanceSM): Promise<ApiResponse<ClientEmployeeAttendanceSM>> {
    if (!attendance || !attendance.id) {
      throw new Error('Employee attendance data with valid ID is required for check-out');
    }
    const apiRequest = new ApiRequest<ClientEmployeeAttendanceSM>();
    apiRequest.reqData = attendance;
    return await this.employeeAttendanceClient.updateEmployeeCheckOutTime(apiRequest);
  }

  /**
   * Get attendance report count
   * @param filters - Report filter data
   * @returns ApiResponse<number>
   */
  async getAttendanceReportCount(filters: any): Promise<ApiResponse<number>> {
    if (!filters) {
      throw new Error('Filter data is required to get report count');
    }
    const apiRequest = new ApiRequest<any>();
    apiRequest.reqData = filters;
    return await this.employeeAttendanceClient.getAttendanceReportCount(apiRequest);
  }

  /**
   * Get employee attendance report with pagination
   * @param skip - Skip records
   * @param top - Take records
   * @param filters - Report filter data
   * @returns ApiResponse<ClientEmployeeAttendanceSM[]>
   */
  async getEmployeeAttendanceReport(skip: number, top: number, filters: any): Promise<ApiResponse<ClientEmployeeAttendanceSM[]>> {
    if (skip < 0 || top <= 0) {
      throw new Error('Skip must be >= 0 and top must be > 0');
    }
    if (!filters) {
      throw new Error('Filter data is required for report');
    }
    const apiRequest = new ApiRequest<any>();
    apiRequest.reqData = filters;
    return await this.employeeAttendanceClient.getEmployeeAttendanceReport(skip, top, apiRequest);
  }

  /**
   * Get attendance report by user ID with pagination
   * @param userId - User ID
   * @param skip - Skip records
   * @param top - Take records
   * @param filters - Report filter data
   * @returns ApiResponse<ClientEmployeeAttendanceSM[]>
   */
  async getAttendanceReportByUserId(userId: number, skip: number, top: number, filters: any): Promise<ApiResponse<ClientEmployeeAttendanceSM[]>> {
    if (!userId || userId <= 0) {
      throw new Error('Valid user ID is required');
    }
    if (skip < 0 || top <= 0) {
      throw new Error('Skip must be >= 0 and top must be > 0');
    }
    if (!filters) {
      throw new Error('Filter data is required for report');
    }
    const apiRequest = new ApiRequest<any>();
    apiRequest.reqData = filters;
    return await this.employeeAttendanceClient.getAttendanceReportByUserId(userId, skip, top, apiRequest);
  }

  /**
   * Delete employee attendance record
   * @param attendanceId - Attendance record ID
   * @returns ApiResponse<any>
   */
  async deleteEmployeeAttendance(attendanceId: number): Promise<ApiResponse<any>> {
    if (!attendanceId || attendanceId <= 0) {
      throw new Error('Valid attendance ID is required to delete record');
    }
    return await this.employeeAttendanceClient.deleteEmployeeAttendance(attendanceId);
  }
}