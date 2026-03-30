import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { CompanyHolidaysClient } from '../clients/company-holidays.client';
import { ClientCompanyHolidaysSM } from '../service-models/app/v1/client/client-company-holidays-s-m';
import { ApiResponse } from '../service-models/foundation/api-contracts/base/api-response';
import { ApiRequest } from '../service-models/foundation/api-contracts/base/api-request';

@Injectable({
  providedIn: 'root'
})
export class CompanyHolidaysService extends BaseService {
  constructor(private companyHolidaysClient: CompanyHolidaysClient) {
    super();
  }

  async getAllCompanyHolidays(): Promise<ApiResponse<ClientCompanyHolidaysSM[]>> {
    return await this.companyHolidaysClient.GetAllCompanyHolidays();
  }

  async getHolidayById(holidayId: number): Promise<ApiResponse<ClientCompanyHolidaysSM>> {
    if (!holidayId || holidayId <= 0) {
      throw new Error('Valid holiday ID is required');
    }
    return await this.companyHolidaysClient.GetHolidayById(holidayId);
  }

  async createHoliday(holiday: ClientCompanyHolidaysSM): Promise<ApiResponse<ClientCompanyHolidaysSM>> {
    if (!holiday) {
      throw new Error('Holiday data is required');
    }
    const apiRequest = new ApiRequest<ClientCompanyHolidaysSM>();
    apiRequest.reqData = holiday;
    return await this.companyHolidaysClient.CreateHoliday(apiRequest);
  }

  async updateHoliday(holidayId: number, holiday: ClientCompanyHolidaysSM): Promise<ApiResponse<ClientCompanyHolidaysSM>> {
    if (!holidayId || holidayId <= 0) {
      throw new Error('Valid holiday ID is required');
    }
    if (!holiday) {
      throw new Error('Holiday data is required');
    }
    const apiRequest = new ApiRequest<ClientCompanyHolidaysSM>();
    apiRequest.reqData = holiday;
    return await this.companyHolidaysClient.UpdateHoliday(holidayId, apiRequest);
  }

  async deleteHoliday(holidayId: number): Promise<ApiResponse<any>> {
    if (!holidayId || holidayId <= 0) {
      throw new Error('Valid holiday ID is required');
    }
    return await this.companyHolidaysClient.DeleteHoliday(holidayId);
  }
}
