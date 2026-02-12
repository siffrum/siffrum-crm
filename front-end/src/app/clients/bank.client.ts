import { Injectable } from "@angular/core";
import { AppConstants } from "src/app-constants";
import { ClientEmployeeBankDetailSM } from "../service-models/app/v1/client/client-employee-bank-detail-s-m";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { QueryFilter } from "../service-models/foundation/api-contracts/query-filter";
import { DeleteResponseRoot } from "../service-models/foundation/common-response/delete-response-root";
import { StorageService } from "../services/storage.service";
import { BaseApiClient } from "./base-client/base-api.client";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { StorageCache } from "./helpers/storage-cache.helper";

@Injectable({
    providedIn: 'root'
})


export class BankClient extends BaseApiClient {
    constructor(storageService: StorageService, storageCache: StorageCache,
        commonResponseCodeHandler: CommonResponseCodeHandler) {
        super(storageService, storageCache, commonResponseCodeHandler)
    }


    /** Get Employee Bank-Info By Employee Id oData*/
    GetEmployeeBankByCompanyIdAndEmployeeIdWithOData = async (queryFilter: QueryFilter,companyId:number,employeeId: number): Promise<ApiResponse<ClientEmployeeBankDetailSM[]>> => {
        let finalUrl = await this.ApplyQueryFilterToUrl(`${AppConstants.ApiUrls.EMPLOYEE_BANK_URL}/odata`,queryFilter);
        let resp = await this.GetResponseAsync<null, ClientEmployeeBankDetailSM[]>(`${finalUrl}&&$filter=ClientCompanyDetailID eq ${companyId} and clientuserid eq ${employeeId}`, 'GET');
        return resp;
    }


    /** Get Employee Bank-Info By Employee Id */
    GetEmployeeBankByEmployeeId = async (employeeId: number): Promise<ApiResponse<ClientEmployeeBankDetailSM[]>> => {
        let resp = await this.GetResponseAsync<number, ClientEmployeeBankDetailSM[]>(`${AppConstants.ApiUrls.EMPLOYEE_BANK_URL}/Employee/${employeeId}`, 'GET');
        return resp;
    }


    /** Add Employee Bank-Info */
    AddEmployeeBankInfo = async (employeeBank: ApiRequest<ClientEmployeeBankDetailSM>): Promise<ApiResponse<ClientEmployeeBankDetailSM>> => {
        let resp = await this.GetResponseAsync<ClientEmployeeBankDetailSM, ClientEmployeeBankDetailSM>(`${AppConstants.ApiUrls.EMPLOYEE_BANK_URL}`, 'POST', employeeBank);
        return resp;
    }


    /** Update Employee Bank-Info By Bank Id */
    UpdateEmployeeBankByBankId = async (updateEmployeeBank: ApiRequest<ClientEmployeeBankDetailSM>): Promise<ApiResponse<ClientEmployeeBankDetailSM>> => {
        let resp = await this.GetResponseAsync<ClientEmployeeBankDetailSM, ClientEmployeeBankDetailSM>(`${AppConstants.ApiUrls.EMPLOYEE_BANK_URL}/${updateEmployeeBank.reqData.id}`, 'PUT', updateEmployeeBank)
        return resp;
    }

    /** Get Employee Bank-Info By Bank Id*/
    GetEmployeeBankByBankId = async (bankId: number): Promise<ApiResponse<ClientEmployeeBankDetailSM>> => {
        let resp = await this.GetResponseAsync<number, ClientEmployeeBankDetailSM>(`${AppConstants.ApiUrls.EMPLOYEE_BANK_URL}/${bankId}`, 'GET');
        return resp;
    }


    /** Delete Employee Bank-Info By Bank Id*/
    DeleteEmployeeBankByBankId = async (bankId: number): Promise<ApiResponse<DeleteResponseRoot>> => {
        let resp = await this.GetResponseAsync<number, DeleteResponseRoot>(`${AppConstants.ApiUrls.EMPLOYEE_BANK_URL}/${bankId}`, 'DELETE');
        return resp;
    }
}