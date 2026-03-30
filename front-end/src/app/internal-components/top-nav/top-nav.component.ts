import { Component, HostListener, OnDestroy, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Subscription } from "rxjs";

import { BaseComponent } from "src/app/components/base.component";
import { AccountService } from "src/app/services/account.service";
import { CommonService } from "src/app/services/common.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { NotificationStateService } from "src/app/services/notification-state.service";
import { NotificationSM } from "src/app/service-models/notification/notification.model";
import { TopNavViewModel } from "src/app/view-models/top-nav.viewmodel";

@Component({
  selector: "app-top-nav",
  templateUrl: "./top-nav.component.html",
  styleUrls: ["./top-nav.component.scss"],
  standalone: false,
})
export class TopNavComponent
  extends BaseComponent<TopNavViewModel>
  implements OnInit, OnDestroy
{
  showNotificationsDropdown = false;
  unreadCount = 0;
  notificationsPreview: NotificationSM[] = [];

  private unreadSub?: Subscription;
  private notificationsSub?: Subscription;

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private accountService: AccountService,
    private router: Router,
    private notificationStateService: NotificationStateService
  ) {
    super(commonService, logService);
    this.viewModel = new TopNavViewModel();
  }

  async ngOnInit(): Promise<void> {
    if (!this.isPublicRoute()) {
      this.notificationStateService.startPolling();

      this.unreadSub = this.notificationStateService.unreadCount$.subscribe(
        (count) => {
          this.unreadCount = count;
        }
      );

      this.notificationsSub =
        this.notificationStateService.notifications$.subscribe((items) => {
          this.notificationsPreview = items.slice(0, 5);
        });

      await this.notificationStateService.refreshAll();
    }
  }

  ngOnDestroy(): void {
    this.unreadSub?.unsubscribe();
    this.notificationsSub?.unsubscribe();
  }

  private isPublicRoute(): boolean {
    const u = (this.router.url || "").toLowerCase();

    const publicRoutes = [
      "/website",
      "/login",
      "/register",
      "/forgotpassword",
      "/resetpassword",
      "/changepassword",
      "/license",
      "/success",
      "/failure",
      "/admin/login",
    ];

    return publicRoutes.some((p) => u === p || u.startsWith(p + "/"));
  }

  toggleMenu(): void {
    if (this.isPublicRoute()) {
      return;
    }

    if (this._commonService.layoutVM.toggleSideMenu === "default") {
      this._commonService.layoutVM.toggleSideMenu = "active";
      this._commonService.layoutVM.toogleWrapper = "toggleWrapper";
    } else {
      this._commonService.layoutVM.toggleSideMenu = "default";
      this._commonService.layoutVM.toogleWrapper = "wrapper";
    }
  }

  toggleNotifications(event: Event): void {
    event.stopPropagation();
    this.showNotificationsDropdown = !this.showNotificationsDropdown;
  }

  async openNotification(item: NotificationSM): Promise<void> {
    if (!item.isRead) {
      await this.notificationStateService.markAsRead(item.id);
    }

    this.showNotificationsDropdown = false;
    this.router.navigateByUrl("/notifications");
  }

  goToNotifications(): void {
    this.showNotificationsDropdown = false;
    this.router.navigateByUrl("/notifications");
  }

  async markAllAsRead(): Promise<void> {
    await this.notificationStateService.markAllAsRead();
  }

  getNotificationIcon(type: string): string {
    switch (type) {
      case "leave":
        return "bi bi-calendar2-check";
      case "payroll":
        return "bi bi-cash-stack";
      case "employee":
        return "bi bi-person-badge";
      case "system":
        return "bi bi-gear";
      default:
        return "bi bi-bell";
    }
  }

  @HostListener("document:click")
  closeNotificationsDropdown(): void {
    this.showNotificationsDropdown = false;
  }

  async logOutUser(): Promise<void> {
    localStorage.removeItem("theme");
    await this.accountService.logoutUser();
    await this._commonService.ShowToastAtTopEnd(
      "Log Out Successful",
      "warning"
    );
    this.router.navigateByUrl("/login");
  }
}