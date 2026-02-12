import { Injectable } from "@angular/core";
import { AppConstants } from "src/app-constants";
import { DocumentsSM } from "../service-models/app/v1/client/documents-s-m";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { DeleteResponseRoot } from "../service-models/foundation/common-response/delete-response-root";
import { StorageService } from "../services/storage.service";
import { BaseApiClient } from "./base-client/base-api.client";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { StorageCache } from "./helpers/storage-cache.helper";


@Injectable({
    providedIn: 'root'
})


export class CompanyLetterClient extends BaseApiClient {
    constructor(storageService: StorageService, storageCache: StorageCache,
        commonResponseCodeHandler: CommonResponseCodeHandler) {
        super(storageService, storageCache, commonResponseCodeHandler)
    }


    /** Get All Company Letters  */
    GetAllCompanyLetters = async (): Promise<ApiResponse<DocumentsSM[]>> => {
        let resp = await this.GetResponseAsync<null, DocumentsSM[]>(`${AppConstants.ApiUrls.LETTER_URL}/my/AllPartialDocuments`, 'GET');
        return resp;
    }


    /** Get Company Letter By Id */
    GetCompanyLetterById = async (letterId: number): Promise<ApiResponse<DocumentsSM>> => {
        let resp = await this.GetResponseAsync<number, DocumentsSM>(`${AppConstants.ApiUrls.LETTER_URL}/${letterId}`, 'GET');
        return resp;
    }


    /** Add Company Letters */
    AddCompanyLetter = async (companyLetterReq: ApiRequest<DocumentsSM>): Promise<ApiResponse<DocumentsSM>> => {
        let resp = await this.GetResponseAsync<DocumentsSM, DocumentsSM>(`${AppConstants.ApiUrls.LETTER_URL}`, 'POST', companyLetterReq);
        return resp;
    }



    /** Delete Company Letter By LetterId */
    DeleteCompanyLetterById = async (letterId: number): Promise<ApiResponse<DeleteResponseRoot>> => {
        let resp = await this.GetResponseAsync<number, DeleteResponseRoot>(`${AppConstants.ApiUrls.LETTER_URL}/${letterId}`, 'DELETE');
        return resp;
    }




}
