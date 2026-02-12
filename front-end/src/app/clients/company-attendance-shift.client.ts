import { Injectable } from "@angular/core";
import { BaseApiClient } from "./base-client/base-api.client";
import { StorageService } from "../services/storage.service";
import { StorageCache } from "./helpers/storage-cache.helper";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { DeleteResponseRoot } from "../service-models/foundation/common-response/delete-response-root";
import { AppConstants } from "src/app-constants";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { ClientCompanyAttendanceShiftSM } from "../service-models/app/v1/client/client-company-attendance-shift-s-m";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";

@Injectable({
  providedIn: "root",
})
export class CompanyAttendanceShift extends BaseApiClient {
  constructor(
    storageService: StorageService,
    storageCache: StorageCache,
    commonResponseCodeHandler: CommonResponseCodeHandler
  ) {
    super(storageService, storageCache, commonResponseCodeHandler);
  }

  /**  Get Company Attendance Shift Details  By Company Id (GET method)
   * @param Id
   */
  GetAllCompanyAttendanceShiftDetails = async (): Promise<
    ApiResponse<ClientCompanyAttendanceShiftSM[]>
  > => {
    let resp = await this.GetResponseAsync<
      null,
      ClientCompanyAttendanceShiftSM[]
    >(
      `${AppConstants.ApiUrls.COMPANY_ATTENDANCE_SHIFT}/my/CompanyAttendanceShift`,
      "GET"
    );
    return resp;
  };
  /** Get Company Shift Details  By Id (GET method)
   * @param Id
   */
  GetCompanyShiftById = async (
    Id: number
  ): Promise<ApiResponse<ClientCompanyAttendanceShiftSM>> => {
    let resp = await this.GetResponseAsync<
      number,
      ClientCompanyAttendanceShiftSM
    >(`${AppConstants.ApiUrls.COMPANY_ATTENDANCE_SHIFT}/${Id}`, "GET");
    return resp;
  };
  /**Update Company Shift Details By Id (PUT method)*/
  UpdateCompanyAttendanceShiftDetails = async (
    updateShiftRequest: ApiRequest<ClientCompanyAttendanceShiftSM>
  ): Promise<ApiResponse<ClientCompanyAttendanceShiftSM>> => {
    let resp = await this.GetResponseAsync<
      ClientCompanyAttendanceShiftSM,
      ClientCompanyAttendanceShiftSM
    >(
      `${AppConstants.ApiUrls.COMPANY_ATTENDANCE_SHIFT}/${updateShiftRequest.reqData.id}`,
      "PUT",
      updateShiftRequest
    );
    return resp;
  };
  /** Add New  Company Shift Details
   * (POST method)
   */
  AddNewCompanyAttendanceShiftDetails = async (
    addShift: ApiRequest<ClientCompanyAttendanceShiftSM>
  ): Promise<ApiResponse<ClientCompanyAttendanceShiftSM>> => {
    let resp = await this.GetResponseAsync<
      ClientCompanyAttendanceShiftSM,
      ClientCompanyAttendanceShiftSM
    >(`${AppConstants.ApiUrls.COMPANY_ATTENDANCE_SHIFT}`, "POST", addShift);
    return resp;
  };

  /**delete company shift by id using Delete
   * @DEV : Musaib
   */
  DeleteCompanyShiftById = async (
    Id: number
  ): Promise<ApiResponse<DeleteResponseRoot>> => {
    let resp = await this.GetResponseAsync<number, DeleteResponseRoot>(
      `${AppConstants.ApiUrls.COMPANY_ATTENDANCE_SHIFT}/${Id}`,
      "DELETE"
    );
    return resp;
  };
}
