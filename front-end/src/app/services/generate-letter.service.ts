import { Injectable } from "@angular/core";
import { GenerateLetterClient } from "../clients/generate-letter.client";
import { DocumentsSM } from "../service-models/app/v1/client/documents-s-m";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { BaseService } from "./base.service";
import { CommonService } from "./common.service";


@Injectable({
    providedIn: 'root'
})


export class GenerateLetterService extends BaseService {
    constructor(
        private commonService: CommonService,
        private generateLetterClient: GenerateLetterClient,
    ) {
        super();
    }


    async getLetterList(): Promise<ApiResponse<DocumentsSM[]>> {
        return await this.generateLetterClient.GetEmployeeLetterList();
    }


    async downloadEmployeeLetter(employeeId: number, letterId: number): Promise<ApiResponse<DocumentsSM>> {
        return await this.generateLetterClient.DownloadEmployeeLetter(employeeId, letterId);
    }


    async GetPaySlipByDate(empId:number,date:Date): Promise<ApiResponse<string>> {
        return await this.generateLetterClient.GetPaySlipByDate(empId,date);
      }


}