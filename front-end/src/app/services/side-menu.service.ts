import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { SideMenuClient } from '../clients/side-menu.client';
import { ApiResponse } from '../service-models/foundation/api-contracts/base/api-response';
import { ApiRequest } from '../service-models/foundation/api-contracts/base/api-request';
import { CompanyModulesSM } from '../service-models/app/v1/client/company-modules-s-m';
import { ClientThemeSM } from '../service-models/app/v1/client/client-theme-s-m';

@Injectable({
  providedIn: 'root'
})
export class SideMenuService extends BaseService {
  superCompanyClient: any;

  constructor(private sideMenuClient:SideMenuClient) {
    super ()
   }
   /**GET Generic Payroll
   * @DEV : Musaib
  */
   async GetUserProfilePictue(): Promise<ApiResponse<string>> {
   return await this.sideMenuClient.GetProfilePictureOfTheUser();
 }

  /**Add Generic Payroll
   * @DEV : Musaib
  */
async AddUserProfilePicture(addUserProfilePicture:string): Promise<ApiResponse<string>> {
  let apiRequest = new ApiRequest<string>();
  apiRequest.reqData = addUserProfilePicture;
  return await this.sideMenuClient.AddUserProfilePicture(apiRequest);
}
  /**Get selected Company Modules */
  async getCompanyModules(): Promise<ApiResponse<CompanyModulesSM>> {
    return await this.sideMenuClient.GetCompanyModules();
  }

}
