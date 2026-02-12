import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { ReportsClient } from '../clients/reports.client';
import { LeaveReportRequestSM } from '../service-models/app/v1/client/leave-report-request-s-m';
import { ApiResponse } from '../service-models/foundation/api-contracts/base/api-response';
import { ClientEmployeeLeaveExtendedUserSM } from '../service-models/app/v1/client/client-employee-leave-extended-user-s-m';
import { ApiRequest } from '../service-models/foundation/api-contracts/base/api-request';
import { PayrollTransactionReportSM } from '../service-models/app/v1/client/payroll-transaction-report-s-m';
import { GeneratePayrollTransactionSM } from '../service-models/app/v1/client/generate-payroll-transaction-s-m';
import { ClientUserSM } from '../service-models/app/v1/app-users/client-user-s-m';
import { IntResponseRoot } from '../service-models/foundation/common-response/int-response-root';
import { QueryFilter } from '../service-models/foundation/api-contracts/query-filter';
import { LeavesReportViewmodel } from '../view-models/leaves-report.viewmodel';
import { PayrollTransactionReportViewmodel } from '../view-models/payroll-reports.viewmodel';

@Injectable({
  providedIn: 'root'
})
export class ReprtsService extends BaseService {

  constructor(private reportsClient:ReportsClient) {
    super()
   }
     /** Leaves Report Based on Date Time (either monthly, Yearly or Custom) */
  async getLeavesReport(leaveReportRequest: LeaveReportRequestSM,viewModel:LeavesReportViewmodel): Promise<ApiResponse<ClientEmployeeLeaveExtendedUserSM[]>> {
    let queryFilter = new QueryFilter();
    queryFilter.skip = (viewModel.pagination.PageNo - 1) * viewModel.pagination.PageSize;
    queryFilter.top = viewModel.pagination.PageSize;
    let apiRequest = new ApiRequest<LeaveReportRequestSM>();
    apiRequest.reqData = leaveReportRequest;
    return await this.reportsClient.GetLeavesReportByDate(queryFilter,apiRequest);
  }
   /** Get  Leaves Report Count Of the Company */
   async getLeavesReportCountOfCompany(leaveReportRequest: LeaveReportRequestSM): Promise<ApiResponse<IntResponseRoot>> {
    let apiRequest = new ApiRequest<LeaveReportRequestSM>();
    apiRequest.reqData = leaveReportRequest;
    return await this.reportsClient.GetLeaveReportCountOfCompany(apiRequest);
  }
   /** Get Payroll Report Count of a Company */
   async getPayrollReportCountOfCompany(payrollReportRequest: PayrollTransactionReportSM): Promise<ApiResponse<IntResponseRoot>> {
    let apiRequest = new ApiRequest<PayrollTransactionReportSM>();
    apiRequest.reqData = payrollReportRequest;
    return await this.reportsClient.GetPayrollReportCountOfCompany(apiRequest);
  }
  /**Get Payroll Transaction Report Of The Company */
  async getPayrollReport(viewModel:PayrollTransactionReportViewmodel,payrolltransactionReportRequest: PayrollTransactionReportSM): Promise<ApiResponse<GeneratePayrollTransactionSM[]>> {
    let queryFilter = new QueryFilter();
    queryFilter.skip = (viewModel.pagination.PageNo - 1) * viewModel.pagination.PageSize;
    queryFilter.top = viewModel.pagination.PageSize;
    let apiRequest = new ApiRequest<PayrollTransactionReportSM>();
    apiRequest.reqData = payrolltransactionReportRequest;
    return await this.reportsClient.GetPayrollReportByDate(queryFilter,apiRequest);
  }

  async getAllEmployeeOfCompany(): Promise<ApiResponse<ClientUserSM[]>> {
    return await this.reportsClient.GetAllEmployeesOfTheCompany();
  }
}
