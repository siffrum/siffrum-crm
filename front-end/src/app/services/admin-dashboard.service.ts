import { Injectable } from '@angular/core';
import { StorageService } from './storage.service';
import { CommonService } from './common.service';
import { BaseService } from './base.service';
import { AdminDashboardClient } from '../clients/admin-dashboard.client';
import { DashBoardSM } from '../service-models/app/v1/client/dash-board-s-m';
import { ApiResponse } from '../service-models/foundation/api-contracts/base/api-response';

@Injectable({
  providedIn: 'root'
})
export class AdminDashboardService extends BaseService {

  constructor(
    private adminDashboardClient:AdminDashboardClient,
    private storageService: StorageService,
    private commonService: CommonService
  ) {
    super();
  }
  /**
   * @Dev Musaib
   * Get All Data For Dashboard
   * @returns
   */
  async getAllDachboardDataItems(): Promise<ApiResponse<DashBoardSM>> {
    return await this.adminDashboardClient.GetAllDasBoardDataItemsList();
  }
}
