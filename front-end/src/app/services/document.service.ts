import { Injectable } from "@angular/core";
import { DocumentClient } from "../clients/document.client";
import { ClientEmployeeDocumentSM } from "../service-models/app/v1/client/client-employee-document-s-m";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { QueryFilter } from "../service-models/foundation/api-contracts/query-filter";
import { DeleteResponseRoot } from "../service-models/foundation/common-response/delete-response-root";
import { IntResponseRoot } from "../service-models/foundation/common-response/int-response-root";
import { BaseService } from "./base.service";
import { EmployeeDocumentInfoViewModel } from "../view-models/employee-document-info.viewmodel";


@Injectable({
    providedIn: "root",
})


export class DocumentService extends BaseService {
    constructor(
        private documentClient: DocumentClient
    ) {
        super();
    }


    /** Get Employee Documents-Info By Employee Id */
    async getEmployeeDocumentsByEmployeeId(employeeId: number): Promise<ApiResponse<ClientEmployeeDocumentSM[]>> {
        return await this.documentClient.GetEmployeeDocumentsByEmployeeId(employeeId);
    }


    /** Get Employee Document-Info By Employee Id*/
    async getEmployeeDocumentsByCompanyIdAndEmployeeIdWithOData(viewModel:EmployeeDocumentInfoViewModel,companyId:number, employeeId: number,): Promise<ApiResponse<ClientEmployeeDocumentSM[]>> {
        let queryFilter = new QueryFilter();
       queryFilter.skip = (viewModel.pagination.PageNo - 1) * viewModel.pagination.PageSize;
       queryFilter.top = viewModel.pagination.PageSize;
        return await this.documentClient.GetEmployeeDocumentsByCompanyIdAndEmployeeIdWithOData(queryFilter,companyId, employeeId);
    }


    /** Get Employee Documents-Info By Document Id */
    async getEmployeeDocumentsByDocumentId(documentId: number): Promise<ApiResponse<ClientEmployeeDocumentSM>> {
        return await this.documentClient.GetEmployeeDocumentsByDocumentId(documentId);
    }
    async doenloadEmployeeDocumentsByDocumentId(documentId: number): Promise<ApiResponse<ClientEmployeeDocumentSM>> {
        return await this.documentClient.DownloadEmployeeDocumentByDocumentId(documentId);
    }

    /** Add Employee Document */
    async addEmployeeDocument(employeeDocument:ClientEmployeeDocumentSM): Promise<ApiResponse<ClientEmployeeDocumentSM>> {
        let apiRequest = new ApiRequest<ClientEmployeeDocumentSM>();
        apiRequest.reqData=employeeDocument
        return await this.documentClient.AddEmployeeDocument(apiRequest);
    }


    /** Update Employee Document By Document Id */
    async updateEmployeeDocumentByDocumentId(employeeDocument: ClientEmployeeDocumentSM): Promise<ApiResponse<ClientEmployeeDocumentSM>> {
        let apiRequest = new ApiRequest<ClientEmployeeDocumentSM>
        apiRequest.reqData=employeeDocument
        return await this.documentClient.UpdateEmployeeDocumentByDocumentId(apiRequest);
    }


    /** Delete Employee Document By Document Id */
    async deleteEmployeeDocumentByDocumentId(documentId: number): Promise<ApiResponse<DeleteResponseRoot>> {
        return await this.documentClient.DeleteEmployeeDocumentByDocumentId(documentId);
    }

  /** Get Total Employee Count of a Company */
  async getEmployeeDocumentCount(empId:number): Promise<ApiResponse<IntResponseRoot>> {
    return await this.documentClient.GetEmployeeDocumentCount(empId);
  }
}