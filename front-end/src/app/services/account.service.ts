import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { AppConstants } from "src/app-constants";
import { AccountsClient } from "../clients/accounts.client";
import { RoleTypeSM } from "../service-models/app/enums/role-type-s-m.enum";
import { ModuleNameSM } from "../service-models/app/enums/module-name-s-m.enum";
import { TokenRequestSM } from "../service-models/app/token/token-request-s-m";
import { TokenResponseSM } from "../service-models/app/token/token-response-s-m";
import { LoginUserSM } from "../service-models/app/v1/app-users/login/login-user-s-m";
import { ForgotPasswordSM } from "../service-models/app/v1/app-users/forgot-password-s-m";
import { ResetPasswordRequestSM } from "../service-models/app/v1/app-users/reset-password-request-s-m";
import { UpdateCredentialSM } from "../service-models/app/v1/app-users/update-credential-s-m";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { BoolResponseRoot } from "../service-models/foundation/common-response/bool-response-root";
import { IntResponseRoot } from "../service-models/foundation/common-response/int-response-root";
import { LoginViewModel } from "../view-models/login.viewmodel";
import { AdminLoginViewModel } from "../view-models/admin-login.viemodel";
import { BaseService } from "./base.service";
import { CommonService } from "./common.service";
import { StorageService } from "./storage.service";

@Injectable({
  providedIn: "root",
})
export class AccountService extends BaseService {
  constructor(
    private accountClient: AccountsClient,
    private storageService: StorageService,
    private router: Router,
    private commonService: CommonService
  ) {
    super();
  }

  /**
   * Generate token for Client Admin and Employees
   */
  async generateToken(
    tokenReq: TokenRequestSM,
    loginViewModel: LoginViewModel
  ): Promise<ApiResponse<TokenResponseSM>> {
    if (!tokenReq || !tokenReq.loginId) {
      throw new Error(AppConstants.ErrorPrompts.Invalid_Input_Data);
    }

    const apiRequest = new ApiRequest<TokenRequestSM>();
    apiRequest.reqData = tokenReq;

    const resp = await this.accountClient.GenerateToken(apiRequest);

    if (
      !resp ||
      resp.isError ||
      !resp.successData ||
      !resp.successData.accessToken
    ) {
      return resp;
    }

    if (loginViewModel.rememberMe === true) {
      this.storageService.setToStorage(
        AppConstants.DbKeys.ACCESS_TOKEN,
        resp.successData.accessToken
      );
      this.storageService.setToStorage(
        AppConstants.DbKeys.TOKEN_EXPIRY,
        resp.successData.expiresUtc.toString()
      );
      this.storageService.setToStorage(
        AppConstants.DbKeys.LOGIN_USER,
        JSON.stringify(resp.successData.loginUserDetails)
      );
      this.storageService.setToStorage(
        AppConstants.DbKeys.CLIENT_COMPANY_ID,
        JSON.stringify(resp.successData.clientCompantId)
      );
      this.storageService.setToStorage(
        AppConstants.DbKeys.REMEMBER_ME,
        JSON.stringify(loginViewModel.rememberMe)
      );
    } else {
      this.storageService.saveToSessionStorage(
        AppConstants.DbKeys.ACCESS_TOKEN,
        resp.successData.accessToken
      );
      this.storageService.saveToSessionStorage(
        AppConstants.DbKeys.TOKEN_EXPIRY,
        resp.successData.expiresUtc.toString()
      );
      this.storageService.saveToSessionStorage(
        AppConstants.DbKeys.LOGIN_USER,
        JSON.stringify(resp.successData.loginUserDetails)
      );
      this.storageService.saveToSessionStorage(
        AppConstants.DbKeys.CLIENT_COMPANY_ID,
        resp.successData.clientCompantId
      );
      this.storageService.saveToSessionStorage(
        AppConstants.DbKeys.REMEMBER_ME,
        loginViewModel.rememberMe
      );
    }

    return resp;
  }

  /**
   * Generate token for Super/System Admin
   */
  async generateAdminLoginToken(
    tokenReq: TokenRequestSM,
    adminLoginViewModel: AdminLoginViewModel
  ): Promise<ApiResponse<TokenResponseSM>> {
    if (!tokenReq || !tokenReq.loginId) {
      throw new Error(AppConstants.ErrorPrompts.Invalid_Input_Data);
    }

    const apiRequest = new ApiRequest<TokenRequestSM>();
    apiRequest.reqData = tokenReq;

    const resp = await this.accountClient.GenerateToken(apiRequest);

    if (!resp || resp.isError || !resp.successData) {
      return resp;
    }

    if (adminLoginViewModel.rememberMe === true) {
      this.storageService.setToStorage(
        AppConstants.DbKeys.ACCESS_TOKEN,
        resp.successData.accessToken
      );
      this.storageService.setToStorage(
        AppConstants.DbKeys.TOKEN_EXPIRY,
        resp.successData.expiresUtc.toString()
      );
      this.storageService.setToStorage(
        AppConstants.DbKeys.LOGIN_USER,
        JSON.stringify(resp.successData.loginUserDetails)
      );
      this.storageService.setToStorage(
        AppConstants.DbKeys.CLIENT_COMPANY_ID,
        JSON.stringify(resp.successData.clientCompantId)
      );
      this.storageService.setToStorage(
        AppConstants.DbKeys.REMEMBER_ME,
        JSON.stringify(adminLoginViewModel.rememberMe)
      );
    } else {
      this.storageService.saveToSessionStorage(
        AppConstants.DbKeys.ACCESS_TOKEN,
        resp.successData.accessToken
      );
      this.storageService.saveToSessionStorage(
        AppConstants.DbKeys.TOKEN_EXPIRY,
        resp.successData.expiresUtc.toString()
      );
      this.storageService.saveToSessionStorage(
        AppConstants.DbKeys.LOGIN_USER,
        JSON.stringify(resp.successData.loginUserDetails)
      );
      this.storageService.saveToSessionStorage(
        AppConstants.DbKeys.CLIENT_COMPANY_ID,
        resp.successData.clientCompantId
      );
      this.storageService.saveToSessionStorage(
        AppConstants.DbKeys.REMEMBER_ME,
        adminLoginViewModel.rememberMe
      );
    }

    return resp;
  }

