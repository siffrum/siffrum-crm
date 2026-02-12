import { Injectable } from "@angular/core";
import { BaseService } from "./base.service";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { ClientCompanyDepartmentSM } from "../service-models/app/v1/client/client-company-department-s-m";
import { CompanyDepartmentClient } from "../clients/company-departments.client";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { DeleteResponseRoot } from "../service-models/foundation/common-response/delete-response-root";
import { AppConstants } from "src/app-constants";


@Injectable({
    providedIn: 'root'
  })
  export class CompanyDepartmentService extends BaseService{
    constructor(private companyDepartmentClient:CompanyDepartmentClient){
        super()
    }
    async getAllCompanyDepartments ():Promise<ApiResponse<ClientCompanyDepartmentSM[]>> {
      return this.companyDepartmentClient.GetCompanyDepartments()
    }
      /**
 * Get Company Department Details For Selected Company
 * @param companyId
 * @returns
 */
  async getCompanyDepartmentById(
    Id: number
  ): Promise<ApiResponse<ClientCompanyDepartmentSM>> {
    return await this.companyDepartmentClient.GetCompanyDepartmentById(Id);
  }

   /**update Company shift Details */
   async updateCompanyDepartmentDetails(updatecompanyDepartmentDetails:ClientCompanyDepartmentSM): Promise<ApiResponse<ClientCompanyDepartmentSM>> {
    let apiRequest = new ApiRequest<ClientCompanyDepartmentSM>();
    apiRequest.reqData = updatecompanyDepartmentDetails;
    return await this.companyDepartmentClient.UpdateCompanyDepartmentDetails(apiRequest);
  }
     /** Add Company Department Details
   * @return Success
    */
     async addNewCompanyDepartmentDetails(user:ClientCompanyDepartmentSM): Promise<ApiResponse<ClientCompanyDepartmentSM>> {
      let apiRequest = new ApiRequest<ClientCompanyDepartmentSM>();
      apiRequest.reqData =user;
      return await this.companyDepartmentClient.AddNewCompanyDepartmentDetails(apiRequest);
    }

     /**Delete Company Department
   * @DEV : Musaib
  */
  async DeleteCompanyDepartment(
    id: number
  ): Promise<ApiResponse<DeleteResponseRoot>> {
    if (id <= 0) {
      throw new Error(AppConstants.ErrorPrompts.Delete_Data_Error);
    }
    return await this.companyDepartmentClient.DeleteCompanyDepartmentById(
      id
    );
  }
  }