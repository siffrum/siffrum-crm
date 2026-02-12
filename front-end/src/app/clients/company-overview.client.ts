import { Injectable } from "@angular/core";
import { AppConstants } from "src/app-constants";
import { ClientCompanyAddressSM } from "../service-models/app/v1/client/client-company-address-s-m";
import { ClientCompanyDetailSM } from "../service-models/app/v1/client/client-company-detail-s-m";

import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { StorageService } from "../services/storage.service";
import { BaseApiClient } from "./base-client/base-api.client";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { StorageCache } from "./helpers/storage-cache.helper";
import { DeleteResponseRoot } from "../service-models/foundation/common-response/delete-response-root";

@Injectable({
  providedIn: "root",
})
export class CompanyOverviewClient extends BaseApiClient {
  constructor(
    storageService: StorageService,
    storageCache: StorageCache,
    commonResponseCodeHandler: CommonResponseCodeHandler
  ) {
    super(storageService, storageCache, commonResponseCodeHandler);
  }

  /** Get Company Details request  */
  GetCompanyDetails = async (): Promise<
    ApiResponse<ClientCompanyDetailSM>
  > => {
    let resp = await this.GetResponseAsync<null, ClientCompanyDetailSM>(
      `${AppConstants.ApiUrls.COMPANY_DETAILS_URL}/my`,
      "GET"
    );
    return resp;
  };

  /**Get Company
   * @DEV : Musaib
   */
  GetCompanyLogo = async (): Promise<ApiResponse<string>> => {
    let resp = await this.GetResponseAsync<null, string>(
      `${AppConstants.ApiUrls.COMPANY_DETAILS_URL}/mine/Logo`,
      "GET"
    );
    return resp;
  };
  /**Add a new Company Logo
   * @DEV : Musaib
   */
  AddCompanyLogo = async (
    addCompanyLogo: ApiRequest<string>
  ): Promise<ApiResponse<string>> => {
    let resp = await this.GetResponseAsync<string, string>(
      `${AppConstants.ApiUrls.COMPANY_DETAILS_URL}/mine/CompanyLogo`,
      "POST",
      addCompanyLogo
    );
    return resp;
  };

  /**Delete Company Logo
   * @DEV : Musaib
   */
  DeleteCompanyLogo = async (): Promise<ApiResponse<DeleteResponseRoot>> => {
    let resp = await this.GetResponseAsync<number, DeleteResponseRoot>(
      `${AppConstants.ApiUrls.COMPANY_DETAILS_URL}/mine/DeleteCompanyLogo`,
      "DELETE"
    );
    return resp;
  };
  /**Update Company Details  request*/
  UpdateCompanyDetails = async (
    updateCompanyDetailsRequest: ApiRequest<ClientCompanyDetailSM>
  ): Promise<ApiResponse<ClientCompanyDetailSM>> => {
    let resp = await this.GetResponseAsync<
      ClientCompanyDetailSM,
      ClientCompanyDetailSM
    >(
      `${AppConstants.ApiUrls.COMPANY_DETAILS_URL}/${updateCompanyDetailsRequest.reqData.id}`,
      "PUT",
      updateCompanyDetailsRequest
    );
    return resp;
  };
  /**UPDATE Company Address request  */
  UpdateCompanyAddress = async (
    updateCompanyAddressRequest: ApiRequest<ClientCompanyAddressSM>
  ): Promise<ApiResponse<ClientCompanyAddressSM>> => {
    let resp = await this.GetResponseAsync<
      ClientCompanyAddressSM,
      ClientCompanyAddressSM
    >(
      `${AppConstants.ApiUrls.COMPANY_ADDRESS_URL}/${updateCompanyAddressRequest.reqData.clientCompanyDetailId}`,
      "PUT",
      updateCompanyAddressRequest
    );
    return resp;
  };

  /** Get Company Details  */
  GetCompanyAddress = async (
    companyId: number
  ): Promise<ApiResponse<ClientCompanyAddressSM>> => {
    let resp = await this.GetResponseAsync<number, ClientCompanyAddressSM>(
      `${AppConstants.ApiUrls.COMPANY_ADDRESS_URL}/CompanyId/${companyId}`,
      "GET"
    );
    return resp;
  };
}
