import { Injectable } from "@angular/core";
import { AppConstants } from "src/app-constants";
import { GeneratePayrollTransactionSM } from "../service-models/app/v1/client/generate-payroll-transaction-s-m";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { StorageService } from "../services/storage.service";
import { BaseApiClient } from "./base-client/base-api.client";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { StorageCache } from "./helpers/storage-cache.helper";
import { IntResponseRoot } from "../service-models/foundation/common-response/int-response-root";
import { QueryFilter } from "../service-models/foundation/api-contracts/query-filter";


@Injectable({
    providedIn: 'root'
})


export class TransactionClient extends BaseApiClient {
    constructor(storageService: StorageService, storageCache: StorageCache,
        commonResponseCodeHandler: CommonResponseCodeHandler) {
        super(storageService, storageCache, commonResponseCodeHandler)
    }
    /** Get Total Payroll Transaction Count of a Company */
    GetPayrollTransactionCountOfCompany = async (UTCDate:Date): Promise<ApiResponse<IntResponseRoot>> => {
        let resp = await this.GetResponseAsync<null, IntResponseRoot>(`${AppConstants.ApiUrls.PAYROLL_TRANSACTION_URL}/PayrollTransactionCountResponse?dateTime=${UTCDate}`, 'GET');
        return resp;
    }
    /** Get Payroll Transaction Monthly Based  */
    GetPayrollTransactionMonthlyBased = async (queryFilter: QueryFilter,date: string): Promise<ApiResponse<GeneratePayrollTransactionSM[]>> => {
        let finalUrl = this.ApplyQueryFilterToUrl(`${AppConstants.ApiUrls.PAYROLL_TRANSACTION_URL}/my/AllPayrollTransactionsBasedonDateTime?dateTime=${date}&skip=${queryFilter.skip}&top=${queryFilter.top}`);
        let resp = await this.GetResponseAsync<number, GeneratePayrollTransactionSM[]>(`${finalUrl}`, 'POST');
        return resp;
    }



    /** Generate Employee Payroll on ClientUserId  */
    GeneratePayrollByClientUserId = async (generatePayroll: ApiRequest<GeneratePayrollTransactionSM>): Promise<ApiResponse<GeneratePayrollTransactionSM>> => {
        let resp = await this.GetResponseAsync<GeneratePayrollTransactionSM, GeneratePayrollTransactionSM>(`${AppConstants.ApiUrls.PAYROLL_TRANSACTION_URL}/GeneratePayroll`, 'POST', generatePayroll);
        return resp;
    }


    /** Generate All Employee Payroll  */
    GenerateAllEmployeePayroll = async (generatePayroll: ApiRequest<GeneratePayrollTransactionSM[]>): Promise<ApiResponse<GeneratePayrollTransactionSM[]>> => {
        let resp = await this.GetResponseAsync<GeneratePayrollTransactionSM[], GeneratePayrollTransactionSM[]>(`${AppConstants.ApiUrls.PAYROLL_TRANSACTION_URL}/GenerateAllPayrolls`, 'POST', generatePayroll);
        return resp;
    }





}