import { Injectable } from "@angular/core";
import { AppConstants } from "src/app-constants";
import { ClientUserSM } from "../service-models/app/v1/app-users/client-user-s-m";
import { ClientCompanyAddressSM } from "../service-models/app/v1/client/client-company-address-s-m";
import { ClientCompanyDetailSM } from "../service-models/app/v1/client/client-company-detail-s-m";
import { CompanyModulesSM } from "../service-models/app/v1/client/company-modules-s-m";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { QueryFilter } from "../service-models/foundation/api-contracts/query-filter";
import { IntResponseRoot } from "../service-models/foundation/common-response/int-response-root";
import { StorageService } from "../services/storage.service";
import { BaseApiClient } from "./base-client/base-api.client";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { StorageCache } from "./helpers/storage-cache.helper";
import { DeleteResponseRoot } from "../service-models/foundation/common-response/delete-response-root";
import { AdditionalRequestDetails, Authentication } from "../internal-models/additional-request-details";
@Injectable({
  providedIn: "root",
})
export class SuperCompanyClient extends BaseApiClient {
  constructor(
    storageService: StorageService,
    storageCache: StorageCache,
    commonResponseCodeHandler: CommonResponseCodeHandler
  ) {
    super(storageService, storageCache, commonResponseCodeHandler);
  }

  //SignUp Methods Start
/**
 * Register New Company
 * @param companyDetails
 * @returns
 */
RegisterNewCompanyDetails = async (companyDetails: ApiRequest<ClientCompanyDetailSM>): Promise<ApiResponse<ClientCompanyDetailSM>> => {
  let resp = await this.GetResponseAsync<ClientCompanyDetailSM, ClientCompanyDetailSM>(`${AppConstants.ApiUrls.COMPANY_DETAILS_URL}`, 'POST', companyDetails,
  new AdditionalRequestDetails<ClientCompanyDetailSM>(false, Authentication.false ));
  return resp;
}
/**
* Register New Company Admin
* @param companyDetails
* @returns
*/
RegisterNewCompanyAdminDetails = async (companyDetails: ApiRequest<ClientUserSM>): Promise<ApiResponse<ClientUserSM>> => {
  let resp = await this.GetResponseAsync<ClientUserSM, ClientUserSM>(`${AppConstants.ApiUrls.COMPANY_USER_URL}/CompanyAdmin`, 'POST', companyDetails,
  new AdditionalRequestDetails<ClientUserSM>(false, Authentication.false ));
  return resp;
}

//---SignUp Methods End---//

  /** Get Company Details By Id request (GET method) */
  GetAllCompanyDetailsByCompanyId = async (
    companyId: number
  ): Promise<ApiResponse<ClientCompanyDetailSM>> => {
    let resp = await this.GetResponseAsync<null, ClientCompanyDetailSM>(
      `${AppConstants.ApiUrls.COMPANY_DETAILS_URL}/${companyId}`,
      "GET"
    );
    return resp;
  };

     /** Add New Compny Details
      * (POST method)
      */
     AddNewCompanyDetails = async (company: ApiRequest<ClientCompanyDetailSM>): Promise<ApiResponse<ClientCompanyDetailSM>> => {
      let resp = await this.GetResponseAsync<ClientCompanyDetailSM, ClientCompanyDetailSM>(`${AppConstants.ApiUrls.COMPANY_DETAILS_URL}`, 'POST', company);
      return resp;
  }


  /** Get Company Address Details  By Company Id (GET method)
   * @param Id
   */
  GetCompanyAddressByCompanyId = async (
    companyId: number
  ): Promise<ApiResponse<ClientCompanyAddressSM>> => {
    let resp = await this.GetResponseAsync<number, ClientCompanyAddressSM>(
      `${AppConstants.ApiUrls.COMPANY_ADDRESS_URL}/CompanyId/${companyId}`,
      "GET"
    );
    return resp;
  };

