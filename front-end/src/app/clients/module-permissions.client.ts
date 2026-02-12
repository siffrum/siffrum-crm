import { Injectable } from "@angular/core";
import { BaseApiClient } from "./base-client/base-api.client";
import { StorageService } from "../services/storage.service";
import { StorageCache } from "./helpers/storage-cache.helper";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { AppConstants } from "src/app-constants";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { RoleTypeSM } from "../service-models/app/enums/role-type-s-m.enum";
import { BoolResponseRoot } from "../service-models/foundation/common-response/bool-response-root";

@Injectable({
  providedIn: "root",
})
export class ModulePermissionClient extends BaseApiClient {
  constructor(
    storageService: StorageService,
    storageCache: StorageCache,
    commonResponseCodeHandler: CommonResponseCodeHandler
  ) {
    super(storageService, storageCache, commonResponseCodeHandler);
  }
  //GET/ADD/UPDATE permissions For Company ADMIN AND EMPLOYEE
  /** Get Company Module And Permissions
   *  @DEV : Musaib
   */
  GetAllModulesAndPermissions = async (
    compid: number,
    roleType: RoleTypeSM
  ): Promise<ApiResponse<PermissionSM[]>> => {
    let finalUrl = `${AppConstants.ApiUrls.SUPER_COMPANY_SETTING_URL}/CompanyModulesPermission/${compid}/${roleType}`;
    let resp = await this.GetResponseAsync<null, PermissionSM[]>(
      `${finalUrl}`,
      "GET"
    );
    return resp;
  };
    /** Update Compny  Module And Permissions for both admin aswell as employee
   * @DEV : Musaib
   */
    updateModulePermissions = async (
      permissions: ApiRequest<PermissionSM[]>
    ): Promise<ApiResponse<BoolResponseRoot>> => {
      let resp = await this.GetResponseAsync<PermissionSM[], BoolResponseRoot>(
        `${AppConstants.ApiUrls.SUPER_COMPANY_SETTING_URL}/UpdateModulePemissions`,
        "PUT",
        permissions
      );
      return resp;
    };
//GET/ADD/UPDATE permissions For user

  /** Get  Module And Permissions For Admin
   *  @DEV : Musaib
   */
  GetAllModulesAndPermissionsForUser = async (
    compid: number,
    roleType: RoleTypeSM,
    userId:number
  ): Promise<ApiResponse<PermissionSM[]>> => {
    let finalUrl = `${AppConstants.ApiUrls.SUPER_COMPANY_SETTING_URL}/ModulePermissionsOfUser/${compid}/${roleType}/${userId}`;
    let resp = await this.GetResponseAsync<null, PermissionSM[]>(
      `${finalUrl}`,
      "GET"
    );
    return resp;
  };
    /**
   * Add ModulePermissionsForUser
   */
    AddModulePermissionsForUser = async (
      updateCompanyModulesPermissionRequest: ApiRequest<PermissionSM[]>
    ): Promise<ApiResponse<BoolResponseRoot>> => {
      let resp = await this.GetResponseAsync<PermissionSM[], BoolResponseRoot>(
        `${AppConstants.ApiUrls.SUPER_COMPANY_SETTING_URL}/AddModulePermissions`,
        "POST",
        updateCompanyModulesPermissionRequest
      );
      return resp;
    };
  /**Update Compny  Module And Permissions
   * @DEV : Musaib
   */
  UpdateModulePermissionsForUser = async (
    updateCompanyModulesPermissionRequest: ApiRequest<PermissionSM[]>
  ): Promise<ApiResponse<BoolResponseRoot>> => {
    let resp = await this.GetResponseAsync<PermissionSM[], BoolResponseRoot>(
      `${AppConstants.ApiUrls.SUPER_COMPANY_SETTING_URL}/UpdateModulePermissionsForUser`,
      "PUT",
      updateCompanyModulesPermissionRequest
    );
    return resp;
  };




}
