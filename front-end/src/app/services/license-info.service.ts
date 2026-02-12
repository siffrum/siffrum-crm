import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { LicenseInfoClient } from '../clients/license-info.client';
import { ApiResponse } from '../service-models/foundation/api-contracts/base/api-response';
import { CheckoutSessionRequestSM } from '../service-models/app/v1/license/checkout-session-request-s-m';
import { CheckoutSessionResponseSM } from '../service-models/app/v1/license/checkout-session-response-s-m';
import { ApiRequest } from '../service-models/foundation/api-contracts/base/api-request';
import { LicenseTypeSM } from '../service-models/app/v1/license/license-type-s-m';
import { AppConstants } from 'src/app-constants';
import { CustomerPortalResponseSM } from '../service-models/app/v1/license/customer-portal-response-s-m';
import { CustomerPortalRequestSM } from '../service-models/app/v1/license/customer-portal-request-s-m';
import { CompanyLicenseDetailsSM } from '../service-models/app/v1/license/company-license-details-s-m';

@Injectable({
  providedIn: 'root'
})
export class LicenseInfoService extends BaseService{

  constructor(private licenseInfoClient:LicenseInfoClient) {
    super()
  }
  /**
   *
   *Get Mine User TrialLicense TyPe
   * @returns
   */
  // async getUserActiveTrialLicenseInfo(): Promise<ApiResponse<CompanyLicenseDetailsSM>> {
  //   return await this.licenseInfoClient.GetUserActiveTrialLicenseInfo();
  // }
  /**
   * Get Mine User License Info
   * @returns
   */
  async getUserMineActiveLicenseInfo(): Promise<ApiResponse<CompanyLicenseDetailsSM>> {
    return await this.licenseInfoClient.GetMyActiveLicenseInfo();
  }
/**
 * Buy New Paid License
 * @param request
 * @returns 
 */
  GenerateCheckoutSessionDetails(request: CheckoutSessionRequestSM): Promise<ApiResponse<CheckoutSessionResponseSM>> {
    let apiReq = new ApiRequest<CheckoutSessionRequestSM>();
    apiReq.reqData = request;
    return this.licenseInfoClient.GenerateCheckoutSessionMine(apiReq);
  }
  /**
   * Get All License Types
   * @returns
   */

  async getALLlicenseTypesExtended(): Promise<ApiResponse<LicenseTypeSM[]>> {
    return await this.licenseInfoClient.GetALLlicenseTypes();
  }

  /**
   *Get License By License ID
   * @param LicenseId
   * @returns 
   */

  async getLicenseByLicenseId(LicenseId: number): Promise<ApiResponse<LicenseTypeSM>> {
    if (LicenseId <= 0) {
      throw new Error(AppConstants.ErrorPrompts.Delete_Data_Error)
    }
    return await this.licenseInfoClient.GetLicenseByLicenseInfoByLicenseId(LicenseId);
  }

/**
 * Add Trial License
 * @returns
 */
async AddTrialLicense(): Promise<ApiResponse<LicenseTypeSM>> {

  return await this.licenseInfoClient.AddUserTrial();
}
/**
 * Upgrade Subscription
 * @param returnUrl
 * @returns 
 */
ManageSubscription(returnUrl: string): Promise<ApiResponse<CustomerPortalResponseSM>> {
  let apiReq = new ApiRequest<CustomerPortalRequestSM>();
  apiReq.reqData = { returnUrl };
  return this.licenseInfoClient.ManageSubscription(apiReq);
}
}