import { Component, OnInit } from '@angular/core';
import { AuthGuard } from 'src/app/guard/auth.guard';
import { AccountService } from 'src/app/services/account.service';
import { CommonService } from 'src/app/services/common.service';
import { LogHandlerService } from 'src/app/services/log-handler.service';
import { AdminDashboardViewModel } from 'src/app/view-models/admin-dashboard.viewmodel';
import { BaseComponent } from '../../base.component';
import { SettingService } from 'src/app/services/setting.service';

@Component({
    selector: 'app-admin-dashboard',
    templateUrl: './admin-dashboard.component.html',
    styleUrls: ['./admin-dashboard.component.scss'],
    standalone: false
})
export class AdminDashboardComponent extends BaseComponent<AdminDashboardViewModel> implements OnInit {
  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private accountService: AccountService,
    private authGuard: AuthGuard,
    private settingService:SettingService
  ) {
    super(commonService, logService);
    this.viewModel = new AdminDashboardViewModel();
  }
  async ngOnInit() {
    try {
      await  this._commonService.loadDefaultTheme();
    } catch (error) {
      console.error("Error loading theme:", error);
    }
    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;
    if (!await this.authGuard.IsTokenValid) {
      await this._commonService.ShowToastAtTopEnd("Please Login", 'warning');
    }
    await this._commonService.dismissLoader();
    await this.getloggedInUser();
  }
/**
 * Get logged in user
 * @returns
 */
  async getloggedInUser() {
    try {
      await this._commonService.presentLoading();
      let tokenValid = await this.authGuard.IsTokenValid();
      if (!tokenValid) {
        return false;
      } else {
        let user = await this.accountService.getUserFromStorage();
        if (user == "") {
          return false;
        }
        this.viewModel.employee = user;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
    return;
  }
}
