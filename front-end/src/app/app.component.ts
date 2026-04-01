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
    const isPublicRoute = !this._commonService.layoutVM.showLeftSideMenu;

    // Only force dashboard on protected CRM pages
    if (!isPublicRoute && event.ctrlKey && (event.key === "F5" || event.key === "f5")) {
      this.router.navigateByUrl("/dashboard");
      event.preventDefault();
    }
  }

  async ngOnInit(): Promise<void> {
    // Theme init always okay
    this.themeService.initTheme();

    // Initial shell visibility
    await this.applyShellVisibility(this.router.url);

    this.navSub = this.router.events
      .pipe(filter((e): e is NavigationEnd => e instanceof NavigationEnd))
      .subscribe(async (e) => {
        const currentUrl = e.urlAfterRedirects || e.url;
        await this.applyShellVisibility(currentUrl);
      });

    const currentUrl = this.router.url;

    // ✅ VERY IMPORTANT:
    // Skip auth/user loading for public website routes
    if (await this.isPublicRoute(currentUrl)) {
      this.resetPublicLayoutState();
      return;
    }

    // Protected area only
    await this.getLoggedUser();

    if (await this.authGuard.IsTokenValid()) {
      const roleType = this._commonService.layoutVM.tokenRole;

      if (
        roleType === RoleTypeSM.ClientAdmin ||
        roleType === RoleTypeSM.ClientEmployee
      ) {
        await this._commonService.applyThemeGlobally();
      } else {
        await this._commonService.loadDefaultTheme();
      }
    } else {
      // fallback if token invalid
      await this._commonService.loadDefaultTheme();
    }
  }

  ngOnDestroy(): void {
    this.navSub?.unsubscribe();
  }

  private async isPublicRoute(url: string): Promise<boolean> {
    const u = (url || "").toLowerCase();

    const publicRoutes = [
      "/website",
      "/login",
      "/register",
      "/forgotpassword",
      "/resetpassword",
      "/changepassword",
      "/success",
      "/failure",
      "/admin/login"
    ];

    return (
      publicRoutes.some((p) => u === p || u.startsWith(p + "/")) ||
      u.startsWith("/admin/login")
    );
  }

  /** Show / hide CRM shell */
  private async applyShellVisibility(url: string): Promise<void> {
    const shouldHide =
      (await this.isPublicRoute(url)) ||
      (url || "").toLowerCase().startsWith("/admin");
    this._commonService.layoutVM.showLeftSideMenu = !shouldHide;
  }

  /** Reset layout values for public website pages */
  private resetPublicLayoutState(): void {
    this._commonService.layoutVM.showLeftSideMenu = false;
    this._commonService.layoutVM.toggleSideMenu = "default";
    this._commonService.layoutVM.toogleWrapper = "wrapper";
    this._commonService.layoutVM.loggedUserName = "";
  }

  async getLoggedUser(): Promise<boolean> {
    try {
      const user = await this.accountService.getUserFromStorage();

      if (!user) return false;
      if (!user.loginId) return false;

      this._commonService.layoutVM.loggedUserName = user.loginId;
      this._commonService.layoutVM.tokenRole = user.roleType;

      return true;
    } catch (error) {
      console.error("Error while loading logged user:", error);
      return false;
    }
  }

  async logOutUser(): Promise<void> {
    await this.accountService.logoutUser();
    await this._commonService.ShowToastAtTopEnd(
      "Log Out Successful",
      "warning"
    );
    this.router.navigateByUrl("/login");
  }
}
