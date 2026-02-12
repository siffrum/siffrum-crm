import { Injectable } from '@angular/core';
import { LeavesClient } from '../clients/leaves.client';
import { ClientEmployeeLeaveExtendedUserSM } from '../service-models/app/v1/client/client-employee-leave-extended-user-s-m';
import { ClientEmployeeLeaveSM } from '../service-models/app/v1/client/client-employee-leave-s-m';
import { LeaveReportRequestSM } from '../service-models/app/v1/client/leave-report-request-s-m';
import { ApiRequest } from '../service-models/foundation/api-contracts/base/api-request';
import { ApiResponse } from '../service-models/foundation/api-contracts/base/api-response';
import { QueryFilter } from '../service-models/foundation/api-contracts/query-filter';
import { DeleteResponseRoot } from '../service-models/foundation/common-response/delete-response-root';
import { IntResponseRoot } from '../service-models/foundation/common-response/int-response-root';
import { BaseService } from './base.service';
import { EmployeeLeavesInfoViewModel } from '../view-models/employee-leaves-info.viewmodel';


@Injectable({
  providedIn: 'root'
})


export class LeavesService extends BaseService {

  constructor(private leavesClient: LeavesClient) {
    super();
  }





  /** Get All Company Leaves By oData  */
  async getAllCompanyLeavesByOdata(viewModel:EmployeeLeavesInfoViewModel): Promise<ApiResponse<ClientEmployeeLeaveExtendedUserSM[]>> {
    let queryFilter = new QueryFilter();
   queryFilter.skip = (viewModel.pagination.PageNo - 1) * viewModel.pagination.PageSize;
   queryFilter.top = viewModel.pagination.PageSize;
    return await this.leavesClient.GetAllCompanyLeavesByOdata(queryFilter);
  }


  /** Get Employee Leaves By Employee Id */
  async getEmployeeLeavesByEmployeeId(employeeId: number): Promise<ApiResponse<ClientEmployeeLeaveSM[]>> {
    return await this.leavesClient.GetEmployeeLeavesByEmployeeId(employeeId);
  }


  /** Get Employee Leaves By Employee Id By Odata */
  async getEmployeeLeavesByEmployeeIdByOdata( employeeId: number,viewModel:EmployeeLeavesInfoViewModel): Promise<ApiResponse<ClientEmployeeLeaveExtendedUserSM[]>> {
    let queryFilter = new QueryFilter();
   queryFilter.skip = (viewModel.pagination.PageNo - 1) * viewModel.pagination.PageSize;
   queryFilter.top = viewModel.pagination.PageSize;
    return await this.leavesClient.GetEmployeeLeavesByEmployeeIdandOdata(queryFilter, employeeId);
  }


  /** Get Employee Leave By Leave Id   */
  async getEmployeeLeaveByLeaveId(leaveId: number): Promise<ApiResponse<ClientEmployeeLeaveExtendedUserSM>> {
    return await this.leavesClient.GetEmployeeLeaveByLeaveId(leaveId);
  }


  /** Add Employee Leave   */
  async addEmployeeLeave(addLeave: ClientEmployeeLeaveExtendedUserSM): Promise<ApiResponse<ClientEmployeeLeaveExtendedUserSM>> {
    let apiRequest = new ApiRequest<ClientEmployeeLeaveExtendedUserSM>();
    apiRequest.reqData=addLeave;
    return await this.leavesClient.AddEmployeeLeave(apiRequest);
  }


  /** Update Employee Leave   */
  async updateEmployeeLeaveByLeaveId(employeeLeave: ClientEmployeeLeaveExtendedUserSM): Promise<ApiResponse<ClientEmployeeLeaveExtendedUserSM>> {
    let apiRequest = new ApiRequest<ClientEmployeeLeaveExtendedUserSM>();
    apiRequest.reqData = employeeLeave;
    return await this.leavesClient.UpdateEmployeeLeave(apiRequest);
  }


  /** Delete Employee Leave By Leave Id */
  async deleteEmployeeLeaveByLeaveId(leaveId: number): Promise<ApiResponse<DeleteResponseRoot>> {
    return await this.leavesClient.DeleteEmployeeLeaveByLeaveId(leaveId);
  }



  /** Get Total Company Leaves Count */
  async getLeavesCountOfCompany(): Promise<ApiResponse<IntResponseRoot>> {
    return await this.leavesClient.GetLeavesCountOfCompany();
  }


  /** Get One Employee Leaves Count By Employee Id */
  async getEmployeeLeaveCountByEmployeeId(employeeId: number): Promise<ApiResponse<IntResponseRoot>> {
    return await this.leavesClient.GetEmployeeLeavesCountByEmployeeId(employeeId);
  }

}
