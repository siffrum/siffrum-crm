import { Injectable } from "@angular/core";
import { BaseApiClient } from "./base-client/base-api.client";
import { StorageService } from "../services/storage.service";
import { StorageCache } from "./helpers/storage-cache.helper";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { AppConstants } from "src/app-constants";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { ClientEmployeeAdditionalReimbursementLogSM } from "../service-models/app/v1/client/client-employee-additional-reimbursement-log-s-m";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";

@Injectable({
  providedIn: "root",
})
export class EmployeeAdditionalReimbursementClient extends BaseApiClient {
  constructor(
    storageService: StorageService,
    storageCache: StorageCache,
    commonResponseCodeHandler: CommonResponseCodeHandler
  ) {
    super(storageService, storageCache, commonResponseCodeHandler);
  }

  /**
   * Get all reimbursement logs for logged-in employee
   * @returns ApiResponse<ClientEmployeeAdditionalReimbursementLogSM[]>
   */
  GetMyReimbursementLogs = async (): Promise<
    ApiResponse<ClientEmployeeAdditionalReimbursementLogSM[]>
  > => {
    try {
      let resp = await this.GetResponseAsync<
        null,
        ClientEmployeeAdditionalReimbursementLogSM[]
      >(
        `${AppConstants.ApiUrls.EMPLOYEE_ADDITIONAL_REIMBURSEMENT_URL}/mine/AdditionalReimbursementDetails`,
        "GET"
      );
      return resp;
    } catch (error) {
      console.error('Error fetching reimbursement logs:', error);
      throw error;
    }
  };

  /**
   * Get reimbursement logs by employee ID
   * @param employeeId - Employee ID
   * @returns ApiResponse<ClientEmployeeAdditionalReimbursementLogSM[]>
   */
  GetReimbursementLogsByEmployeeId = async (
    employeeId: number
  ): Promise<ApiResponse<ClientEmployeeAdditionalReimbursementLogSM[]>> => {
    if (!employeeId || employeeId <= 0) {
      throw new Error('Valid employee ID is required');
    }
    try {
      let resp = await this.GetResponseAsync<
        number,
        ClientEmployeeAdditionalReimbursementLogSM[]
      >(
        `${AppConstants.ApiUrls.EMPLOYEE_ADDITIONAL_REIMBURSEMENT_URL}/EmployeeId/${employeeId}`,
        "GET"
      );
      return resp;
    } catch (error) {
      console.error('Error fetching reimbursement logs by employee ID:', error);
      throw error;
    }
  };

  /**
   * Create new reimbursement log
   * @param reimbursement - Reimbursement data
   * @returns ApiResponse<ClientEmployeeAdditionalReimbursementLogSM>
   */
  CreateReimbursement = async (
    reimbursement: ApiRequest<ClientEmployeeAdditionalReimbursementLogSM>
  ): Promise<ApiResponse<ClientEmployeeAdditionalReimbursementLogSM>> => {
    if (!reimbursement || !reimbursement.reqData) {
      throw new Error('Reimbursement data is required');
    }
    try {
      let resp = await this.GetResponseAsync<
        ClientEmployeeAdditionalReimbursementLogSM,
        ClientEmployeeAdditionalReimbursementLogSM
      >(
        `${AppConstants.ApiUrls.EMPLOYEE_ADDITIONAL_REIMBURSEMENT_URL}`,
        "POST",
        reimbursement
      );
      return resp;
    } catch (error) {
      console.error('Error creating reimbursement:', error);
      throw error;
    }
  };

  /**
   * Update reimbursement log
   * @param reimbursementId - Reimbursement ID
   * @param reimbursement - Reimbursement data
   * @returns ApiResponse<ClientEmployeeAdditionalReimbursementLogSM>
   */
  UpdateReimbursement = async (
    reimbursementId: number,
    reimbursement: ApiRequest<ClientEmployeeAdditionalReimbursementLogSM>
  ): Promise<ApiResponse<ClientEmployeeAdditionalReimbursementLogSM>> => {
    if (!reimbursementId || reimbursementId <= 0) {
      throw new Error('Valid reimbursement ID is required');
    }
    if (!reimbursement || !reimbursement.reqData) {
      throw new Error('Reimbursement data is required');
    }
    try {
      let resp = await this.GetResponseAsync<
        ClientEmployeeAdditionalReimbursementLogSM,
        ClientEmployeeAdditionalReimbursementLogSM
      >(
        `${AppConstants.ApiUrls.EMPLOYEE_ADDITIONAL_REIMBURSEMENT_URL}/${reimbursementId}`,
        "PUT",
        reimbursement
      );
      return resp;
    } catch (error) {
      console.error('Error updating reimbursement:', error);
      throw error;
    }
  };

  /**
   * Delete reimbursement log
   * @param reimbursementId - Reimbursement ID
   * @returns ApiResponse<any>
   */
  DeleteReimbursement = async (
    reimbursementId: number
  ): Promise<ApiResponse<any>> => {
    if (!reimbursementId || reimbursementId <= 0) {
      throw new Error('Valid reimbursement ID is required');
    }
    try {
      let resp = await this.GetResponseAsync<number, any>(
        `${AppConstants.ApiUrls.EMPLOYEE_ADDITIONAL_REIMBURSEMENT_URL}/my/EmployeeAdditionalReimbursementLog/${reimbursementId}`,
        "DELETE"
      );
      return resp;
    } catch (error) {
      console.error('Error deleting reimbursement:', error);
      throw error;
    }
  };
}
