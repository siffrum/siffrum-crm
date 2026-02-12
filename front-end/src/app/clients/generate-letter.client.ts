import { Injectable } from "@angular/core";
import { AppConstants } from "src/app-constants";
import { DocumentsSM } from "../service-models/app/v1/client/documents-s-m";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { StorageService } from "../services/storage.service";
import { BaseApiClient } from "./base-client/base-api.client";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { StorageCache } from "./helpers/storage-cache.helper";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";


@Injectable({
    providedIn: 'root'
})


export class GenerateLetterClient extends BaseApiClient {
    constructor(storageService: StorageService, storageCache: StorageCache,
        commonResponseCodeHandler: CommonResponseCodeHandler) {
        super(storageService, storageCache, commonResponseCodeHandler)
    }


    /** Get Letter List */
    GetEmployeeLetterList = async (): Promise<ApiResponse<DocumentsSM[]>> => {
        let resp = await this.GetResponseAsync<null, DocumentsSM[]>(`${AppConstants.ApiUrls.LETTER_URL}/my/AllPartialDocuments`, 'GET');
        return resp;
    }


    /** Download Employee Letter */
    DownloadEmployeeLetter = async (employeeId: number, letterId: number): Promise<ApiResponse<DocumentsSM>> => {
        let resp = await this.GetResponseAsync<number, DocumentsSM>(`${AppConstants.ApiUrls.LETTER_URL}/GenerateLetterForEmployee/${employeeId}/${letterId}`, 'GET');
        return resp;
    }



    GetPaySlipByDate = async (emplId:number,date:Date): Promise<
      ApiResponse<string>
    > => {
      let resp = await this.GetResponseAsync<null, string>(
        `${AppConstants.ApiUrls.PAYROLL_TRANSACTION_URL}/my/PaySlips/${emplId}/${date}`,
        "GET"
      );
      return resp;
    };

}