export interface NotificationSM {
  id: number;
  title: string;
  message: string;
  time: string;
  type: "general" | "leave" | "payroll" | "employee" | "system";
  isRead: boolean;
  createdOn?: string;
  createdDate?: string;
  moduleName?: string;
  redirectUrl?: string;
  rawData?: any;
}

export interface NotificationApiResponseSM<T = any> {
  isError: boolean;
  successData: T | null;
  error?: any;
}

export interface NotificationCountSM {
  unreadCount: number;
}