import { Injectable } from "@angular/core";
import { AppConstants } from "src/app-constants";
import { ClientUserAddressSM } from "../service-models/app/v1/app-users/client-user-address-s-m";
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


export class AddressClient extends BaseApiClient {
    constructor(storageService: StorageService, storageCache: StorageCache,
        commonResponseCodeHandler: CommonResponseCodeHandler) {
        super(storageService, storageCache, commonResponseCodeHandler)
    }

    /** Get Employee Address By Employee Id oData */
    GetEmployeeAddressByCompanyIdAndEmployeeIdWithOData = async (queryFilter: QueryFilter,compId:number, employeeId: number): Promise<ApiResponse<ClientUserAddressSM[]>> => {
        let finalUrl = await this.ApplyQueryFilterToUrl(`${AppConstants.ApiUrls.EMPLOYEE_ADDRESS_URL}/odata`,queryFilter);
        let resp = await this.GetResponseAsync<null, ClientUserAddressSM[]>(`${finalUrl}&&$filter=ClientCompanyDetailID eq ${compId}  and ClientUserId eq ${employeeId} `, 'GET');
        return resp;
    }


    /** Get Employee Address-Info By Employee Id*/
    GetEmployeeAddressByEmployeeId = async (employeeId: number): Promise<ApiResponse<ClientUserAddressSM[]>> => {
        let resp = await this.GetResponseAsync<number, ClientUserAddressSM[]>(`${AppConstants.ApiUrls.EMPLOYEE_ADDRESS_URL}/ClientUserId/${employeeId}`, 'GET');
        return resp;
    }
    /** Add Employee Address-Info */
    AddEmployeeAddress = async (employee: ApiRequest<ClientUserAddressSM>): Promise<ApiResponse<ClientUserAddressSM>> => {
        let resp = await this.GetResponseAsync<ClientUserAddressSM, ClientUserAddressSM>(`${AppConstants.ApiUrls.EMPLOYEE_ADDRESS_URL}`, 'POST', employee);
        return resp;
    }
    /** Update Employee Address-Info By Address Id */
    UpdateEmployeeAddressByAddressId = async (updateEmployeeAddress: ApiRequest<ClientUserAddressSM>): Promise<ApiResponse<ClientUserAddressSM>> => {
        let resp = await this.GetResponseAsync<ClientUserAddressSM, ClientUserAddressSM>(`${AppConstants.ApiUrls.EMPLOYEE_ADDRESS_URL}/${updateEmployeeAddress.reqData.id}`, 'PUT', updateEmployeeAddress)
        return resp;
    }


    /** Get Employee Address-Info By Address Id*/
    GetEmployeeAddressByAddressId = async (addressId: number): Promise<ApiResponse<ClientUserAddressSM>> => {
        let resp = await this.GetResponseAsync<number, ClientUserAddressSM>(`${AppConstants.ApiUrls.EMPLOYEE_ADDRESS_URL}/${addressId}`, 'GET');
        return resp;
    }


    /** Delete Employee Address-Info By Address Id*/
    DeleteEmployeeAddressInfoByAddressId = async (addressId: number): Promise<ApiResponse<DeleteResponseRoot>> => {
        let resp = await this.GetResponseAsync<number, DeleteResponseRoot>(`${AppConstants.ApiUrls.EMPLOYEE_ADDRESS_URL}/${addressId}`, 'DELETE');
        return resp;
    }



}