import { Injectable } from "@angular/core";
import { CompanyLetterClient } from "../clients/company-letter.client";
import { DocumentsSM } from "../service-models/app/v1/client/documents-s-m";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { DeleteResponseRoot } from "../service-models/foundation/common-response/delete-response-root";
import { BaseService } from "./base.service";


@Injectable({
    providedIn: "root",
})


export class CompanyLetterService extends BaseService {
    constructor(
        private companyLetterClient: CompanyLetterClient,
    ) {
        super();
    }

    /** Get All Company Letters  */
    async getAllCompanyLetters(): Promise<ApiResponse<DocumentsSM[]>> {
        return await this.companyLetterClient.GetAllCompanyLetters();
    }


    /** Get Employee Documents-Info By Document Id */
    async getCompanyLetterById(letterId: number): Promise<ApiResponse<DocumentsSM>> {
        return await this.companyLetterClient.GetCompanyLetterById(letterId);
    }


    /** Add Company Letters */
    async AddCompanyLetter(companyLetterReq: DocumentsSM): Promise<ApiResponse<DocumentsSM>> {
        let apiRequest = new ApiRequest<DocumentsSM>();
        apiRequest.reqData = companyLetterReq;
        return await this.companyLetterClient.AddCompanyLetter(apiRequest);
    }


    /** Delete Company Letter By Letter Id */
    async deleteCompanyLetterById(letterId: number): Promise<ApiResponse<DeleteResponseRoot>> {
        return await this.companyLetterClient.DeleteCompanyLetterById(letterId);
    }



}