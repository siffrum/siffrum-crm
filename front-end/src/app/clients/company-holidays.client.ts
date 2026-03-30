import { Injectable } from "@angular/core";
import { BaseApiClient } from "./base-client/base-api.client";
import { StorageService } from "../services/storage.service";
import { StorageCache } from "./helpers/storage-cache.helper";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { AppConstants } from "src/app-constants";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { ClientCompanyHolidaysSM } from "../service-models/app/v1/client/client-company-holidays-s-m";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";

@Injectable({
  providedIn: "root",
})
export class CompanyHolidaysClient extends BaseApiClient {
  constructor(
    storageService: StorageService,
    storageCache: StorageCache,
    commonResponseCodeHandler: CommonResponseCodeHandler
  ) {
    super(storageService, storageCache, commonResponseCodeHandler);
  }

  /**
   * Get all company holidays
   * @returns ApiResponse<ClientCompanyHolidaysSM[]>
   */
  GetAllCompanyHolidays = async (): Promise<
    ApiResponse<ClientCompanyHolidaysSM[]>
  > => {
    try {
      let resp = await this.GetResponseAsync<null, ClientCompanyHolidaysSM[]>(
        `${AppConstants.ApiUrls.COMPANY_HOLIDAYS_URL}/my`,
        "GET"
      );
      return resp;
    } catch (error) {
      console.error('Error fetching company holidays:', error);
      throw error;
    }
  };

  /**
   * Get holiday by ID
   * @param holidayId - Holiday ID
   * @returns ApiResponse<ClientCompanyHolidaysSM>
   */
  GetHolidayById = async (
    holidayId: number
  ): Promise<ApiResponse<ClientCompanyHolidaysSM>> => {
    if (!holidayId || holidayId <= 0) {
      throw new Error('Valid holiday ID is required');
    }
    try {
      let resp = await this.GetResponseAsync<number, ClientCompanyHolidaysSM>(
        `${AppConstants.ApiUrls.COMPANY_HOLIDAYS_URL}/${holidayId}`,
        "GET"
      );
      return resp;
    } catch (error) {
      console.error('Error fetching holiday by ID:', error);
      throw error;
    }
  };

  /**
   * Create new holiday
   * @param holiday - Holiday data
   * @returns ApiResponse<ClientCompanyHolidaysSM>
   */
  CreateHoliday = async (
    holiday: ApiRequest<ClientCompanyHolidaysSM>
  ): Promise<ApiResponse<ClientCompanyHolidaysSM>> => {
    if (!holiday || !holiday.reqData) {
      throw new Error('Holiday data is required');
    }
    try {
      let resp = await this.GetResponseAsync<
        ClientCompanyHolidaysSM,
        ClientCompanyHolidaysSM
      >(
        `${AppConstants.ApiUrls.COMPANY_HOLIDAYS_URL}/my`,
        "POST",
        holiday
      );
      return resp;
    } catch (error) {
      console.error('Error creating holiday:', error);
      throw error;
    }
  };

  /**
   * Update holiday
   * @param holidayId - Holiday ID
   * @param holiday - Holiday data
   * @returns ApiResponse<ClientCompanyHolidaysSM>
   */
  UpdateHoliday = async (
    holidayId: number,
    holiday: ApiRequest<ClientCompanyHolidaysSM>
  ): Promise<ApiResponse<ClientCompanyHolidaysSM>> => {
    if (!holidayId || holidayId <= 0) {
      throw new Error('Valid holiday ID is required');
    }
    if (!holiday || !holiday.reqData) {
      throw new Error('Holiday data is required');
    }
    try {
      let resp = await this.GetResponseAsync<
        ClientCompanyHolidaysSM,
        ClientCompanyHolidaysSM
      >(
        `${AppConstants.ApiUrls.COMPANY_HOLIDAYS_URL}/my/${holidayId}`,
        "PUT",
        holiday
      );
      return resp;
    } catch (error) {
      console.error('Error updating holiday:', error);
      throw error;
    }
  };

  /**
   * Delete holiday
   * @param holidayId - Holiday ID
   * @returns ApiResponse<any>
   */
  DeleteHoliday = async (holidayId: number): Promise<ApiResponse<any>> => {
    if (!holidayId || holidayId <= 0) {
      throw new Error('Valid holiday ID is required');
    }
    try {
      let resp = await this.GetResponseAsync<number, any>(
        `${AppConstants.ApiUrls.COMPANY_HOLIDAYS_URL}/my/${holidayId}`,
        "DELETE"
      );
      return resp;
    } catch (error) {
      console.error('Error deleting holiday:', error);
      throw error;
    }
  };
}
