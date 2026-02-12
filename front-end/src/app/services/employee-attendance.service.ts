import { Injectable } from '@angular/core';
import { EmployeeAttendanceClient } from '../clients/employee-attendance.client';
import { ClientEmployeeAttendanceSM } from '../service-models/app/v1/client/client-employee-attendance-s-m';
import { ApiRequest } from '../service-models/foundation/api-contracts/base/api-request';
import { ApiResponse } from '../service-models/foundation/api-contracts/base/api-response';
import { BaseService } from './base.service';

@Injectable({
  providedIn: 'root'
})
export class EmployeeAttendanceService extends BaseService{

  constructor(private employeeAttendanceClient:EmployeeAttendanceClient) {
    super()
   }

/**
 * get employe  Attendance Status from client 
 * @param date 
 * @returns 
 */
   async getEmployeeAttendance(date: Date): Promise<ApiResponse<ClientEmployeeAttendanceSM[]>> {
    return await this.employeeAttendanceClient.GetEmployeeAttendance(date)
  }
/**
 * Add Check in time
 * @param employeeCheckIn 
 * @returns 
 */
  async addEmployeeCheckInTime(employeeCheckIn:ClientEmployeeAttendanceSM): Promise<ApiResponse<ClientEmployeeAttendanceSM>> {
    let apiRequest = new ApiRequest<ClientEmployeeAttendanceSM>();
    apiRequest.reqData = employeeCheckIn;
    return await this.employeeAttendanceClient.AddEmployeeCheckInTime(apiRequest);
  }

  /**
   * add and update checkOut time and status of the employee
   * @param employeeCheckIn 
   * @returns 
   */
  async  updateEmployeeCheckOutTime(employeeCheckIn:ClientEmployeeAttendanceSM): Promise<ApiResponse<ClientEmployeeAttendanceSM>> {
    let apiRequest = new ApiRequest<ClientEmployeeAttendanceSM>();
    apiRequest.reqData = employeeCheckIn;
    return await this.employeeAttendanceClient. updateEmployeeCheckOutTime(apiRequest );
  }
}
