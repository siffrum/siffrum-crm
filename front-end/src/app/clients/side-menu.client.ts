import { Injectable } from "@angular/core";
import { BaseApiClient } from "./base-client/base-api.client";
import { StorageService } from "../services/storage.service";
import { StorageCache } from "./helpers/storage-cache.helper";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { AppConstants } from "src/app-constants";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { CompanyModulesSM } from "../service-models/app/v1/client/company-modules-s-m";
import { ClientThemeSM } from "../service-models/app/v1/client/client-theme-s-m";

@Injectable({
  providedIn: "root",
})
export class SideMenuClient extends BaseApiClient {
  constructor(
    storageService: StorageService,
    storageCache: StorageCache,
    commonResponseCodeHandler: CommonResponseCodeHandler
  ) {
    super(storageService, storageCache, commonResponseCodeHandler);
  }

  /**Get Profile picture
   * @DEV : Musaib
   */
  GetProfilePictureOfTheUser = async (): Promise<ApiResponse<string>> => {
    let resp = await this.GetResponseAsync<null, string>(
      `${AppConstants.ApiUrls.EMPLOYEE_URL}/mine/ProfilePicture`,
      "GET"
    );
    return resp;
  };

  /**Add a new User Profile Picture
   * @DEV : Musaib
   */
  AddUserProfilePicture = async (
    addUserProfilePictureRequest: ApiRequest<string>
  ): Promise<ApiResponse<string>> => {
    let resp = await this.GetResponseAsync<string, string>(
      `${AppConstants.ApiUrls.EMPLOYEE_URL}/mine/ProfilePicture`,
      "POST",
      addUserProfilePictureRequest
    );
    return resp;
  };

  /** Get Company Details   By Company Id (GET method) */
  GetCompanyModules = async (): Promise<ApiResponse<CompanyModulesSM>> => {
    let resp = await this.GetResponseAsync<number, CompanyModulesSM>(
      `${AppConstants.ApiUrls.SUPER_COMPANY_SETTING_URL}/mine/Modules`,
      "GET"
    );
    return resp;
  };

}
