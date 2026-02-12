import { Injectable } from "@angular/core";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { BaseApiClient } from "./base-client/base-api.client";
import { StorageService } from "../services/storage.service";
import { StorageCache } from "./helpers/storage-cache.helper";
import { ClientCompanyDepartmentSM } from "../service-models/app/v1/client/client-company-department-s-m";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { AppConstants } from "src/app-constants";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { DeleteResponseRoot } from "../service-models/foundation/common-response/delete-response-root";

@Injectable({
    providedIn: "root",
  })
  export class CompanyDepartmentClient extends BaseApiClient {
    constructor(
      storageService: StorageService,
      storageCache: StorageCache,
      commonResponseCodeHandler: CommonResponseCodeHandler
    ) {
      super(storageService, storageCache, commonResponseCodeHandler);
    }
     /**  Get Company DEPARTMENTS
      * @DEV Musaib
   * @param Id
   */
  GetCompanyDepartments = async (): Promise<
  ApiResponse<ClientCompanyDepartmentSM[]>
> => {
  let resp = await this.GetResponseAsync<
    null,
    ClientCompanyDepartmentSM[]
  >(
    `${AppConstants.ApiUrls.COMPANY_DEPARTMENTS}/my/CompanyDepartments`,
    "GET"
  );
  return resp;
};

  /** Get Company Department Details  By Id (GET method)
   * @param Id
   */
  GetCompanyDepartmentById = async (
    Id: number
  ): Promise<ApiResponse<ClientCompanyDepartmentSM>> => {
    let resp = await this.GetResponseAsync<
      number,
      ClientCompanyDepartmentSM
    >(`${AppConstants.ApiUrls.COMPANY_DEPARTMENTS}/${Id}`, "GET");
    return resp;
  };
  /**Update Company Department Details By Id (PUT method)*/
  UpdateCompanyDepartmentDetails = async (
    updateDepartmentRequest: ApiRequest<ClientCompanyDepartmentSM>
  ): Promise<ApiResponse<ClientCompanyDepartmentSM>> => {
    let resp = await this.GetResponseAsync<
      ClientCompanyDepartmentSM,
      ClientCompanyDepartmentSM
    >(
      `${AppConstants.ApiUrls.COMPANY_DEPARTMENTS}/my/${updateDepartmentRequest.reqData.id}`,
      "PUT",
      updateDepartmentRequest
    );
    return resp;
  };
  /** Add New  Company Department Details
   * (POST method)
   */
  AddNewCompanyDepartmentDetails = async (
    addDepartment: ApiRequest<ClientCompanyDepartmentSM>
  ): Promise<ApiResponse<ClientCompanyDepartmentSM>> => {
    let resp = await this.GetResponseAsync<
      ClientCompanyDepartmentSM,
      ClientCompanyDepartmentSM
    >(`${AppConstants.ApiUrls.COMPANY_DEPARTMENTS}/my`, "POST", addDepartment);
    return resp;
  };

  /**delete company shift by id using Delete
   * @DEV : Musaib
   */
  DeleteCompanyDepartmentById = async (
    Id: number
  ): Promise<ApiResponse<DeleteResponseRoot>> => {
    let resp = await this.GetResponseAsync<number, DeleteResponseRoot>(
      `${AppConstants.ApiUrls.COMPANY_DEPARTMENTS}/${Id}`,
      "DELETE"
    );
    return resp;
  };
}