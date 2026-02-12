import { Injectable } from "@angular/core";
import { AppConstants } from "src/app-constants";
import { SuperCompanyClient } from "../clients/super-company.client";
import { ClientUserSM } from "../service-models/app/v1/app-users/client-user-s-m";
import { ClientCompanyAddressSM } from "../service-models/app/v1/client/client-company-address-s-m";
import { ClientCompanyDetailSM } from "../service-models/app/v1/client/client-company-detail-s-m";
import { CompanyModulesSM } from "../service-models/app/v1/client/company-modules-s-m";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { QueryFilter } from "../service-models/foundation/api-contracts/query-filter";
import { IntResponseRoot } from "../service-models/foundation/common-response/int-response-root";
import { BaseService } from "./base.service";
import { AddCompanyAdminViewModel } from "../view-models/add-company-admin.viewmodel";
import { DeleteResponseRoot } from "../service-models/foundation/common-response/delete-response-root";

@Injectable({
  providedIn: "root",
})
export class SuperCompanyService extends BaseService {
  constructor(private superCompanyClient: SuperCompanyClient) {
    super();
  }
  // SignUp Methods  Start Here
     /** Register New Company  Details */
     async registerNewCompanyDetails(company: ClientCompanyDetailSM): Promise<ApiResponse<ClientCompanyDetailSM>> {
      let apiRequest = new ApiRequest<ClientCompanyDetailSM>();
      apiRequest.reqData = company;
      return await this.superCompanyClient.RegisterNewCompanyDetails(apiRequest);
    }
    //Register New Company Admin
    async registerNewCompanyAdmin(user:ClientUserSM): Promise<ApiResponse<ClientUserSM>> {
      let apiRequest = new ApiRequest<ClientUserSM>();
      apiRequest.reqData =user;
      return await this.superCompanyClient.RegisterNewCompanyAdminDetails(apiRequest);
    }

    // ---SignUp Methods End ---Here



  /**Get Company Details For Selected Company */
  async getCompanyDetailsByCompanyId(
    id: number
  ): Promise<ApiResponse<ClientCompanyDetailSM>> {
    return await this.superCompanyClient.GetAllCompanyDetailsByCompanyId(id);
  }

    /** Add New Company  Details */
    async addNewCompanyDetails(company: ClientCompanyDetailSM): Promise<ApiResponse<ClientCompanyDetailSM>> {
      let apiRequest = new ApiRequest<ClientCompanyDetailSM>();
      apiRequest.reqData = company;
      return await this.superCompanyClient.AddNewCompanyDetails(apiRequest);
    }
/**
 * Get Company Address For Selected Company
 * @param companyId
 * @returns
 */
  async getCompanyAddressByCompanyId(
    companyId: number
  ): Promise<ApiResponse<ClientCompanyAddressSM>> {
    return await this.superCompanyClient.GetCompanyAddressByCompanyId(companyId);
  }
  /**
   * Add Company Address Details
   */
  async addCompanyAddress(companyAddress:ClientCompanyAddressSM): Promise<ApiResponse<ClientCompanyAddressSM>> {
    let apiRequest = new ApiRequest<ClientCompanyAddressSM>();
    apiRequest.reqData = companyAddress;
    return await this.superCompanyClient.AddNewCompanyAddress(apiRequest);
  }

  /**
   * Get all Companies Admin list ByOdata
   * @param companyId 
   * @returns successData
   */
  async getAllCompanyAdminsByCompanyIdUsingOdata(companyId:number,viewModel:AddCompanyAdminViewModel): Promise<ApiResponse<ClientUserSM[]>> {
    let queryFilter = new QueryFilter();
    queryFilter.skip = (viewModel.pagination.PageNo - 1) * viewModel.pagination.PageSize;
    queryFilter.top = viewModel.pagination.PageSize;
    return await this.superCompanyClient. GetCompanyAdminsByCompanyIdUsingOdata(queryFilter,companyId);
  }
    /** Get Company Admin By Admin Id*/
    async getCompanyAdminsByAdminId(
      bankId: number
    ): Promise<ApiResponse<ClientUserSM>> {
      return await this.superCompanyClient.GetCompanyAdminsByAdminId(bankId);
    }
      /** Delete Company Admin By Admin Id*/
  async deleteCompanyAdminByAdminId(
    adminId: number
  ): Promise<ApiResponse<DeleteResponseRoot>> {
    return await this.superCompanyClient.DeleteCompanyAdminByAdminId(adminId);
  }
/**
 * Get Total CompanyUder Count
 * @param id 
 * @returns successData
 */
    async getAllCompanyAdminsCountByCompanyId(companyId:number): Promise<ApiResponse<IntResponseRoot>> {
      return await this.superCompanyClient.GetAllCompanyAdminsCountByCompanyId(companyId);
    }
  /**update user login status */
  async updateUserLoginStatus( status: ClientUserSM): Promise<ApiResponse<ClientUserSM>> {
    if (status == null) {
      throw new Error(AppConstants.ErrorPrompts.Invalid_Input_Data);
    } else {
      let apiRequest = new ApiRequest<ClientUserSM>();
      apiRequest.reqData = status;
      return await this.superCompanyClient.UpdateUserLoginStatus(apiRequest);
    }
  }

   /** Add New clientAdmin for  the company
   * @return Success
    */
   async addNewCompanyAdmin(user:ClientUserSM): Promise<ApiResponse<ClientUserSM>> {
    let apiRequest = new ApiRequest<ClientUserSM>();
    apiRequest.reqData =user;
    return await this.superCompanyClient.AddNewCompanyAdmin(apiRequest);
  }
  /** Update Admin Info By ClientUserId  */
  async updateCompanyAdminDetails(
    employee: ClientUserSM
  ): Promise<ApiResponse<ClientUserSM>> {
    let apiRequest = new ApiRequest<ClientUserSM>();
    apiRequest.reqData = employee;
    return await this.superCompanyClient.UpdateCompanyAdminDetails(apiRequest);
  }
}
