import { Injectable } from "@angular/core";
import { BaseApiClient } from "./base-client/base-api.client";
import { StorageService } from "../services/storage.service";
import { StorageCache } from "./helpers/storage-cache.helper";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { AppConstants } from "src/app-constants";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { ClientGenericPayrollComponentSM } from "../service-models/app/v1/client/client-generic-payroll-component-s-m";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";

@Injectable({
  providedIn: "root",
})
export class GenericPayrollClient extends BaseApiClient {
  constructor(
    storageService: StorageService,
    storageCache: StorageCache,
    commonResponseCodeHandler: CommonResponseCodeHandler
  ) {
    super(storageService, storageCache, commonResponseCodeHandler);
  }

  /**
   * Get all generic payroll components
   * @returns ApiResponse<ClientGenericPayrollComponentSM[]>
   */
  GetAllComponents = async (): Promise<
    ApiResponse<ClientGenericPayrollComponentSM[]>
  > => {
    try {
      let resp = await this.GetResponseAsync<
        null,
        ClientGenericPayrollComponentSM[]
      >(
        `${AppConstants.ApiUrls.GENERIC_PAYROLL_COMPONENT_URL}/my/AllGenericPayrollComponents`,
        "GET"
      );
      return resp;
    } catch (error) {
      console.error('Error fetching generic payroll components:', error);
      throw error;
    }
  };

  /**
   * Get component by ID
   * @param componentId - Component ID
   * @returns ApiResponse<ClientGenericPayrollComponentSM>
   */
  GetComponentById = async (
    componentId: number
  ): Promise<ApiResponse<ClientGenericPayrollComponentSM>> => {
    if (!componentId || componentId <= 0) {
      throw new Error('Valid component ID is required');
    }
    try {
      let resp = await this.GetResponseAsync<
        number,
        ClientGenericPayrollComponentSM
      >(
        `${AppConstants.ApiUrls.GENERIC_PAYROLL_COMPONENT_URL}/${componentId}`,
        "GET"
      );
      return resp;
    } catch (error) {
      console.error('Error fetching component by ID:', error);
      throw error;
    }
  };

  /**
   * Create new payroll component
   * @param component - Component data
   * @returns ApiResponse<ClientGenericPayrollComponentSM>
   */
  CreateComponent = async (
    component: ApiRequest<ClientGenericPayrollComponentSM>
  ): Promise<ApiResponse<ClientGenericPayrollComponentSM>> => {
    if (!component || !component.reqData) {
      throw new Error('Component data is required');
    }
    try {
      let resp = await this.GetResponseAsync<
        ClientGenericPayrollComponentSM,
        ClientGenericPayrollComponentSM
      >(
        `${AppConstants.ApiUrls.GENERIC_PAYROLL_COMPONENT_URL}/my/GenericPayrollComponent`,
        "POST",
        component
      );
      return resp;
    } catch (error) {
      console.error('Error creating payroll component:', error);
      throw error;
    }
  };

  /**
   * Update payroll component
   * @param componentId - Component ID
   * @param component - Component data
   * @returns ApiResponse<ClientGenericPayrollComponentSM>
   */
  UpdateComponent = async (
    componentId: number,
    component: ApiRequest<ClientGenericPayrollComponentSM>
  ): Promise<ApiResponse<ClientGenericPayrollComponentSM>> => {
    if (!componentId || componentId <= 0) {
      throw new Error('Valid component ID is required');
    }
    if (!component || !component.reqData) {
      throw new Error('Component data is required');
    }
    try {
      let resp = await this.GetResponseAsync<
        ClientGenericPayrollComponentSM,
        ClientGenericPayrollComponentSM
      >(
        `${AppConstants.ApiUrls.GENERIC_PAYROLL_COMPONENT_URL}/my/GenericPayrollComponent/${componentId}`,
        "PUT",
        component
      );
      return resp;
    } catch (error) {
      console.error('Error updating payroll component:', error);
      throw error;
    }
  };

  /**
   * Delete payroll component
   * @param componentId - Component ID
   * @returns ApiResponse<any>
   */
  DeleteComponent = async (componentId: number): Promise<ApiResponse<any>> => {
    if (!componentId || componentId <= 0) {
      throw new Error('Valid component ID is required');
    }
    try {
      let resp = await this.GetResponseAsync<number, any>(
        `${AppConstants.ApiUrls.GENERIC_PAYROLL_COMPONENT_URL}/my/GenericPayrollComponent/${componentId}`,
        "DELETE"
      );
      return resp;
    } catch (error) {
      console.error('Error deleting payroll component:', error);
      throw error;
    }
  };
}
