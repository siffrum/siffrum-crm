import { Injectable } from "@angular/core";
import { AppConstants } from "src/app-constants";
import {
  AdditionalRequestDetails,
  Authentication,
} from "../internal-models/additional-request-details";
import { TokenRequestSM } from "../service-models/app/token/token-request-s-m";
import { TokenResponseSM } from "../service-models/app/token/token-response-s-m";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { StorageService } from "../services/storage.service";
import { BaseApiClient } from "./base-client/base-api.client";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { StorageCache } from "./helpers/storage-cache.helper";
import { IntResponseRoot } from "../service-models/foundation/common-response/int-response-root";
import { ForgotPasswordSM } from "../service-models/app/v1/app-users/forgot-password-s-m";
import { ResetPasswordRequestSM } from "../service-models/app/v1/app-users/reset-password-request-s-m";
import { BoolResponseRoot } from "../service-models/foundation/common-response/bool-response-root";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
import { UpdateCredentialSM } from "../service-models/app/v1/app-users/update-credential-s-m";
import { ModuleNameSM } from "../service-models/app/enums/module-name-s-m.enum";

@Injectable({
  providedIn: "root",
})
export class AccountsClient extends BaseApiClient {
  constructor(
    storageService: StorageService,
    storageCache: StorageCache,
    commonResponseCodeHandler: CommonResponseCodeHandler
  ) {
    super(storageService, storageCache, commonResponseCodeHandler);
  }
  /** comments */
  GenerateToken = async ( tokenRequestSM: ApiRequest<TokenRequestSM>): Promise<ApiResponse<TokenResponseSM>> => {
    let resp = await this.GetResponseAsync<TokenRequestSM, TokenResponseSM>(
      `${AppConstants.ApiUrls.ACCOUNT_URL}/ValidateLoginAndGenerateToken`,
      "POST",
      tokenRequestSM,
      new AdditionalRequestDetails(false, Authentication.false)
    );
    return resp;
  };

  ValidateLoginId = async (
    loginId: string
  ): Promise<ApiResponse<BoolResponseRoot>> => {
    let resp = await this.GetResponseAsync<null, BoolResponseRoot>(
      `${AppConstants.ApiUrls.ADD_VALID_USER}/ValidateLoginId?loginId=${loginId}`,
      "GET",
      null,
      new AdditionalRequestDetails<BoolResponseRoot>(
        false,
        Authentication.false
      )
    );
    return resp;
  };

  UpdatePassword = async (
    updatePasswordRequest: ApiRequest<ResetPasswordRequestSM>
  ): Promise<ApiResponse<ResetPasswordRequestSM>> => {
    let resp = await this.GetResponseAsync<
      ResetPasswordRequestSM,
      ResetPasswordRequestSM
    >(
      `${AppConstants.ApiUrls.USER_API_URL}/ResetPassword`,
      "POST",
      updatePasswordRequest,
      new AdditionalRequestDetails<ResetPasswordRequestSM>(
        false,
        Authentication.false
      )
    );
    return resp;
  };

  SendForgotPassword = async (
    contactUsRequest: ApiRequest<ForgotPasswordSM>
  ): Promise<ApiResponse<ForgotPasswordSM>> => {
    let resp = await this.GetResponseAsync<ForgotPasswordSM, ForgotPasswordSM>(
      `${AppConstants.ApiUrls.USER_API_URL}/ForgotPassword`,
      "POST",
      contactUsRequest,
      new AdditionalRequestDetails<ForgotPasswordSM>(
        false,
        Authentication.false
      )
    );
    return resp;
  };
  //update this accordingly
  ValidateAuthCode = async (
    authCode: string
  ): Promise<ApiResponse<IntResponseRoot>> => {
    let url = `${
      AppConstants.ApiUrls.USER_API_URL
    }/ValidatePassword?authcode=${encodeURIComponent(authCode)}`;
    let resp = await this.GetResponseAsync<null, IntResponseRoot>(
      url,
      "GET",
      null,
      new AdditionalRequestDetails<IntResponseRoot>(false, Authentication.false)
    );
    return resp;
  };

  /** Get All the Company Module Permissions(GET method) */
  GetAllCompanyPermissions = async (): Promise<
    ApiResponse<PermissionSM[]>
  > => {
    let resp = await this.GetResponseAsync<null, PermissionSM[]>(
      `${AppConstants.ApiUrls.PERMISSIONS}/my`,
      "GET",
      null,
      new AdditionalRequestDetails(false, Authentication.true)
    );
    return resp;
  };

    /** Get Company Module Permissions by module Name(GET method) */
    GetCompanyModulePermissions = async (moduleName: ModuleNameSM ): Promise<
    ApiResponse<PermissionSM>
  > => {
    let resp = await this.GetResponseAsync<null, PermissionSM>(
      `${AppConstants.ApiUrls.PERMISSIONS}/my/${moduleName}`,
      "GET",
      null,
      new AdditionalRequestDetails(false, Authentication.true)
    );
    return resp;
  };


/**Update password */
    GetUserName = async (): Promise<
    ApiResponse<UpdateCredentialSM>
  > => {
    let resp = await this.GetResponseAsync<null, UpdateCredentialSM>(
      `${AppConstants.ApiUrls.USER_API_URL}/ChangePassword`,
      "GET"
    );
    return resp;
  };
  /**Change password */
  ChangePassword = async (
    updateCredentials: ApiRequest<UpdateCredentialSM>
  ): Promise<ApiResponse<BoolResponseRoot>> => {
    let resp = await this.GetResponseAsync<UpdateCredentialSM, BoolResponseRoot>(
      `${AppConstants.ApiUrls.USER_API_URL}/ChangePassword`,
      "POST",
      updateCredentials
    );
    return resp;
  };
}
