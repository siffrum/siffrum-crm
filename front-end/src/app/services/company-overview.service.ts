import { Injectable } from "@angular/core";
import { CompanyOverviewClient } from "../clients/company-overview.client";
import { ClientCompanyAddressSM } from "../service-models/app/v1/client/client-company-address-s-m";
import { ClientCompanyDetailSM } from "../service-models/app/v1/client/client-company-detail-s-m";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { BaseService } from "./base.service";
import { DeleteResponseRoot } from "../service-models/foundation/common-response/delete-response-root";

@Injectable({
  providedIn: "root",
})
export class CompanyOverviewService extends BaseService {
  constructor(private companyClient: CompanyOverviewClient) {
    super();
  }

/**
 * Get Company Details
 * @returns
 */
  async getCompanyDetails(): Promise<ApiResponse<ClientCompanyDetailSM>> {
    return await this.companyClient.GetCompanyDetails();
  }
     /**GET Company Logo
   * @DEV : Musaib
  */
     async GetCompanyLogo(): Promise<ApiResponse<string>> {
      return await this.companyClient.GetCompanyLogo();
    }
      /**Add Company Logo
   * @DEV : Musaib
  */
async AddCompanyLogo(addCompanyLogo:string): Promise<ApiResponse<string>> {
  let apiRequest = new ApiRequest<string>();
  apiRequest.reqData = addCompanyLogo;
  return await this.companyClient.AddCompanyLogo(apiRequest);
}
/**Delete Company Logo 
   * @DEV : Musaib
  */
async DeleteCompanyLogo(): Promise<ApiResponse<DeleteResponseRoot>> {
  return await this.companyClient.DeleteCompanyLogo( );
}
  /**
   * Update Company Details
   * @param updateCompanyDetails 
   * @returns 
   */
  async updateCompanyDetails(
    updateCompanyDetails: ClientCompanyDetailSM
  ): Promise<ApiResponse<ClientCompanyDetailSM>> {
    let apiRequest = new ApiRequest<ClientCompanyDetailSM>();
    apiRequest.reqData=updateCompanyDetails
    return await this.companyClient.UpdateCompanyDetails(
      apiRequest
    );
  }
/**Get Company Address */
  async getCompanyAddress(companyId: number): Promise<ApiResponse<ClientCompanyAddressSM>> {
    return await this.companyClient.GetCompanyAddress(companyId);
  }
/**
 * Update Aompany Address
 * @param updateCompanyAddressComponent 
 * @returns 
 */
  async updateCompanyAddress(
    updateCompanyAddressComponent: ClientCompanyAddressSM
  ): Promise<ApiResponse<ClientCompanyAddressSM>> {
    let apiRequest = new ApiRequest<ClientCompanyAddressSM>();
    apiRequest.reqData = updateCompanyAddressComponent ;
    return await this.companyClient.UpdateCompanyAddress(
      apiRequest
    );
  }

}
