import { Injectable } from "@angular/core";
import { BaseApiClient } from "./base-client/base-api.client";
import { StorageService } from "../services/storage.service";
import { StorageCache } from "./helpers/storage-cache.helper";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { AppConstants } from "src/app-constants";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";

@Injectable({
  providedIn: "root",
})
export class NotificationClient extends BaseApiClient {
  constructor(
    storageService: StorageService,
    storageCache: StorageCache,
    commonResponseCodeHandler: CommonResponseCodeHandler
  ) {
    super(storageService, storageCache, commonResponseCodeHandler);
  }

  /**
   * Get all notifications for logged-in user
   * @returns ApiResponse<any[]>
   */
  GetAllNotifications = async (): Promise<ApiResponse<any[]>> => {
    try {
      let resp = await this.GetResponseAsync<null, any[]>(
        `${AppConstants.ApiUrls.NOTIFICATION_URL}/my`,
        "GET"
      );
      return resp;
    } catch (error) {
      console.error('Error fetching notifications:', error);
      throw error;
    }
  };

  /**
   * Get count of unread notifications
   * @returns ApiResponse<number>
   */
  GetUnreadCount = async (): Promise<ApiResponse<number>> => {
    try {
      let resp = await this.GetResponseAsync<null, number>(
        `${AppConstants.ApiUrls.NOTIFICATION_URL}/my/unread-count`,
        "GET"
      );
      return resp;
    } catch (error) {
      console.error('Error fetching unread notification count:', error);
      throw error;
    }
  };

  /**
   * Mark notification as read
   * @param notificationId - Notification ID
   * @returns ApiResponse<any>
   */
  MarkAsRead = async (notificationId: number): Promise<ApiResponse<any>> => {
    if (!notificationId || notificationId <= 0) {
      throw new Error('Valid notification ID is required');
    }
    try {
      let resp = await this.GetResponseAsync<number, any>(
        `${AppConstants.ApiUrls.NOTIFICATION_URL}/my/${notificationId}/read`,
        "PUT"
      );
      return resp;
    } catch (error) {
      console.error('Error marking notification as read:', error);
      throw error;
    }
  };

  /**
   * Delete notification
   * @param notificationId - Notification ID
   * @returns ApiResponse<any>
   */
  DeleteNotification = async (
    notificationId: number
  ): Promise<ApiResponse<any>> => {
    if (!notificationId || notificationId <= 0) {
      throw new Error('Valid notification ID is required');
    }
    try {
      let resp = await this.GetResponseAsync<number, any>(
        `${AppConstants.ApiUrls.NOTIFICATION_URL}/my/${notificationId}`,
        "DELETE"
      );
      return resp;
    } catch (error) {
      console.error('Error deleting notification:', error);
      throw error;
    }
  };
}
