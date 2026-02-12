import { Injectable } from "@angular/core";
import { AppConstants } from "src/app-constants";
import { ClientEmployeeCTCDetailSM } from "../service-models/app/v1/client/client-employee-c-t-c-detail-s-m";
import { ClientGenericPayrollComponentSM } from "../service-models/app/v1/client/client-generic-payroll-component-s-m";

import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { DeleteResponseRoot } from "../service-models/foundation/common-response/delete-response-root";
import { StorageService } from "../services/storage.service";
import { BaseApiClient } from "./base-client/base-api.client";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { StorageCache } from "./helpers/storage-cache.helper";

@Injectable({
  providedIn: "root",
})
export class PayrollStructureClient extends BaseApiClient {
  constructor(
    storageService: StorageService,
    storageCache: StorageCache,
    commonResponseCodeHandler: CommonResponseCodeHandler
  ) {
    super(storageService, storageCache, commonResponseCodeHandler);
  }



  /**Get all Generic Payroll Components
   * @DEV : Musaib
  */
  GetAllGenericPayrollComponents = async (): Promise<ApiResponse<ClientGenericPayrollComponentSM[]>> => {
    let resp = await this.GetResponseAsync<number, ClientGenericPayrollComponentSM[]>
      (`${AppConstants.ApiUrls.GENERIC_COMPONENT_URL}`, "GET");
    return resp;
  };


  /**Get Generic Payroll Components by id 
   * @DEV : Musaib
  */
  GetGenericPayrollComponentsById = async (Id: number): Promise<ApiResponse<ClientGenericPayrollComponentSM>> => {
    let resp = await this.GetResponseAsync<number, ClientGenericPayrollComponentSM>(`${AppConstants.ApiUrls.GENERIC_COMPONENT_URL}/${Id}`, "GET");
    return resp;
  };


  /**Add a new Generic Payroll Components
   * @DEV : Musaib
  */
  AddGenericPayrollComponents = async (addGenericPayrollComponentsRequest: ApiRequest<ClientGenericPayrollComponentSM>): Promise<ApiResponse<ClientGenericPayrollComponentSM>> => {
    let resp = await this.GetResponseAsync<ClientGenericPayrollComponentSM, ClientGenericPayrollComponentSM>
      (`${AppConstants.ApiUrls.GENERIC_COMPONENT_URL}/my/GenericPayrollComponent`, "POST", addGenericPayrollComponentsRequest);
    return resp;
  };


  /**Update Generic Payroll Components Using PUT
   * @DEV : Musaib
  */
  UpdateGenericPayrollComponents = async (updateGenericPayrollComponentsRequest: ApiRequest<ClientGenericPayrollComponentSM>): Promise<ApiResponse<ClientGenericPayrollComponentSM>> => {
    let resp = await this.GetResponseAsync<ClientGenericPayrollComponentSM, ClientGenericPayrollComponentSM>
      (`${AppConstants.ApiUrls.GENERIC_COMPONENT_URL}/my/UpdateGenericPayrollComponent/${updateGenericPayrollComponentsRequest.reqData.id}`, "PUT", updateGenericPayrollComponentsRequest);
    return resp;
  };


  /**delete Generic Payroll Components by id using Delete
   * @DEV : Musaib
  */
  DeleteGenericPayrollComponentsById = async (Id: number): Promise<ApiResponse<DeleteResponseRoot>> => {
    let resp = await this.GetResponseAsync<number, DeleteResponseRoot>(`${AppConstants.ApiUrls.GENERIC_COMPONENT_URL}/${Id}`, "DELETE");
    return resp;
  };


  async GetClientEmployeeSalaryById(ctcId: number): Promise<ApiResponse<ClientEmployeeCTCDetailSM>> {
    let resp = await this.GetResponseAsync<number, ClientEmployeeCTCDetailSM>(`${AppConstants.ApiUrls.EMPLOYEE_SALARY_URL}/${ctcId}`, "GET");
    return resp;
  }

}
