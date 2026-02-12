import { Injectable } from "@angular/core";
import { BaseApiClient } from "./base-client/base-api.client";
import { StorageService } from "../services/storage.service";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { StorageCache } from "./helpers/storage-cache.helper";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { AppConstants } from "src/app-constants";
import { DashBoardSM } from "../service-models/app/v1/client/dash-board-s-m";

@Injectable({
    providedIn: 'root'
})


export class AdminDashboardClient extends BaseApiClient {
    constructor(storageService: StorageService, storageCache: StorageCache,
        commonResponseCodeHandler: CommonResponseCodeHandler) {
        super(storageService, storageCache, commonResponseCodeHandler)
    }
  /**
   * @Dev Musaib
   * Get All Data For Dashboard
   * @returns
   */
    GetAllDasBoardDataItemsList = async (): Promise<ApiResponse<DashBoardSM>> => {
    let resp = await this.GetResponseAsync<null, DashBoardSM>(
      `${AppConstants.ApiUrls.USER_API_URL}/DashBoardDetails`,
      "GET"
    );
    return resp;
  };
}