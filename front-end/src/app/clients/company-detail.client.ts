import { Injectable } from "@angular/core";
import { BaseApiClient } from "./base-client/base-api.client";
import { StorageService } from "../services/storage.service";
import { StorageCache } from "./helpers/storage-cache.helper";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { AppConstants } from "src/app-constants";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { ClientCompanyDetailSM } from "../service-models/app/v1/client/client-company-detail-s-m";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";

@Injectable({
  providedIn: "root",
})
export class CompanyDetailClient extends BaseApiClient {
  constructor(
    storageService: StorageService,
    storageCache: StorageCache,
    commonResponseCodeHandler: CommonResponseCodeHandler
  ) {
    super(storageService, storageCache, commonResponseCodeHandler);
  }

  /**
   * Get company details for logged-in company
   * @returns ApiResponse<ClientCompanyDetailSM>
   */
  GetCompanyDetails = async (): Promise<ApiResponse<ClientCompanyDetailSM>> => {
    try {
      let resp = await this.GetResponseAsync<null, ClientCompanyDetailSM>(
        `${AppConstants.ApiUrls.COMPANY_DETAIL_URL}/my`,
        "GET"
      );
      return resp;
    } catch (error) {
      console.error('Error fetching company details:', error);
      throw error;
    }
  };

  /**
   * Get company detail by ID
   * @param companyId - Company ID
   * @returns ApiResponse<ClientCompanyDetailSM>
   */
  GetCompanyDetailById = async (
    companyId: number
  ): Promise<ApiResponse<ClientCompanyDetailSM>> => {
    if (!companyId || companyId <= 0) {
      throw new Error('Valid company ID is required');
    }
    try {
      let resp = await this.GetResponseAsync<number, ClientCompanyDetailSM>(
        `${AppConstants.ApiUrls.COMPANY_DETAIL_URL}/${companyId}`,
        "GET"
      );
      return resp;
    } catch (error) {
      console.error('Error fetching company detail by ID:', error);
      throw error;
    }
  };

  /**
   * Update company details
   * @param companyDetail - Company detail data
   * @returns ApiResponse<ClientCompanyDetailSM>
   */
  UpdateCompanyDetail = async (
    companyDetail: ApiRequest<ClientCompanyDetailSM>
  ): Promise<ApiResponse<ClientCompanyDetailSM>> => {
    if (!companyDetail || !companyDetail.reqData || !companyDetail.reqData.id) {
      throw new Error('Company detail with ID is required');
    }
    try {
      let resp = await this.GetResponseAsync<
        ClientCompanyDetailSM,
        ClientCompanyDetailSM
      >(
        `${AppConstants.ApiUrls.COMPANY_DETAIL_URL}/${companyDetail.reqData.id}`,
        "PUT",
        companyDetail
      );
      return resp;
    } catch (error) {
      console.error('Error updating company detail:', error);
      throw error;
    }
  };

  /**
   * Get company logo
   * @returns ApiResponse<any>
   */
  GetCompanyLogo = async (): Promise<ApiResponse<any>> => {
    try {
      let resp = await this.GetResponseAsync<null, any>(
        `${AppConstants.ApiUrls.COMPANY_DETAIL_URL}/mine/Logo`,
        "GET"
      );
      return resp;
    } catch (error) {
      console.error('Error fetching company logo:', error);
      throw error;
    }
  };

  /**
   * Upload company logo
   * @param logoFile - Logo file data
   * @returns ApiResponse<any>
   */
  UploadCompanyLogo = async (
    logoFile: ApiRequest<any>
  ): Promise<ApiResponse<any>> => {
    if (!logoFile || !logoFile.reqData) {
      throw new Error('Logo file data is required');
    }
    try {
      let resp = await this.GetResponseAsync<any, any>(
        `${AppConstants.ApiUrls.COMPANY_DETAIL_URL}/mine/CompanyLogo`,
        "POST",
        logoFile
      );
      return resp;
    } catch (error) {
      console.error('Error uploading company logo:', error);
      throw error;
    }
  };

  /**
   * Delete company logo
   * @returns ApiResponse<any>
   */
  DeleteCompanyLogo = async (): Promise<ApiResponse<any>> => {
    try {
      let resp = await this.GetResponseAsync<null, any>(
        `${AppConstants.ApiUrls.COMPANY_DETAIL_URL}/mine/DeleteCompanyLogo`,
        "DELETE"
      );
      return resp;
    } catch (error) {
      console.error('Error deleting company logo:', error);
      throw error;
    }
  };
}
