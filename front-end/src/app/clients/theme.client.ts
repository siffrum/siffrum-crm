import { Injectable } from "@angular/core";
import { BaseApiClient } from "./base-client/base-api.client";
import { StorageService } from "../services/storage.service";
import { StorageCache } from "./helpers/storage-cache.helper";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { AppConstants } from "src/app-constants";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { ClientThemeSM } from "../service-models/app/v1/client/client-theme-s-m";

@Injectable({
  providedIn: "root",
})
export class ThemeClient extends BaseApiClient {
  constructor(
    storageService: StorageService,
    storageCache: StorageCache,
    commonResponseCodeHandler: CommonResponseCodeHandler
  ) {
    super(storageService, storageCache, commonResponseCodeHandler);
  }

  /**
   * Get all available themes
   * @returns ApiResponse<ClientThemeSM[]>
   */
  GetAllThemes = async (): Promise<ApiResponse<ClientThemeSM[]>> => {
    try {
      let resp = await this.GetResponseAsync<null, ClientThemeSM[]>(
        `${AppConstants.ApiUrls.THEME_URL}`,
        "GET"
      );
      return resp;
    } catch (error) {
      console.error('Error fetching themes:', error);
      throw error;
    }
  };

  /**
   * Get default theme
   * @returns ApiResponse<ClientThemeSM>
   */
  GetDefaultTheme = async (): Promise<ApiResponse<ClientThemeSM>> => {
    try {
      let resp = await this.GetResponseAsync<null, ClientThemeSM>(
        `${AppConstants.ApiUrls.THEME_URL}/DefaultTheme`,
        "GET"
      );
      return resp;
    } catch (error) {
      console.error('Error fetching default theme:', error);
      throw error;
    }
  };

  /**
   * Get user's theme
   * @returns ApiResponse<ClientThemeSM>
   */
  GetUserTheme = async (): Promise<ApiResponse<ClientThemeSM>> => {
    try {
      let resp = await this.GetResponseAsync<null, ClientThemeSM>(
        `${AppConstants.ApiUrls.THEME_URL}/mine/Theme`,
        "GET"
      );
      return resp;
    } catch (error) {
      console.error('Error fetching user theme:', error);
      throw error;
    }
  };
}
