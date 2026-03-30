import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { EmployeeAdditionalReimbursementClient } from '../clients/employee-additional-reimbursement.client';
import { ClientEmployeeAdditionalReimbursementLogSM } from '../service-models/app/v1/client/client-employee-additional-reimbursement-log-s-m';
import { ApiResponse } from '../service-models/foundation/api-contracts/base/api-response';
import { ApiRequest } from '../service-models/foundation/api-contracts/base/api-request';

@Injectable({
  providedIn: 'root'
})
export class EmployeeAdditionalReimbursementService extends BaseService {
  constructor(private reimbursementClient: EmployeeAdditionalReimbursementClient) {
    super();
  }

  async getMyReimbursementLogs(): Promise<ApiResponse<ClientEmployeeAdditionalReimbursementLogSM[]>> {
    return await this.reimbursementClient.GetMyReimbursementLogs();
  }

  async getReimbursementLogsByEmployeeId(employeeId: number): Promise<ApiResponse<ClientEmployeeAdditionalReimbursementLogSM[]>> {
    if (!employeeId || employeeId <= 0) {
      throw new Error('Invalid employee ID provided');
    }
    return await this.reimbursementClient.GetReimbursementLogsByEmployeeId(employeeId);
  }

  async createReimbursementLog(log: ClientEmployeeAdditionalReimbursementLogSM): Promise<ApiResponse<ClientEmployeeAdditionalReimbursementLogSM>> {
    if (!log) {
      throw new Error('Invalid reimbursement log data provided');
    }
    const request = new ApiRequest<ClientEmployeeAdditionalReimbursementLogSM>();
    request.reqData = log;
    return await this.reimbursementClient.CreateReimbursement(request);
  }

  async updateReimbursementLog(id: number, log: ClientEmployeeAdditionalReimbursementLogSM): Promise<ApiResponse<ClientEmployeeAdditionalReimbursementLogSM>> {
    if (!id || id <= 0 || !log) {
      throw new Error('Invalid ID or reimbursement log data provided');
    }
    const request = new ApiRequest<ClientEmployeeAdditionalReimbursementLogSM>();
    request.reqData = log;
    return await this.reimbursementClient.UpdateReimbursement(id, request);
  }

  async deleteReimbursementLog(id: number): Promise<ApiResponse<void>> {
    if (!id || id <= 0) {
      throw new Error('Invalid ID provided');
    }
    return await this.reimbursementClient.DeleteReimbursement(id);
  }
}
