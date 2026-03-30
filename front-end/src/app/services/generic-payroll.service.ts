import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { GenericPayrollClient } from '../clients/generic-payroll.client';
import { ClientGenericPayrollComponentSM } from '../service-models/app/v1/client/client-generic-payroll-component-s-m';
import { ApiResponse } from '../service-models/foundation/api-contracts/base/api-response';
import { ApiRequest } from '../service-models/foundation/api-contracts/base/api-request';

@Injectable({
  providedIn: 'root'
})
export class GenericPayrollService extends BaseService {
  constructor(private genericPayrollClient: GenericPayrollClient) {
    super();
  }

  async getAllGenericPayrollComponents(): Promise<ApiResponse<ClientGenericPayrollComponentSM[]>> {
    return await this.genericPayrollClient.GetAllComponents();
  }

  async getGenericPayrollComponentById(id: number): Promise<ApiResponse<ClientGenericPayrollComponentSM>> {
    if (!id || id <= 0) {
      throw new Error('Invalid ID provided');
    }
    return await this.genericPayrollClient.GetComponentById(id);
  }

  async createGenericPayrollComponent(component: ClientGenericPayrollComponentSM): Promise<ApiResponse<ClientGenericPayrollComponentSM>> {
    if (!component) {
      throw new Error('Invalid component data provided');
    }
    const request = new ApiRequest<ClientGenericPayrollComponentSM>();
    request.reqData = component;
    return await this.genericPayrollClient.CreateComponent(request);
  }

  async updateGenericPayrollComponent(id: number, component: ClientGenericPayrollComponentSM): Promise<ApiResponse<ClientGenericPayrollComponentSM>> {
    if (!id || id <= 0 || !component) {
      throw new Error('Invalid ID or component data provided');
    }
    const request = new ApiRequest<ClientGenericPayrollComponentSM>();
    request.reqData = component;
    return await this.genericPayrollClient.UpdateComponent(id, request);
  }

  async deleteGenericPayrollComponent(id: number): Promise<ApiResponse<void>> {
    if (!id || id <= 0) {
      throw new Error('Invalid ID provided');
    }
    return await this.genericPayrollClient.DeleteComponent(id);
  }
}
