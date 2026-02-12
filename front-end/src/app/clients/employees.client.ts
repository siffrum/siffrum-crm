import { Injectable } from "@angular/core";
import { AppConstants } from "src/app-constants";
import { ClientUserSM } from "../service-models/app/v1/app-users/client-user-s-m";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { QueryFilter } from "../service-models/foundation/api-contracts/query-filter";
import { DeleteResponseRoot } from "../service-models/foundation/common-response/delete-response-root";
import { IntResponseRoot } from "../service-models/foundation/common-response/int-response-root";
import { StorageService } from "../services/storage.service";
import { BaseApiClient } from "./base-client/base-api.client";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { StorageCache } from "./helpers/storage-cache.helper";


@Injectable({
    providedIn: 'root'
})

export class EmployeesClient extends BaseApiClient {
    constructor(storageService: StorageService, storageCache: StorageCache,
        commonResponseCodeHandler: CommonResponseCodeHandler) {
        super(storageService, storageCache, commonResponseCodeHandler)
    }


    /** Get all Employees */
    GetAllEmployees = async (): Promise<ApiResponse<ClientUserSM[]>> => {
        let resp = await this.GetResponseAsync<null, ClientUserSM[]>(`${AppConstants.ApiUrls.EMPLOYEE_URL}/my/AllEmployees`, 'GET');
        return resp;
    }


    /** Get Employees by odata */
    GetAllEmployeeByCompanyIdAndOdata = async (queryFilter: QueryFilter,companyId:number): Promise<ApiResponse<ClientUserSM[]>> => {
        let finalUrl = this.ApplyQueryFilterToUrl(`${AppConstants.ApiUrls.EMPLOYEE_URL}/odata`, queryFilter );
        let resp = await this.GetResponseAsync<null, ClientUserSM[]>(`${finalUrl}&&$filter=ClientCompanyDetailID eq ${companyId} and  RoleType eq 'ClientEmployee'`, 'GET');
        return resp;
    }


    /** Get Employee By Employee Id  */
    GetEmployeeByEmployeeId = async (employeeId: number): Promise<ApiResponse<ClientUserSM>> => {
        let resp = await this.GetResponseAsync<number, ClientUserSM>(`${AppConstants.ApiUrls.EMPLOYEE_URL}/${employeeId}`, 'GET');
        return resp;
    }


    /** Get Employee by Mine Endpoints */
    GetEmployeeByMineEndpoint = async (): Promise<ApiResponse<ClientUserSM>> => {
        let resp = await this.GetResponseAsync<number, ClientUserSM>(`${AppConstants.ApiUrls.EMPLOYEE_URL}/mine`, 'GET');
        return resp;
    }


    /** Add New Employee  */
    AddNewEmployee = async (employee: ApiRequest<ClientUserSM>): Promise<ApiResponse<ClientUserSM>> => {
        let resp = await this.GetResponseAsync<ClientUserSM, ClientUserSM>(`${AppConstants.ApiUrls.EMPLOYEE_URL}`, 'POST', employee);
        return resp;
    }


    /** Update Employee Info By Employee Id  */
    UpdateEmployee = async (updateEmployee: ApiRequest<ClientUserSM>): Promise<ApiResponse<ClientUserSM>> => {
        let resp = await this.GetResponseAsync<ClientUserSM, ClientUserSM>(`${AppConstants.ApiUrls.EMPLOYEE_URL}/${updateEmployee.reqData.id}`, 'PUT', updateEmployee)
        return resp;
    }


    /** Delete Employee By Employee Id  */
    DeleteEmployee = async (id: number): Promise<ApiResponse<DeleteResponseRoot>> => {
        let resp = await this.GetResponseAsync<number, DeleteResponseRoot>(`${AppConstants.ApiUrls.EMPLOYEE_URL}/${id}`, 'DELETE');
        return resp;
    }


    /** Get Total Employee Count of a Company */
    GetEmployeeCountOfCompany = async (): Promise<ApiResponse<IntResponseRoot>> => {
        let resp = await this.GetResponseAsync<null, IntResponseRoot>(`${AppConstants.ApiUrls.EMPLOYEE_URL}/my/ClientEmployeeUserCountResponse`, 'GET');
        return resp;
    }
    /**
     * Get Total Employee Address Count 
     * @param empId 
     * @returns 
     */
    GetEmployeeAddressCount = async (empId:number): Promise<ApiResponse<IntResponseRoot>> => {
        let resp = await this.GetResponseAsync<null, IntResponseRoot>(`${AppConstants.ApiUrls.EMPLOYEE_ADDRESS_URL}/ClientUserAddressCountsResponse/${empId}`, 'GET');
        return resp;
    }
    /**
     * Get Total Employee Bank Details Count
     * @param empId 
     * @returns 
     */
    GetEmployeeBankDetailCount = async (empId:number): Promise<ApiResponse<IntResponseRoot>> => {
        let resp = await this.GetResponseAsync<null, IntResponseRoot>(`${AppConstants.ApiUrls.EMPLOYEE_BANK_URL}/ClientEmployeeBankDetailCountResponse/${empId}`, 'GET');
        return resp;
    }

  



}