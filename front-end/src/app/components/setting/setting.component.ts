import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../base.component';
import { SettingViewModel, settingTabs } from 'src/app/view-models/setting.viewmodel';
import { LogHandlerService } from 'src/app/services/log-handler.service';
import { CommonService } from 'src/app/services/common.service';
import { SettingService } from 'src/app/services/setting.service';

@Component({
    selector: 'app-setting',
    templateUrl: './setting.component.html',
    styleUrls: ['./setting.component.scss'],
    standalone: false
})
export class SettingComponent extends BaseComponent<SettingViewModel> implements OnInit  {
  //*@Dev Musaib
  constructor(
    commonService:CommonService,
    logService:LogHandlerService,
   private settingService:SettingService
    ){
    super(commonService,logService)
    this.viewModel=new SettingViewModel()
  }

  async ngOnInit() {
    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;
  }
/**
 * Get Themes List
 */
  override async loadPageData() {
    try {
      await this._commonService.presentLoading();
      let resp = await this.settingService.getAllClientThemes();
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {

        this.viewModel.clientTheme = resp.successData;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
/**
 * Update Theme
 * @param theme
   *@Dev Musaib
 *
 */
  async updateTheme(theme:any) {
    try {
      await this._commonService.presentLoading();
      let themeId=theme.id;
      let resp = await this.settingService.updateThemeById(
        themeId
      );
      if (resp.isError) {
        // await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this._commonService.ShowToastAtTopEnd(
          "Theme Updated Successfully",
          "success"
        );
        let theme = resp.successData.boolResponse;
        if(theme==true){
           await this._commonService.applyThemeGlobally()
        }
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
//update Tab locations
  updateTabLocation(tabLocation: settingTabs) {
    switch (tabLocation) {
      case settingTabs.notification:
        break;
      case settingTabs.security:
        break;
      case settingTabs.accountSetting:
      default:
        break;
    }
    this.viewModel.tablocation = tabLocation;
  }
}
