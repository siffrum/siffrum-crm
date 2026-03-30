import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { CompanyDetailClient } from '../clients/company-detail.client';
import { ClientCompanyDetailSM } from '../service-models/app/v1/client/client-company-detail-s-m';
import { ApiResponse } from '../service-models/foundation/api-contracts/base/api-response';
import { ApiRequest } from '../service-models/foundation/api-contracts/base/api-request';

@Injectable({
  providedIn: 'root'
})
export class CompanyDetailService extends BaseService {
  constructor(private companyDetailClient: CompanyDetailClient) {
    super();
  }

  /**
   * Get company details for logged-in company
   */
  async getCompanyDetails(): Promise<ApiResponse<ClientCompanyDetailSM>> {
    return await this.companyDetailClient.GetCompanyDetails();
  }

  /**
   * Get company detail by ID
   */
  async getCompanyDetailById(companyId: number): Promise<ApiResponse<ClientCompanyDetailSM>> {
    if (!companyId || companyId <= 0) {
      throw new Error('Valid company ID is required');
    }
    return await this.companyDetailClient.GetCompanyDetailById(companyId);
  }

  /**
   * Update company details
   */
  async updateCompanyDetail(companyDetail: ClientCompanyDetailSM): Promise<ApiResponse<ClientCompanyDetailSM>> {
    if (!companyDetail || !companyDetail.id) {
      throw new Error('Company detail with valid ID is required');
    }
    const apiRequest = new ApiRequest<ClientCompanyDetailSM>();
    apiRequest.reqData = companyDetail;
    return await this.companyDetailClient.UpdateCompanyDetail(apiRequest);
  }

  /**
   * Get company logo
   */
  async getCompanyLogo(): Promise<ApiResponse<any>> {
    return await this.companyDetailClient.GetCompanyLogo();
  }

  /**
   * Upload company logo
   */
  async uploadCompanyLogo(logoFile: any): Promise<ApiResponse<any>> {
    if (!logoFile) {
      throw new Error('Logo file is required');
    }
    const apiRequest = new ApiRequest<any>();
    apiRequest.reqData = logoFile;
    return await this.companyDetailClient.UploadCompanyLogo(apiRequest);
  }

  /**
   * Delete company logo
   */
  async deleteCompanyLogo(): Promise<ApiResponse<any>> {
    return await this.companyDetailClient.DeleteCompanyLogo();
  }
}