  async logoutUser(): Promise<void> {
    try {
      this.storageService.removeFromSessionStorage(
        AppConstants.DbKeys.ACCESS_TOKEN
      );
      this.storageService.removeFromSessionStorage(
        AppConstants.DbKeys.LOGIN_USER
      );
      this.storageService.removeFromSessionStorage(
        AppConstants.DbKeys.CLIENT_COMPANY_ID
      );
      this.storageService.removeFromSessionStorage(
        AppConstants.DbKeys.REMEMBER_ME
      );
      this.storageService.removeFromSessionStorage(
        AppConstants.DbKeys.TOKEN_EXPIRY
      );
    } catch {
      this.storageService.clearSessionStorage();
    }

    try {
      this.storageService.removeFromStorage(AppConstants.DbKeys.ACCESS_TOKEN);
      this.storageService.removeFromStorage(AppConstants.DbKeys.LOGIN_USER);
      this.storageService.removeFromStorage(
        AppConstants.DbKeys.CLIENT_COMPANY_ID
      );
      this.storageService.removeFromStorage(AppConstants.DbKeys.REMEMBER_ME);
      this.storageService.removeFromStorage(AppConstants.DbKeys.TOKEN_EXPIRY);
    } catch {
      this.storageService.clearStorage();
    }

    if (this.commonService.layoutVM.tokenRole == 1) {
      this.commonService.layoutVM.tokenRole = RoleTypeSM.Unknown;
      this.commonService.layoutVM.showLeftSideMenu = false;
      this.router.navigate(["admin/login"]);
    } else {
      this.commonService.layoutVM.tokenRole = RoleTypeSM.Unknown;
      this.commonService.layoutVM.showLeftSideMenu = false;
      this.router.navigate(["login"]);
    }
  }

  async getTokenFromStorage(): Promise<string> {
    const remMe: boolean = await this.storageService.getFromStorage(
      AppConstants.DbKeys.REMEMBER_ME
    );

    if (remMe === true) {
      return await this.storageService.getFromStorage(
        AppConstants.DbKeys.ACCESS_TOKEN
      );
    }

    return await this.storageService.getFromSessionStorage(
      AppConstants.DbKeys.ACCESS_TOKEN
    );
  }

  async getUserFromStorage(): Promise<LoginUserSM | ""> {
    const remMe: boolean = await this.storageService.getFromStorage(
      AppConstants.DbKeys.REMEMBER_ME
    );

    if (remMe === true) {
      return await this.storageService.getFromStorage(
        AppConstants.DbKeys.LOGIN_USER
      );
    }

    return await this.storageService.getFromSessionStorage(
      AppConstants.DbKeys.LOGIN_USER
    );
  }

  async ValidateLoginId(
    loginId: string
  ): Promise<ApiResponse<BoolResponseRoot>> {
    return await this.accountClient.ValidateLoginId(loginId);
  }

  async UpdatePassword(
    updatePassword: ResetPasswordRequestSM
  ): Promise<ApiResponse<ResetPasswordRequestSM>> {
    if (!updatePassword) {
      throw new Error(AppConstants.ErrorPrompts.Invalid_Input_Data);
    }

    const apiRequest = new ApiRequest<ResetPasswordRequestSM>();
    apiRequest.reqData = updatePassword;

    return await this.accountClient.UpdatePassword(apiRequest);
  }

  async Send_forgotPassword(
    password: ForgotPasswordSM
  ): Promise<ApiResponse<ForgotPasswordSM>> {
    if (!password) {
      throw new Error(AppConstants.ErrorPrompts.Invalid_Input_Data);
    }

    const apiRequest = new ApiRequest<ForgotPasswordSM>();
    apiRequest.reqData = password;

    return await this.accountClient.SendForgotPassword(apiRequest);
  }

  async validateAuthCode(
    authCode: string
  ): Promise<ApiResponse<IntResponseRoot>> {
    if (!authCode) {
      throw new Error("authCode is required");
    }

    return await this.accountClient.ValidateAuthCode(authCode);
  }

  async getMyModulePermissions(
    moduleName: ModuleNameSM
  ): Promise<PermissionSM | undefined> {
    const permissions =
      await this.accountClient.GetCompanyModulePermissions(moduleName);
    return permissions.successData;
  }

  async getUserName(): Promise<ApiResponse<UpdateCredentialSM>> {
    return await this.accountClient.GetUserName();
  }

  async changePassword(
    updateCredentials: UpdateCredentialSM
  ): Promise<ApiResponse<BoolResponseRoot>> {
    const apiRequest = new ApiRequest<UpdateCredentialSM>();
    apiRequest.reqData = updateCredentials;
    return await this.accountClient.ChangePassword(apiRequest);
  }
}