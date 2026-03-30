import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { CompanyAttendanceShift } from '../clients/company-attendance-shift.client';
import { ClientCompanyAttendanceShiftSM } from '../service-models/app/v1/client/client-company-attendance-shift-s-m';
import { ApiResponse } from '../service-models/foundation/api-contracts/base/api-response';
import { ApiRequest } from '../service-models/foundation/api-contracts/base/api-request';
import { DeleteResponseRoot } from '../service-models/foundation/common-response/delete-response-root';
import { AppConstants } from 'src/app-constants';

@Injectable({
  providedIn: 'root'
})
export class CompanyAttendanceShiftService extends BaseService {

  constructor(private companyAttendanceShiftClient: CompanyAttendanceShift) {
    super();
  }

  /**
   * Get all company attendance shift details
   * @returns ApiResponse<ClientCompanyAttendanceShiftSM[]>
   */
  async getAllCompanyAttendanceShiftDetails(): Promise<ApiResponse<ClientCompanyAttendanceShiftSM[]>> {
    return await this.companyAttendanceShiftClient.GetAllCompanyAttendanceShiftDetails();
  }

  /**
   * Get shift details by ID
   * @param id Shift ID
   * @returns ApiResponse<ClientCompanyAttendanceShiftSM>
   */
  async getCompanyShiftById(id: number): Promise<ApiResponse<ClientCompanyAttendanceShiftSM>> {
    if (!id || id <= 0) {
      throw new Error('Valid shift ID is required');
    }
    return await this.companyAttendanceShiftClient.GetCompanyShiftById(id);
  }

  /**
   * Add new company attendance shift
   * @param shiftData ClientCompanyAttendanceShiftSM
   * @returns ApiResponse<ClientCompanyAttendanceShiftSM>
   */
  async addCompanyShift(shiftData: ClientCompanyAttendanceShiftSM): Promise<ApiResponse<ClientCompanyAttendanceShiftSM>> {
    if (!shiftData) {
      throw new Error('Shift data is required to add new company shift');
    }
    const apiRequest = new ApiRequest<ClientCompanyAttendanceShiftSM>();
    apiRequest.reqData = shiftData;
    return await this.companyAttendanceShiftClient.AddNewCompanyAttendanceShiftDetails(apiRequest);
  }

  /**
   * Update existing company attendance shift
   * @param shiftData ClientCompanyAttendanceShiftSM
   * @returns ApiResponse<ClientCompanyAttendanceShiftSM>
   */
  async updateCompanyShift(shiftData: ClientCompanyAttendanceShiftSM): Promise<ApiResponse<ClientCompanyAttendanceShiftSM>> {
    if (!shiftData || !shiftData.id) {
      throw new Error('Shift data with valid ID is required to update shift');
    }
    const apiRequest = new ApiRequest<ClientCompanyAttendanceShiftSM>();
    apiRequest.reqData = shiftData;
    return await this.companyAttendanceShiftClient.UpdateCompanyAttendanceShiftDetails(apiRequest);
  }

  /**
   * Delete company shift by ID
   * @param id Shift ID
   * @returns ApiResponse<DeleteResponseRoot>
   */
  async deleteCompanyShift(id: number): Promise<ApiResponse<DeleteResponseRoot>> {
    if (id <= 0) {
      throw new Error(AppConstants.ErrorPrompts.Delete_Data_Error);
    }
    return await this.companyAttendanceShiftClient.DeleteCompanyShiftById(id);
  }
}