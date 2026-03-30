import { Injectable, OnDestroy } from "@angular/core";
import { BehaviorSubject, Subscription, timer } from "rxjs";

import { NotificationService } from "./notification.service";
import { NotificationSM } from "src/app/service-models/notification/notification.model";

@Injectable({
  providedIn: "root",
})
export class NotificationStateService implements OnDestroy {
  private unreadCountSubject = new BehaviorSubject<number>(0);
  unreadCount$ = this.unreadCountSubject.asObservable();

  private notificationsSubject = new BehaviorSubject<NotificationSM[]>([]);
  notifications$ = this.notificationsSubject.asObservable();

  private pollingSub?: Subscription;
  private isPollingStarted = false;

  constructor(private notificationService: NotificationService) {}

  /** =========================
   * GETTERS
   * ========================= */
  get unreadCountValue(): number {
    return this.unreadCountSubject.value;
  }

  get notificationsValue(): NotificationSM[] {
    return this.notificationsSubject.value;
  }

  /** =========================
   * MAIN REFRESH
   * ========================= */
  async refreshAll(): Promise<void> {
    await this.refreshNotifications();
    await this.refreshUnreadCount();
  }

  /** =========================
   * FETCH NOTIFICATIONS
   * ========================= */
  async refreshNotifications(): Promise<void> {
    try {
      const resp = await this.notificationService.getAllNotifications();

      const data = Array.isArray(resp)
        ? resp
        : resp?.data || resp?.successData || resp?.items || [];

      const mapped: NotificationSM[] = (data || []).map(
        (item: any, index: number) => ({
          id: item.id ?? index + 1,
          title: item.title || item.subject || "Notification",
          message: item.message || item.description || "No message",
          time:
            item.createdOn ||
            item.createdDate ||
            item.createdAt ||
            "Recently",
          type: this.mapType(item.moduleName || item.type || "general"),
          isRead: item.isRead ?? item.read ?? false,
          createdOn: item.createdOn,
          createdDate: item.createdDate,
          moduleName: item.moduleName,
          redirectUrl: item.redirectUrl,
          rawData: item,
        })
      );

      this.notificationsSubject.next(mapped);
      this.syncUnreadFromList(mapped);
    } catch (error) {
      console.error("Failed to refresh notifications", error);
    }
  }

  /** =========================
   * FETCH UNREAD COUNT
   * ========================= */
  async refreshUnreadCount(): Promise<void> {
    try {
      const resp = await this.notificationService.getUnreadCount();

      const count =
        resp?.unreadCount ??
        resp?.count ??
        resp?.data?.unreadCount ??
        resp?.successData?.unreadCount ??
        this.notificationsValue.filter((item) => !item.isRead).length;

      this.unreadCountSubject.next(count);
    } catch (error) {
      this.syncUnreadFromList(this.notificationsValue);
    }
  }

  /** =========================
   * ACTIONS
   * ========================= */

  async markAsRead(id: number): Promise<boolean> {
    try {
      await this.notificationService.markAsRead(id);

      const updated = this.notificationsValue.map((item) =>
        item.id === id ? { ...item, isRead: true } : item
      );

      this.notificationsSubject.next(updated);
      this.syncUnreadFromList(updated);
      return true;
    } catch (error) {
      console.error("Failed to mark as read", error);
      return false;
    }
  }

  /**
   * No backend endpoint → loop through all unread
   */
  async markAllAsRead(): Promise<boolean> {
    try {
      const unreadIds = this.notificationsValue
        .filter((item) => !item.isRead)
        .map((item) => item.id);

      if (unreadIds.length === 0) return true;

      await Promise.all(
        unreadIds.map((id) => this.notificationService.markAsRead(id))
      );

      const updated = this.notificationsValue.map((item) => ({
        ...item,
        isRead: true,
      }));

      this.notificationsSubject.next(updated);
      this.unreadCountSubject.next(0);
      return true;
    } catch (error) {
      console.error("Failed to mark all as read", error);
      return false;
    }
  }

  async deleteNotification(id: number): Promise<boolean> {
    try {
      await this.notificationService.deleteNotification(id);

      const updated = this.notificationsValue.filter(
        (item) => item.id !== id
      );

      this.notificationsSubject.next(updated);
      this.syncUnreadFromList(updated);
      return true;
    } catch (error) {
      console.error("Failed to delete notification", error);
      return false;
    }
  }

  /** =========================
   * POLLING (REALTIME SIMULATION)
   * ========================= */
  startPolling(intervalMs: number = 20000): void {
    if (this.isPollingStarted) return;

    this.isPollingStarted = true;

    this.pollingSub = timer(0, intervalMs).subscribe(async () => {
      await this.refreshAll();
    });
  }

  stopPolling(): void {
    this.pollingSub?.unsubscribe();
    this.pollingSub = undefined;
    this.isPollingStarted = false;
  }

  /** =========================
   * HELPERS
   * ========================= */
  private syncUnreadFromList(list: NotificationSM[]): void {
    this.unreadCountSubject.next(
      list.filter((item) => !item.isRead).length
    );
  }

  private mapType(
    type: string
  ): "general" | "leave" | "payroll" | "employee" | "system" {
    const t = (type || "").toLowerCase();

    if (t.includes("leave")) return "leave";
    if (t.includes("payroll")) return "payroll";
    if (t.includes("employee")) return "employee";
    if (t.includes("system")) return "system";

    return "general";
  }

  ngOnDestroy(): void {
    this.stopPolling();
  }
}