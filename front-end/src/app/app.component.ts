import { Component, HostListener, OnInit } from "@angular/core";
import { AccountService } from "./services/account.service";
import { CommonService } from "./services/common.service";
import { AuthGuard } from "./guard/auth.guard";
import { RoleTypeSM } from "./service-models/app/enums/role-type-s-m.enum";
import { Router } from "@angular/router";
import { ThemeService } from "./services/theme.service"; // ✅ added

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"],
  standalone: false
})
export class AppComponent implements OnInit {
  title = "CoinManagement";
  currentUrl: string = "";
  isDashboardVisible: Boolean = false;
  protected _commonService: CommonService;

  constructor(
    private accountService: AccountService,
    private commonService: CommonService,
    private router: Router,
    private authGuard: AuthGuard,
    private themeService: ThemeService // ✅ added
  ) {
    this._commonService = commonService;
  }

  @HostListener('document:keydown', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent): void {
    if (event.ctrlKey && event.key === 'F5') {
      this.router.navigateByUrl('/dashboard');
      event.preventDefault(); // Prevent the default browser refresh behavior
    }
  }

  async ngOnInit() {
    // ✅ added: initialize dark/light mode class from localStorage/system preference
    this.themeService.initTheme();

    // await this.loadThemeFromStorage();
    await this.getLoggedUser();
    // let roleType = this._commonService.layoutVM.tokenRole;
    if (await this.authGuard.IsTokenValid()) {
      /**Set theme Globally Dynamically*/
      this._commonService.layoutVM.showLeftSideMenu = true;
      let roleType: any = RoleTypeSM[this._commonService.layoutVM.tokenRole];
      if (roleType === RoleTypeSM.ClientAdmin || roleType === RoleTypeSM.ClientEmployee) {
        await this._commonService.applyThemeGlobally();
        // let resp= this._commonService.applyThemeGlobally();
        // console.log(resp)
      } else {
        await this._commonService.loadDefaultTheme();
      }
    }
  }

  async getLoggedUser() {
    let user = await this.accountService.getUserFromStorage();
    if (user == "") {
      return false;
    } else {
      this._commonService.layoutVM.loggedUserName = user.loginId;
      this._commonService.layoutVM.tokenRole = user.roleType;
    }
    return;
  }

  async logOutUser() {
    await this.accountService.logoutUser();
    await this._commonService.ShowToastAtTopEnd(
      "Log Out Successful",
      "warning"
    );
  }
}
