import { Injectable } from '@angular/core';
import { SettingClient } from '../clients/setting.client';
import { ApiResponse } from '../service-models/foundation/api-contracts/base/api-response';
import { ClientThemeSM } from '../service-models/app/v1/client/client-theme-s-m';
import { BaseService } from './base.service';
import { BoolResponseRoot } from '../service-models/foundation/common-response/bool-response-root';

@Injectable({
  providedIn: 'root'
})
export class SettingService extends BaseService{

  constructor(private settingClient:SettingClient) {

      super()

   }
   /**
    * Get All Theme Names
    * @returns
    */
   getAllClientThemes= async (): Promise<ApiResponse<ClientThemeSM[]>> => {
    return await this.settingClient.GetAllClientThemes();
  }
  /**
   * Get Client theme
   * @returns
   */
    getClientTheme = async (): Promise<ApiResponse<ClientThemeSM>> => {
    return await this.settingClient.GetClientTheme();
  }

  /**
   * Default theme
   * @returns
   */
   getClientDefaultTheme = async ():Promise<ApiResponse<ClientThemeSM>> =>{
    return await this.settingClient.GetClientDefaultTheme();
  }

  /**
   * Update Theme by theme id 
   * @param themeId
   * @returns 
   */
  async updateThemeById(themeId:number): Promise<ApiResponse<BoolResponseRoot>> {
    return await this.settingClient.UpdateTheme(themeId);
}
}
