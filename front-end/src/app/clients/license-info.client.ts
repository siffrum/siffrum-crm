import { Injectable } from "@angular/core";
import { BaseApiClient } from "./base-client/base-api.client";
import { StorageService } from "../services/storage.service";
import { StorageCache } from "./helpers/storage-cache.helper";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { AppConstants } from "src/app-constants";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { CheckoutSessionRequestSM } from "../service-models/app/v1/license/checkout-session-request-s-m";
import { CheckoutSessionResponseSM } from "../service-models/app/v1/license/checkout-session-response-s-m";
import { LicenseTypeSM } from "../service-models/app/v1/license/license-type-s-m";
import { AdditionalRequestDetails, Authentication } from "../internal-models/additional-request-details";
import { CustomerPortalRequestSM } from "../service-models/app/v1/license/customer-portal-request-s-m";
import { CustomerPortalResponseSM } from "../service-models/app/v1/license/customer-portal-response-s-m";
import { CompanyLicenseDetailsSM } from "../service-models/app/v1/license/company-license-details-s-m";

@Injectable({
    providedIn: 'root'
})


export class LicenseInfoClient extends BaseApiClient {

    constructor(storageService: StorageService, storageCache: StorageCache,
        commonResponseCodeHandler: CommonResponseCodeHandler) {
        super(storageService, storageCache, commonResponseCodeHandler)
    }
    /**@Developer Musaib */
    /**
     * Get ActiveTrial License Info Of The LoggedIn User
     * @returns
     */
    GetUserActiveTrialLicenseInfo = async (): Promise<ApiResponse<CompanyLicenseDetailsSM>> => {
        let finalUrl = this.ApplyQueryFilterToUrl(`${AppConstants.ApiUrls.LICENSE_DETAILS}/mine/ActiveTrialLicense`);
        let resp = await this.GetResponseAsync<CompanyLicenseDetailsSM, CompanyLicenseDetailsSM>(`${finalUrl}`, 'GET');
        return resp;
    }
      /**
     * Get Mine Active PurchasedLicense Info Of The LoggedIn User
     * @returns
     */
    GetMyActiveLicenseInfo = async (): Promise<ApiResponse<CompanyLicenseDetailsSM>> => {
        let resp = await this.GetResponseAsync<null, CompanyLicenseDetailsSM>(`${AppConstants.ApiUrls.LICENSE_DETAILS}/my/Active`, 'GET');
        return resp;
    }
/**
 * Buy New License Using Stripe
 * Get Success/Failure Url
 * @param apiRequest
 * @returns 
 */
    GenerateCheckoutSessionMine = async (apiRequest: ApiRequest<CheckoutSessionRequestSM>): Promise<ApiResponse<CheckoutSessionResponseSM>> => {
        let resp = await this.GetResponseAsync<CheckoutSessionRequestSM, CheckoutSessionResponseSM>
            (`${AppConstants.ApiUrls.PAYMENT_URL}/mine/checkout`,"POST", apiRequest);
        return resp;
    }

/**
 * Get All License Types
 * @returns
 */
    GetALLlicenseTypes = async (): Promise<ApiResponse<LicenseTypeSM[]>> => {
        let resp = await this.GetResponseAsync<null, LicenseTypeSM[]>
            (`${AppConstants.ApiUrls.LICENSE_TYPE_URL}`, 'GET', null,
                new AdditionalRequestDetails<LicenseTypeSM[]>(false, Authentication.false));
        return resp;
    }
    /**
     * Get License Info By License Id
     * @param licenseId
     * @returns 
     */

    GetLicenseByLicenseInfoByLicenseId= async (licenseId: number): Promise<ApiResponse<LicenseTypeSM>> => {
        let resp = await this.GetResponseAsync<number, LicenseTypeSM>
            (`${AppConstants.ApiUrls.LICENSE_TYPE_URL}/${licenseId}`, 'GET', null,
                new AdditionalRequestDetails<LicenseTypeSM>(false, Authentication.false));
        return resp;
    }
/**
 * Add Trial License To The User
 * @returns
 */
    AddUserTrial = async (): Promise<ApiResponse<LicenseTypeSM>> => {
        let resp = await this.GetResponseAsync<LicenseTypeSM, LicenseTypeSM>
          (`${AppConstants.ApiUrls.LICENSE_DETAILS}/mine/AddTrial`, "POST",);
        return resp;
      };

/**
 * Upgrade Subscription
 * @param apiRequest
 * @returns 
 */
      ManageSubscription = async (apiRequest: ApiRequest<CustomerPortalRequestSM>): Promise<ApiResponse<CustomerPortalResponseSM>> => {
        let resp = await this.GetResponseAsync<CustomerPortalRequestSM, CustomerPortalResponseSM>
            (`${AppConstants.ApiUrls.PAYMENT_URL}/mine/StripeCustomerPortal`, "POST", apiRequest);
        return resp;
    }
}