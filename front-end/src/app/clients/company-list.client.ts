import { Injectable } from "@angular/core";
import { AppConstants } from "src/app-constants";
import { ClientCompanyDetailSM } from "../service-models/app/v1/client/client-company-detail-s-m";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { QueryFilter } from "../service-models/foundation/api-contracts/query-filter";
import { DeleteResponseRoot } from "../service-models/foundation/common-response/delete-response-root";
import { IntResponseRoot } from "../service-models/foundation/common-response/int-response-root";
import { StorageService } from "../services/storage.service";
import { BaseApiClient } from "./base-client/base-api.client";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { StorageCache } from "./helpers/storage-cache.helper";

@Injectable({
  providedIn: "root",
})
export class CompanyListClient extends BaseApiClient {
  constructor(
    storageService: StorageService,
    storageCache: StorageCache,
    commonResponseCodeHandler: CommonResponseCodeHandler
  ) {
    super(storageService, storageCache, commonResponseCodeHandler);
  }
  GetAllCompanieslistByOdata = async (queryFilter: QueryFilter,): Promise<ApiResponse<ClientCompanyDetailSM[]>> => {
    let finalUrl = this.ApplyQueryFilterToUrl(`${AppConstants.ApiUrls.COMPANY_DETAILS_URL}/odata`, queryFilter);
    let resp = await this.GetResponseAsync<null, ClientCompanyDetailSM[]>(`${finalUrl}`, 'GET');
    return resp;
  }
  /**Calls Get API to fetch Company List data */
  GetAllCompanyList = async (): Promise<
    ApiResponse<ClientCompanyDetailSM[]>
  > => {
    let resp = await this.GetResponseAsync<null, ClientCompanyDetailSM[]>(
      `${AppConstants.ApiUrls.COMPANY_DETAILS_URL}`,
      "GET"
    );
    return resp;
  };

  /**Call Get API to fetch Total number of Companies Count*/
  GetAllCompaniesCount = async (): Promise<ApiResponse<IntResponseRoot>> => {
    let resp = await this.GetResponseAsync<null, IntResponseRoot>(
      `${AppConstants.ApiUrls.COMPANY_DETAILS_URL}/ClientCompanyDetailCountResponse`,
      "GET"
    );
    return resp;
  };
  /**Update Company status using PUT method */
  UpdateCompanyStatus = async (
    id: number,
    updateCompanyStatus: boolean
  ): Promise<ApiResponse<DeleteResponseRoot>> => {
    let resp = await this.GetResponseAsync<number, DeleteResponseRoot>(
      `${AppConstants.ApiUrls.SUPER_COMPANY_SETTING_URL}/CompanyEnableOrDisable/${id}/${updateCompanyStatus}`,
      "PUT"
    );
    return resp;
  };
}
