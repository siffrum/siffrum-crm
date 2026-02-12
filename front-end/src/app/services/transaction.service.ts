import { Injectable } from "@angular/core";
import { TransactionClient } from "../clients/transaction.client";
import { GeneratePayrollTransactionSM } from "../service-models/app/v1/client/generate-payroll-transaction-s-m";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { BaseService } from "./base.service";
import { IntResponseRoot } from "../service-models/foundation/common-response/int-response-root";
import { QueryFilter } from "../service-models/foundation/api-contracts/query-filter";
import { TransactionViewModel } from "../view-models/payroll-transaction.viewmodel";


@Injectable({
  providedIn: 'root'
})


export class TransactionService extends BaseService {

  constructor(
    private transactionClient: TransactionClient,
  ) {
    super();
  }

  /** Get Total Employee Count of a Company */
  async getPayrollTransactionCountOfCompany(UTCDate:Date): Promise<ApiResponse<IntResponseRoot>> {
    return await this.transactionClient.GetPayrollTransactionCountOfCompany(UTCDate);
  }
  /** Get Payroll Transaction Monthly Based */
  async getPayrollTransactionMonthlyBased(viewModel:TransactionViewModel ,date: string): Promise<ApiResponse<GeneratePayrollTransactionSM[]>> {
    let queryFilter = new QueryFilter();
    queryFilter.skip = (viewModel.pagination.PageNo - 1) * viewModel.pagination.PageSize;
    queryFilter.top = viewModel.pagination.PageSize;
    return await this.transactionClient.GetPayrollTransactionMonthlyBased(queryFilter,date);
  }


  /** Generate Employee Payroll on ClientUserId  */
  async generatePayrollByClientUserId(generatePayroll: GeneratePayrollTransactionSM): Promise<ApiResponse<GeneratePayrollTransactionSM>> {
    let apiRequest = new ApiRequest<GeneratePayrollTransactionSM>();
    apiRequest.reqData = generatePayroll;
    return await this.transactionClient.GeneratePayrollByClientUserId(apiRequest);
  }


  /** Generate All Employee Payroll  */
  async GenerateAllEmployeePayroll(generatePayroll: GeneratePayrollTransactionSM[]): Promise<ApiResponse<GeneratePayrollTransactionSM[]>> {
    let apiRequest = new ApiRequest<GeneratePayrollTransactionSM[]>();
    apiRequest.reqData = generatePayroll;
    return await this.transactionClient.GenerateAllEmployeePayroll(apiRequest);
  }

}
