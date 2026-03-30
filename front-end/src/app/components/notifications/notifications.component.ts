import { Component, OnDestroy, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Subscription } from "rxjs";

import { CommonService } from "src/app/services/common.service";
import { NotificationStateService } from "src/app/services/notification-state.service";
import { NotificationSM } from "src/app/service-models/notification/notification.model";

@Component({
  selector: "app-notifications",
  standalone: true,
  imports: [CommonModule],
  templateUrl: "./notifications.component.html",
  styleUrls: ["./notifications.component.scss"],
})
export class NotificationsComponent implements OnInit, OnDestroy {
  isLoading = false;
  unreadCount = 0;
  activeFilter: "all" | "unread" | "read" = "all";

  notifications: NotificationSM[] = [];

  private notificationsSub?: Subscription;
  private unreadSub?: Subscription;

  constructor(
    public _commonService: CommonService,
    private notificationStateService: NotificationStateService
  ) {}

  async ngOnInit(): Promise<void> {
    this._commonService.layoutVM.PageTitle = "Notifications";

    this.notificationStateService.startPolling();

    this.notificationsSub =
      this.notificationStateService.notifications$.subscribe((items) => {
        this.notifications = items;
      });

    this.unreadSub = this.notificationStateService.unreadCount$.subscribe(
      (count) => {
        this.unreadCount = count;
      }
    );

    this.isLoading = true;
    await this.notificationStateService.refreshAll();
    this.isLoading = false;
  }

  ngOnDestroy(): void {
    this.notificationsSub?.unsubscribe();
    this.unreadSub?.unsubscribe();
  }

  get filteredNotifications(): NotificationSM[] {
    if (this.activeFilter === "unread") {
      return this.notifications.filter((item) => !item.isRead);
    }

    if (this.activeFilter === "read") {
      return this.notifications.filter((item) => item.isRead);
    }

    return this.notifications;
  }

  setFilter(filter: "all" | "unread" | "read"): void {
    this.activeFilter = filter;
  }

  async markAsRead(item: NotificationSM): Promise<void> {
    const ok = await this.notificationStateService.markAsRead(item.id);

    if (ok) {
      await this._commonService.ShowToastAtTopEnd("Marked as read", "success");
    }
  }

  async markAllAsRead(): Promise<void> {
    const ok = await this.notificationStateService.markAllAsRead();

    if (ok) {
      await this._commonService.ShowToastAtTopEnd(
        "All marked as read",
        "success"
      );
    }
  }

  async deleteNotification(id: number): Promise<void> {
    const ok = await this.notificationStateService.deleteNotification(id);

    if (ok) {
      await this._commonService.ShowToastAtTopEnd(
        "Deleted successfully",
        "success"
      );
    }
  }

  getTypeIcon(type: string): string {
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

  getTypeLabel(type: string): string {
    switch (type) {
      case "leave":
        return "Leave";
      case "payroll":
        return "Payroll";
      case "employee":
        return "Employee";
      case "system":
        return "System";
      default:
        return "General";
    }
  }
}