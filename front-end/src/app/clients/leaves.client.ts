import { Injectable } from "@angular/core";
import { AppConstants } from "src/app-constants";
import { ClientEmployeeLeaveExtendedUserSM } from "../service-models/app/v1/client/client-employee-leave-extended-user-s-m";
import { ClientEmployeeLeaveSM } from "../service-models/app/v1/client/client-employee-leave-s-m";

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


export class LeavesClient extends BaseApiClient {

    constructor(storageService: StorageService, storageCache: StorageCache,
        commonResponseCodeHandler: CommonResponseCodeHandler) {
        super(storageService, storageCache, commonResponseCodeHandler)
    }

    /**Get All Leaves of a Company By oData */
    GetAllCompanyLeavesByOdata = async (queryFilter: QueryFilter): Promise<ApiResponse<ClientEmployeeLeaveExtendedUserSM[]>> => {
        let finalUrl = this.ApplyQueryFilterToUrl(`${AppConstants.ApiUrls.EMPLOYEE_LEAVES_URL}/my/LeaveExtendedByUserName?skip=${queryFilter.skip}&top=${queryFilter.top}`);
        let resp = await this.GetResponseAsync<number, ClientEmployeeLeaveExtendedUserSM[]>(`${finalUrl}`, 'GET');
        return resp;
    }
    /** Get Leaves by Employee Id */
    GetEmployeeLeavesByEmployeeId = async (employeeId: number): Promise<ApiResponse<ClientEmployeeLeaveSM[]>> => {
        let resp = await this.GetResponseAsync<number, ClientEmployeeLeaveSM[]>(`${AppConstants.ApiUrls.EMPLOYEE_LEAVES_URL}/ClientEmployeeLeaveByUserId/${employeeId}`, 'GET');
        return resp;
    }
    /** Get Employee Leaves By Employee Id By oData */
    GetEmployeeLeavesByEmployeeIdandOdata = async (queryFilter: QueryFilter, employeeId: number): Promise<ApiResponse<ClientEmployeeLeaveExtendedUserSM[]>> => {
        let finalUrl = this.ApplyQueryFilterToUrl(`${AppConstants.ApiUrls.EMPLOYEE_LEAVES_URL}/odata`,queryFilter);
        let resp = await this.GetResponseAsync<null, ClientEmployeeLeaveExtendedUserSM[]>(`${finalUrl}&&$filter=clientUserId eq ${employeeId}`, 'GET');
        return resp;
    }
    /** Get Employee Leave By Leave Id */
    GetEmployeeLeaveByLeaveId = async (leaveId: number): Promise<ApiResponse<ClientEmployeeLeaveExtendedUserSM>> => {
        let resp = await this.GetResponseAsync<number, ClientEmployeeLeaveExtendedUserSM>(`${AppConstants.ApiUrls.EMPLOYEE_LEAVES_URL}/${leaveId}`, 'GET');
        return resp;
    }
    /** Add Employee Leave */
    AddEmployeeLeave = async (addLeaveRequest: ApiRequest<ClientEmployeeLeaveExtendedUserSM>): Promise<ApiResponse<ClientEmployeeLeaveExtendedUserSM>> => {
        let resp = await this.GetResponseAsync<ClientEmployeeLeaveExtendedUserSM, ClientEmployeeLeaveExtendedUserSM>(AppConstants.ApiUrls.EMPLOYEE_LEAVES_URL, 'POST', addLeaveRequest);
        return resp;
    }
    /** Update Leave by Leave Id  */
    UpdateEmployeeLeave = async (updateLeaveRequest: ApiRequest<ClientEmployeeLeaveExtendedUserSM>): Promise<ApiResponse<ClientEmployeeLeaveExtendedUserSM>> => {
        let resp = await this.GetResponseAsync<ClientEmployeeLeaveExtendedUserSM, ClientEmployeeLeaveExtendedUserSM>
            (`${AppConstants.ApiUrls.EMPLOYEE_LEAVES_URL}/${updateLeaveRequest.reqData.id}`, 'PUT', updateLeaveRequest);
        return resp;
    }
    /** Delete Employee Leave By Leave Id */
    DeleteEmployeeLeaveByLeaveId = async (Id: number): Promise<ApiResponse<DeleteResponseRoot>> => {
        let resp = await this.GetResponseAsync<number, DeleteResponseRoot>(`${AppConstants.ApiUrls.EMPLOYEE_LEAVES_URL}/${Id}`, 'DELETE');
        return resp;
    }
    /** Get Total Leaves Count of a Company */
    GetLeavesCountOfCompany = async (): Promise<ApiResponse<IntResponseRoot>> => {
        let resp = await this.GetResponseAsync<null, IntResponseRoot>(`${AppConstants.ApiUrls.EMPLOYEE_LEAVES_URL}/AllClientEmployeeLeaveCountResponse`, 'GET');
        return resp;
    }
    /** Get Total Leave Count of one Employee By Employee Id */
    GetEmployeeLeavesCountByEmployeeId = async (employeeId: number): Promise<ApiResponse<IntResponseRoot>> => {
        let resp = await this.GetResponseAsync<number, IntResponseRoot>(`${AppConstants.ApiUrls.EMPLOYEE_LEAVES_URL}/ClientEmployeeLeaveCountResponse/${employeeId}`, 'GET');
        return resp;
    }
}