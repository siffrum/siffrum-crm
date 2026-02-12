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

  constructor(private companyAttendanceShiftClient:CompanyAttendanceShift) {

    super()
   }


  /**
   * @Dev Musaib
 * Get Company  Shift Details
 * @param companyId
 * @returns
 */
  async getAllCompanyAttendanceShiftDetails(): Promise<ApiResponse<ClientCompanyAttendanceShiftSM[]>> {
    return await this.companyAttendanceShiftClient.GetAllCompanyAttendanceShiftDetails();
  }

  /**
 * Get Company Shift Details For Selected Company
 * @param companyId
 * @returns
 */
  async getCompanyShiftById(
    Id: number
  ): Promise<ApiResponse<ClientCompanyAttendanceShiftSM>> {
    return await this.companyAttendanceShiftClient.GetCompanyShiftById(Id);
  }

   /**update Company shift Details */
   async updateCompanyAttendanceShiftDetails(updatecompanyAttendanceShiftDetails:ClientCompanyAttendanceShiftSM): Promise<ApiResponse<ClientCompanyAttendanceShiftSM>> {
    let apiRequest = new ApiRequest<ClientCompanyAttendanceShiftSM>();
    apiRequest.reqData = updatecompanyAttendanceShiftDetails;
    return await this.companyAttendanceShiftClient.UpdateCompanyAttendanceShiftDetails(apiRequest);
  }
     /** Add Company Shift Details
   * @return Success
    */
     async addNewCompanyAttendanceShiftDetails(user:ClientCompanyAttendanceShiftSM): Promise<ApiResponse<ClientCompanyAttendanceShiftSM>> {
      let apiRequest = new ApiRequest<ClientCompanyAttendanceShiftSM>();
      apiRequest.reqData =user;
      return await this.companyAttendanceShiftClient.AddNewCompanyAttendanceShiftDetails(apiRequest);
    }

     /**Delete Company Shift
   * @DEV : Musaib
  */
  async DeleteCompanyShift(
    id: number
  ): Promise<ApiResponse<DeleteResponseRoot>> {
    if (id <= 0) {
      throw new Error(AppConstants.ErrorPrompts.Delete_Data_Error);
    }
    return await this.companyAttendanceShiftClient.DeleteCompanyShiftById(
      id
    );
  }
}
