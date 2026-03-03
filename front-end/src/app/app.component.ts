import { Component, HostListener, OnDestroy, OnInit } from "@angular/core";
import { NavigationEnd, Router } from "@angular/router";
import { filter, Subscription } from "rxjs";

import { AccountService } from "./services/account.service";
import { CommonService } from "./services/common.service";
import { AuthGuard } from "./guard/auth.guard";
import { RoleTypeSM } from "./service-models/app/enums/role-type-s-m.enum";
import { ThemeService } from "./services/theme.service";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"],
  standalone: false
})
export class AppComponent implements OnInit, OnDestroy {

  title = "CoinManagement";
  private navSub?: Subscription;

  constructor(
    private accountService: AccountService,
    public _commonService: CommonService,
    private router: Router,
    private authGuard: AuthGuard,
    private themeService: ThemeService
  ) {}

  @HostListener("document:keydown", ["$event"])
  handleKeyboardEvent(event: KeyboardEvent): void {
    if (event.ctrlKey && (event.key === "F5" || event.key === "f5")) {
      this.router.navigateByUrl("/dashboard");
      event.preventDefault();
    }
  }

  async ngOnInit() {

    // ✅ Initialize theme (dark/light)
    this.themeService.initTheme();

    // ✅ Apply layout visibility based on route
    this.applyShellVisibility(this.router.url);

    this.navSub = this.router.events
      .pipe(filter((e): e is NavigationEnd => e instanceof NavigationEnd))
      .subscribe((e) => {
        this.applyShellVisibility(e.urlAfterRedirects || e.url);
      });

    // ✅ Load logged user
    await this.getLoggedUser();

    // ✅ Apply theme based on role if token valid
    if (await this.authGuard.IsTokenValid()) {

      const roleType: any =
        RoleTypeSM[this._commonService.layoutVM.tokenRole];

      if (
        roleType === RoleTypeSM.ClientAdmin ||
        roleType === RoleTypeSM.ClientEmployee
      ) {
        await this._commonService.applyThemeGlobally();
      } else {
        await this._commonService.loadDefaultTheme();
      }

    }
  }

  ngOnDestroy(): void {
    this.navSub?.unsubscribe();
  }

  /** ✅ SHOW / HIDE SIDEBAR + TOPBAR */
  private applyShellVisibility(url: string): void {

    const u = (url || "").toLowerCase();

    const hideShellOn = [
      "/website",
      "/login",
      "/register",
      "/forgotpassword",
      "/resetpassword",
      "/changepassword",
      "/license",
      "/success",
      "/failure",
      "/admin/login"
    ];

    const shouldHide =
      hideShellOn.some(p => u === p || u.startsWith(p + "/")) ||
      u.startsWith("/admin");

    this._commonService.layoutVM.showLeftSideMenu = !shouldHide;
  }

  /** ✅ FIXED FUNCTION (NO TYPE ERROR) */
  async getLoggedUser(): Promise<boolean> {

    const user = await this.accountService.getUserFromStorage();

    // If no user stored
    if (!user) return false;

    // Extra safety check
    if (!user.loginId) return false;

    this._commonService.layoutVM.loggedUserName = user.loginId;
    this._commonService.layoutVM.tokenRole = user.roleType;

    return true;
  }

  async logOutUser() {
    await this.accountService.logoutUser();
    await this._commonService.ShowToastAtTopEnd(
      "Log Out Successful",
      "warning"
    );
    this.router.navigateByUrl("/login");
  }
}