import { Injectable } from "@angular/core";
import { StorageService } from "../services/storage.service";
import { BaseApiClient } from "./base-client/base-api.client";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { StorageCache } from "./helpers/storage-cache.helper";
import { ClientThemeSM } from "../service-models/app/v1/client/client-theme-s-m";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { AppConstants } from "src/app-constants";
import { AdditionalRequestDetails, Authentication } from "../internal-models/additional-request-details";
import { BoolResponseRoot } from "../service-models/foundation/common-response/bool-response-root";

@Injectable({
  providedIn: "root",
})
export class SettingClient extends BaseApiClient {
  constructor(
    storageService: StorageService,
    storageCache: StorageCache,
    commonResponseCodeHandler: CommonResponseCodeHandler
  ) {
    super(storageService, storageCache, commonResponseCodeHandler);
  }
  GetAllClientThemes = async (): Promise<ApiResponse<ClientThemeSM[]>> => {
    let resp = await this.GetResponseAsync<null, ClientThemeSM[]>( `${AppConstants.ApiUrls.THEME_API_URL}`, "GET" );
    return resp;
  };
  /** Get theme */
  GetClientTheme = async (): Promise<ApiResponse<ClientThemeSM>> => {
    let resp = await this.GetResponseAsync<null, ClientThemeSM>(`${AppConstants.ApiUrls.THEME_API_URL}/mine/Theme`,"GET" );
    return resp;
  };
  /**Get Default Theme */
  GetClientDefaultTheme = async (): Promise<ApiResponse<ClientThemeSM>> => {
    let resp = await this.GetResponseAsync<null, ClientThemeSM>(`${AppConstants.ApiUrls.THEME_API_URL}/DefaultTheme`,"GET", null,
      new AdditionalRequestDetails<ClientThemeSM>(false, Authentication.false ));
    return resp;
  };

  UpdateTheme = async (themeId:number): Promise<ApiResponse<BoolResponseRoot>> => {
    let resp = await this.GetResponseAsync<null, BoolResponseRoot>(`${AppConstants.ApiUrls.USER_API_URL}/mine/Theme/${themeId}`, 'PUT')
    return resp;
}

}
