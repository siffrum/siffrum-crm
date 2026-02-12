import { Injectable } from "@angular/core";
import { AppConstants } from "src/app-constants";
import { PayrollStructureClient } from "../clients/payroll-structure.client";
import { ClientEmployeeCTCDetailSM } from "../service-models/app/v1/client/client-employee-c-t-c-detail-s-m";
import { ClientGenericPayrollComponentSM } from "../service-models/app/v1/client/client-generic-payroll-component-s-m";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { DeleteResponseRoot } from "../service-models/foundation/common-response/delete-response-root";
import { BaseService } from "./base.service";

@Injectable({
  providedIn: "root",
})
export class PayrollService extends BaseService {
  constructor(
    private payrollStructureClient: PayrollStructureClient,
  ) {
    super();
  }
  /**GET Generic Payroll
   * @DEV : Musaib
  */
  async GetAllGenericPayrollComponents(): Promise<
    ApiResponse<ClientGenericPayrollComponentSM[]>
  > {
    return await this.payrollStructureClient.GetAllGenericPayrollComponents();
  }
  /**GET Generic Payroll Data By Id 
   * @DEV : Musaib
  */
  async GetGenericPayrollComponentsById(
    id: number
  ): Promise<ApiResponse<ClientGenericPayrollComponentSM>> {
    if (id <= 0) {
      throw new Error(AppConstants.ErrorPrompts.Delete_Data_Error);
    }
    return await this.payrollStructureClient.GetGenericPayrollComponentsById(
      id
    );
  }
  /** ADD Generic Payroll
   * @DEV : Musaib
  */

  async AddGenericPayrollComponents(addGenericPayrollComponent:ClientGenericPayrollComponentSM): Promise<ApiResponse<ClientGenericPayrollComponentSM>> {
    let apiRequest = new ApiRequest<ClientGenericPayrollComponentSM>();
    apiRequest.reqData = addGenericPayrollComponent;
    return await this.payrollStructureClient.AddGenericPayrollComponents(apiRequest);
  }
  /**Update Generic Payroll
   * @DEV : Musaib
  */
  async updateGenericComponent(updateGenericPayrollComponent:ClientGenericPayrollComponentSM): Promise<ApiResponse<ClientGenericPayrollComponentSM>> {
    let apiRequest = new ApiRequest<ClientGenericPayrollComponentSM>();
    apiRequest.reqData = updateGenericPayrollComponent;
    return await this.payrollStructureClient.UpdateGenericPayrollComponents(apiRequest);
  }
  /**Delete Generic Payroll
   * @DEV : Musaib
  */
  async DeleteGenericPayrollComponents(
    id: number
  ): Promise<ApiResponse<DeleteResponseRoot>> {
    if (id <= 0) {
      throw new Error(AppConstants.ErrorPrompts.Delete_Data_Error);
    }
    return await this.payrollStructureClient.DeleteGenericPayrollComponentsById(
      id
    );
  }


  async GetEmployeeSalaryByCTCId(
    ctcId: number
  ): Promise<ApiResponse<ClientEmployeeCTCDetailSM>> {
    if (ctcId <= 0) {
      throw new Error(AppConstants.ErrorPrompts.Load_Data_Error);
    }
    return await this.payrollStructureClient.GetClientEmployeeSalaryById(ctcId);
  }
  async getGenericComponents(): Promise<ApiResponse<ClientGenericPayrollComponentSM[]>> {
    return this.payrollStructureClient.GetAllGenericPayrollComponents();

  }

}
