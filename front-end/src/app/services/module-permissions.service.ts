import { Injectable } from "@angular/core";
import { BaseService } from "./base.service";
import { ModulePermissionClient } from "../clients/module-permissions.client";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { RoleTypeSM } from "../service-models/app/enums/role-type-s-m.enum";
import { BoolResponseRoot } from "../service-models/foundation/common-response/bool-response-root";

@Injectable({
  providedIn: "root",
})
export class ModulePermissionsService extends BaseService {
  constructor(private modulePermissionsClient: ModulePermissionClient) {
    super();
  }
  /** Get Company Module And Permissions
   *  @DEV : Musaib
   */
  async getAllModulesAndPermissions(
    compid: number,
    roleType: RoleTypeSM
  ): Promise<ApiResponse<PermissionSM[]>> {
    return await this.modulePermissionsClient.GetAllModulesAndPermissions(
      compid,
      roleType
    );
  }
  async getAllModulesAndPermissionsForUser(
    compid: number,
    roleType: RoleTypeSM,
    userId:number
  ): Promise<ApiResponse<PermissionSM[]>> {
    return await this.modulePermissionsClient.GetAllModulesAndPermissionsForUser(
      compid,
      roleType,
      userId
    );
  }
    /**Update Company Modules Permission
   * @DEV : Musaib
   */
    async addModulePermissionsForUser(
      updateCompanyModulesPermission: PermissionSM[]
    ): Promise<ApiResponse<BoolResponseRoot>> {
      let apiRequest = new ApiRequest<PermissionSM[]>();
      apiRequest.reqData = updateCompanyModulesPermission;
      return await this.modulePermissionsClient.AddModulePermissionsForUser(
        apiRequest
      );
    }
  /**Update Company Modules Permission
   * @DEV : Musaib
   */
  async updateModulePermissionsForUser(
    updateCompanyModulesPermission: PermissionSM[]
  ): Promise<ApiResponse<BoolResponseRoot>> {
    let apiRequest = new ApiRequest<PermissionSM[]>();
    apiRequest.reqData = updateCompanyModulesPermission;
    return await this.modulePermissionsClient.UpdateModulePermissionsForUser(
      apiRequest
    );
  }
  /** update Module And Permission  Details for both admin as well as employee
   *  @DEV : Musaib
   */
  async updateModulePermissions(
    permissions: PermissionSM[]
  ): Promise<ApiResponse<BoolResponseRoot>> {
    let apiRequest = new ApiRequest<PermissionSM[]>();
    apiRequest.reqData = permissions;
    return await this.modulePermissionsClient.updateModulePermissions(
      apiRequest
    );
  }
}
