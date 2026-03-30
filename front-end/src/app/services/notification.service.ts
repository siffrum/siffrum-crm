import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { firstValueFrom } from "rxjs";

import { BaseService } from "./base.service";
import { CommonService } from "./common.service";
import { StorageService } from "./storage.service";

@Injectable({
  providedIn: "root",
})
export class NotificationService extends BaseService {
  private baseUrl = "/api/v1/ClientNotification";

  constructor(
    private http: HttpClient,
    private storageService: StorageService,
    private commonService: CommonService
  ) {
    super();
  }

  async getAllNotifications(): Promise<any> {
    try {
      return await firstValueFrom(
        this.http.get<any>(`${this.baseUrl}/my`)
      );
    } catch (error) {
      console.error("getAllNotifications error:", error);
      throw error;
    }
  }

  async getUnreadCount(): Promise<any> {
    try {
      return await firstValueFrom(
        this.http.get<any>(`${this.baseUrl}/my/unread-count`)
      );
    } catch (error) {
      console.error("getUnreadCount error:", error);
      throw error;
    }
  }

  async markAsRead(id: number): Promise<any> {
    try {
      return await firstValueFrom(
        this.http.put<any>(`${this.baseUrl}/my/${id}/read`, {})
      );
    } catch (error) {
      console.error("markAsRead error:", error);
      throw error;
    }
  }

  async deleteNotification(id: number): Promise<any> {
    try {
      return await firstValueFrom(
        this.http.delete<any>(`${this.baseUrl}/my/${id}`)
      );
    } catch (error) {
      console.error("deleteNotification error:", error);
      throw error;
    }
  }

  async markAllAsRead(notificationIds: number[]): Promise<any[]> {
    try {
      const unreadIds = (notificationIds || []).filter((id) => !!id);

      return await Promise.all(
        unreadIds.map((id) => this.markAsRead(id))
      );
    } catch (error) {
      console.error("markAllAsRead error:", error);
      throw error;
    }
  }
}