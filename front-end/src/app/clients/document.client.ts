import { Injectable } from "@angular/core";
import { AppConstants } from "src/app-constants";
import { ClientEmployeeDocumentSM } from "../service-models/app/v1/client/client-employee-document-s-m";
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


export class DocumentClient extends BaseApiClient {
    constructor(storageService: StorageService, storageCache: StorageCache,
        commonResponseCodeHandler: CommonResponseCodeHandler) {
        super(storageService, storageCache, commonResponseCodeHandler)
    }


    /** Get Employee Documents-Info By Employee Id */
    GetEmployeeDocumentsByEmployeeId = async (employeeId: number): Promise<ApiResponse<ClientEmployeeDocumentSM[]>> => {
        let resp = await this.GetResponseAsync<number, ClientEmployeeDocumentSM[]>(`${AppConstants.ApiUrls.EMPLOYEE_DOCUMENT_URL}/ClientEmployeeId/${employeeId}`, 'GET');
        return resp;
    }


    /** Get Employee Document-Info By Employee Id*/
    GetEmployeeDocumentsByCompanyIdAndEmployeeIdWithOData = async (queryFilter: QueryFilter,compId:number, employeeId: number): Promise<ApiResponse<ClientEmployeeDocumentSM[]>> => {
        let finalUrl = await this.ApplyQueryFilterToUrl(`${AppConstants.ApiUrls.EMPLOYEE_DOCUMENT_URL}/odata`,queryFilter);
        let resp = await this.GetResponseAsync<null, ClientEmployeeDocumentSM[]>(`${finalUrl}&&$filter=ClientCompanyDetailID eq ${compId} and clientUserId eq ${employeeId}`, 'GET');
        return resp;
    }


    /** Get Employee Documents-Info By Document Id */
    GetEmployeeDocumentsByDocumentId = async (documentId: number): Promise<ApiResponse<ClientEmployeeDocumentSM>> => {
        let resp = await this.GetResponseAsync<number, ClientEmployeeDocumentSM>(`${AppConstants.ApiUrls.EMPLOYEE_DOCUMENT_URL}/${documentId}`, 'GET');
        return resp;
    }
    /**Download Employee Document By Document Id */
    DownloadEmployeeDocumentByDocumentId = async (documentId: number): Promise<ApiResponse<ClientEmployeeDocumentSM>> => {
        let resp = await this.GetResponseAsync<number, ClientEmployeeDocumentSM>(`${AppConstants.ApiUrls.EMPLOYEE_DOCUMENT_URL}/my/Documents/${documentId}`, 'GET');
        return resp;
    }


    /** Add Employee Document */
    AddEmployeeDocument = async (employeeDocument: ApiRequest<ClientEmployeeDocumentSM>): Promise<ApiResponse<ClientEmployeeDocumentSM>> => {
        let resp = await this.GetResponseAsync<ClientEmployeeDocumentSM, ClientEmployeeDocumentSM>(`${AppConstants.ApiUrls.EMPLOYEE_DOCUMENT_URL}`, 'POST', employeeDocument);
        return resp;
    }


    /** Update Employee Document By Document Id */
    UpdateEmployeeDocumentByDocumentId = async (updateEmployeeDocument: ApiRequest<ClientEmployeeDocumentSM>): Promise<ApiResponse<ClientEmployeeDocumentSM>> => {
        let resp = await this.GetResponseAsync<ClientEmployeeDocumentSM, ClientEmployeeDocumentSM>(`${AppConstants.ApiUrls.EMPLOYEE_DOCUMENT_URL}/${updateEmployeeDocument.reqData.id}`, 'PUT', updateEmployeeDocument)
        return resp;
    }


    /** Delete Employee Document By Document Id */
    DeleteEmployeeDocumentByDocumentId = async (documentId: number): Promise<ApiResponse<DeleteResponseRoot>> => {
        let resp = await this.GetResponseAsync<number, DeleteResponseRoot>(`${AppConstants.ApiUrls.EMPLOYEE_DOCUMENT_URL}/${documentId}`, 'DELETE');
        return resp;
    }

    GetEmployeeDocumentCount = async (empId:number): Promise<ApiResponse<IntResponseRoot>> => {
        let resp = await this.GetResponseAsync<null, IntResponseRoot>(`${AppConstants.ApiUrls.EMPLOYEE_DOCUMENT_URL}/ClientEmployeeDocumentCountResponse/${empId}`, 'GET');
        return resp;
    }
}