      /** Add New Compny Address
       * (POST method)
      */
      AddNewCompanyAddress = async (companyAddress: ApiRequest<ClientCompanyAddressSM>): Promise<ApiResponse<ClientCompanyAddressSM>> => {
        let resp = await this.GetResponseAsync<ClientCompanyAddressSM, ClientCompanyAddressSM>(`${AppConstants.ApiUrls.COMPANY_ADDRESS_URL}`, 'POST', companyAddress)
        return resp;
    }
  /**Get List of Users associated with the Company By company id and Odata
   * (GET method)
  */

  GetCompanyAdminsByCompanyIdUsingOdata = async (queryFilter: QueryFilter,   companyId: number): Promise<ApiResponse<ClientUserSM[]>> => {
    let finalUrl = this.ApplyQueryFilterToUrl(`${AppConstants.ApiUrls.COMPANY_USER_URL}/odata`, queryFilter);
    let resp = await this.GetResponseAsync<null, ClientUserSM[]>(`${finalUrl}&&$filter=clientcompanydetailid eq ${companyId} and  RoleType eq 'ClientAdmin'`, 'GET');
    return resp;
  }
    /**Call Get API to fetch Total number of Company User Count (GET method)
    */
    GetAllCompanyAdminsCountByCompanyId = async (compId:number): Promise<ApiResponse<IntResponseRoot>> => {
      let resp = await this.GetResponseAsync<null, IntResponseRoot>(
        `${AppConstants.ApiUrls.COMPANY_USER_URL}/ClientCompanyUserCountResponse/${compId}`,
        "GET"
      );
      return resp;
    };
     /** Get company admin Info By admin Id*/
     GetCompanyAdminsByAdminId = async (adminId: number): Promise<ApiResponse<ClientUserSM>> => {
      let resp = await this.GetResponseAsync<number, ClientUserSM>(`${AppConstants.ApiUrls.COMPANY_USER_URL}/${adminId}`, 'GET');
      return resp;
  }
      /** Delete company admin  By admin Id*/
      DeleteCompanyAdminByAdminId = async (adminId: number): Promise<ApiResponse<DeleteResponseRoot>> => {
        let resp = await this.GetResponseAsync<number, DeleteResponseRoot>(`${AppConstants.ApiUrls.COMPANY_USER_URL}/${adminId}`, 'DELETE');
        return resp;
    }

  /**Update Company User Login Status by user Id (PUT method)*/
  UpdateUserLoginStatus = async (
    updateLoginStatusRequest: ApiRequest<ClientUserSM>
  ): Promise<ApiResponse<ClientUserSM>> => {
    let resp = await this.GetResponseAsync<
    ClientUserSM,
    ClientUserSM
    >(
      `${AppConstants.ApiUrls.SUPER_COMPANY_SETTING_URL}/CompanyAdminUserActivationSetting/${updateLoginStatusRequest.reqData.id}/${updateLoginStatusRequest.reqData.loginStatus}`,
      "PUT"
    );
    return resp;
  };


   /** Add New ClientAdmin for the Company
    * (POST method)
    */
   AddNewCompanyAdmin = async (user: ApiRequest<ClientUserSM>): Promise<ApiResponse<ClientUserSM>> => {
    let resp = await this.GetResponseAsync<ClientUserSM, ClientUserSM>(`${AppConstants.ApiUrls.COMPANY_USER_URL}/CompanyAdmin`, 'POST', user);
    return resp;
}

      /** Update Admin Info By clientUser Id  */
      UpdateCompanyAdminDetails = async (updateAdmin: ApiRequest<ClientUserSM>): Promise<ApiResponse<ClientUserSM>> => {
        let resp = await this.GetResponseAsync<ClientUserSM, ClientUserSM>(`${AppConstants.ApiUrls.COMPANY_USER_URL}/CompanyAdmin/${updateAdmin.reqData.id}`, 'PUT', updateAdmin)
        return resp;
    }

}
