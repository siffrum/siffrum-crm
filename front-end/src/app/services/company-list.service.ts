import { Injectable } from "@angular/core";
import { AppConstants } from "src/app-constants";
import { CompanyListClient } from "../clients/company-list.client";
import { ClientCompanyDetailSM } from "../service-models/app/v1/client/client-company-detail-s-m";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { QueryFilter } from "../service-models/foundation/api-contracts/query-filter";
import { DeleteResponseRoot } from "../service-models/foundation/common-response/delete-response-root";
import { IntResponseRoot } from "../service-models/foundation/common-response/int-response-root";
import { BaseService } from "./base.service";
import { SuperCompanyListViewModel } from "../view-models/admin-company-list.viewmodel";

@Injectable({
  providedIn: "root",
})
export class CompanyListService extends BaseService {
  constructor(private companyListclient: CompanyListClient) {
    super();
  }
  /**Get all Companies list ByOdata */
  async getAllCompaniesByOdata(viewModel:SuperCompanyListViewModel): Promise<ApiResponse<ClientCompanyDetailSM[]>> {
    let queryFilter= new QueryFilter()
    queryFilter.skip=(viewModel.pagination.PageNo-1)*viewModel.pagination.PageSize;
    queryFilter.top=viewModel.pagination.PageSize;
    return await this.companyListclient.GetAllCompanieslistByOdata(queryFilter);
  }
  /**Get all Companies list */
  async getAllCompanyList(): Promise<ApiResponse<ClientCompanyDetailSM[]>> {
    return await this.companyListclient.GetAllCompanyList();
  }
  /** Get Total Company Count */
  async getAllCompaniesCount(): Promise<ApiResponse<IntResponseRoot>> {
    return await this.companyListclient.GetAllCompaniesCount();
  }
  /**Update Company status */
    /**update user login status */
    async updateCompanyStatus(id:number, updateCompanyStatus: boolean): Promise<ApiResponse<DeleteResponseRoot>> {
      if (updateCompanyStatus == null) {
        throw new Error(AppConstants.ErrorPrompts.Invalid_Input_Data);
      } else {
        return await this.companyListclient.UpdateCompanyStatus(id,updateCompanyStatus);
      }
    }
}
