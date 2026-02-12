import { Component,  OnInit} from '@angular/core';
import { BaseComponent } from 'src/app/components/base.component';
import { AccountService } from 'src/app/services/account.service';
import { CommonService } from 'src/app/services/common.service';
import { LogHandlerService } from 'src/app/services/log-handler.service';
import { SideMenuService } from 'src/app/services/side-menu.service';
import { TopNavViewModel } from 'src/app/view-models/top-nav.viewmodel';
@Component({
    selector: 'app-top-nav',
    templateUrl: './top-nav.component.html',
    styleUrls: ['./top-nav.component.scss'],
    standalone: false
})
export class TopNavComponent extends BaseComponent<TopNavViewModel> implements OnInit {
  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private accountService: AccountService,
  ) {
    super(commonService, logService);
    this.viewModel = new TopNavViewModel();
  }
  async ngOnInit() {
  }


  toggleMenu() {
    if (this._commonService.layoutVM.toggleSideMenu == 'default') {
      this._commonService.layoutVM.toggleSideMenu = 'active';
      this._commonService.layoutVM.toogleWrapper = 'toggleWrapper';
    } else {
      this._commonService.layoutVM.toggleSideMenu = 'default';
      this._commonService.layoutVM.toogleWrapper = 'wrapper';
    }
    return;
  }
  async logOutUser() {
      // Clear stored theme data
    localStorage.removeItem('theme');
    await this.accountService.logoutUser();
    await this._commonService.ShowToastAtTopEnd("Log Out Successful", 'warning');
  }


}
