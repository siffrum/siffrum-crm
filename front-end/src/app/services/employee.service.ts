import { Injectable } from "@angular/core";
import { AddressClient } from "../clients/address.client";
import { BankClient } from "../clients/bank.client";
import { EmployeesClient } from "../clients/employees.client";
import { SalaryClient } from "../clients/salary.client";
import { ClientUserAddressSM } from "../service-models/app/v1/app-users/client-user-address-s-m";
import { ClientUserSM } from "../service-models/app/v1/app-users/client-user-s-m";
import { ClientEmployeeBankDetailSM } from "../service-models/app/v1/client/client-employee-bank-detail-s-m";
import { ClientEmployeeCTCDetailSM } from "../service-models/app/v1/client/client-employee-c-t-c-detail-s-m";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { QueryFilter } from "../service-models/foundation/api-contracts/query-filter";
import { DeleteResponseRoot } from "../service-models/foundation/common-response/delete-response-root";
import { IntResponseRoot } from "../service-models/foundation/common-response/int-response-root";
import { BaseService } from "./base.service";
import { EmployeesListViewModel } from "../view-models/employees-list.viewmodel";
import { EmployeeAddressInfoViewModel } from "../view-models/employee-address-info.viewmodel";
import { EmployeeBankInfoViewModel } from "../view-models/employee-bank-info.viewmodel";
import { EmployeeSalaryInfoViewModel } from "../view-models/employee-salary-info.viewmodel";
import { BoolResponseRoot } from "../service-models/foundation/common-response/bool-response-root";

@Injectable({
  providedIn: "root",
})
export class EmployeeService extends BaseService {
  constructor(
    private employeeClient: EmployeesClient,
    private addressClient: AddressClient,
    private bankClient: BankClient,
    private salaryClient: SalaryClient
  ) {
    super();
  }

  /** Employee-Info Section Starts  Bismillah Barkat*/

  /** Get all Employees */
  async getAllEmployees(): Promise<ApiResponse<ClientUserSM[]>> {
    return await this.employeeClient.GetAllEmployees();
  }

  /** Get all Employees by odata */
  async getAllEmployeeByCompanyIdAndOdata(viewModel: EmployeesListViewModel, companyId: number): Promise<ApiResponse<ClientUserSM[]>> {
    let queryFilter = new QueryFilter();
    queryFilter.skip = (viewModel.pagination.PageNo - 1) * viewModel.pagination.PageSize;
    queryFilter.top = viewModel.pagination.PageSize;
    return await this.employeeClient.GetAllEmployeeByCompanyIdAndOdata(queryFilter, companyId);
  }

  /** Get Employee By Employee ID  */
  async getEmployeeByEmployeeId(
    employeeId: number
  ): Promise<ApiResponse<ClientUserSM>> {
    return await this.employeeClient.GetEmployeeByEmployeeId(employeeId);
  }

  /** Get Employee by Mine Endpoints */
  getEmployeeByMineEndpoint = async (): Promise<ApiResponse<ClientUserSM>> => {
    return await this.employeeClient.GetEmployeeByMineEndpoint();
  };

  /** Add New Employee  */
  async addEmployee(
    employee: ClientUserSM
  ): Promise<ApiResponse<ClientUserSM>> {
    let apiRequest = new ApiRequest<ClientUserSM>();
    apiRequest.reqData = employee;
    return await this.employeeClient.AddNewEmployee(apiRequest);
  }

  /** Update Employee Info By Employee Id  */
  async updateEmployeeInfo(
    employee: ClientUserSM
  ): Promise<ApiResponse<ClientUserSM>> {
    let apiRequest = new ApiRequest<ClientUserSM>();
    // employee.dateOfJoining = this.convertToUtc(employee.dateOfJoining);
    apiRequest.reqData = employee;
    return await this.employeeClient.UpdateEmployee(apiRequest);
  }

  /** Delete Employee By Employee Id  */
  async deleteEmployee(id: number): Promise<ApiResponse<DeleteResponseRoot>> {
    return await this.employeeClient.DeleteEmployee(id);
  }

  /** Get Total Employee Count of a Company */
  async getEmployeeCountOfCompany(): Promise<ApiResponse<IntResponseRoot>> {
    return await this.employeeClient.GetEmployeeCountOfCompany();
  }
  /** Get Total Employee address Count
   * @param id
   */
  async getEmployeeAddressCount(
    empId: number
  ): Promise<ApiResponse<IntResponseRoot>> {
    return await this.employeeClient.GetEmployeeAddressCount(empId);
  }

  /** Employee-Info Section Ends */

  /** Employee-Address Section Starts */

  /** Get Employee Address By Employee Id oData */
  async getEmployeeAddressByCompanyIdAndEmployeeIdWithOData(viewModel: EmployeeAddressInfoViewModel, companyId: number, employeeId: number): Promise<ApiResponse<ClientUserAddressSM[]>> {
    let queryFilter = new QueryFilter();
    queryFilter.skip = (viewModel.pagination.PageNo - 1) * viewModel.pagination.PageSize;
    queryFilter.top = viewModel.pagination.PageSize
    return await this.addressClient.GetEmployeeAddressByCompanyIdAndEmployeeIdWithOData(
      queryFilter,
      companyId,
      employeeId
    );
  }

  /** Get Employee Address-Info By Employee Id*/
  async getEmployeeAddressByEmployeeId(
    employeeId: number
  ): Promise<ApiResponse<ClientUserAddressSM[]>> {
    return await this.addressClient.GetEmployeeAddressByEmployeeId(employeeId);
  }

  /** Add Employee Address-Info */
  async addEmployeeAddress(
    employee: ClientUserAddressSM
  ): Promise<ApiResponse<ClientUserAddressSM>> {
    let apiRequest = new ApiRequest<ClientUserAddressSM>();
    apiRequest.reqData = employee;
    return await this.addressClient.AddEmployeeAddress(apiRequest);
  }

  /** Update Employee Address-Info By Address Id */
  async updateEmployeeAddressByAddressId(
    employeeAddress: ClientUserAddressSM
  ): Promise<ApiResponse<ClientUserAddressSM>> {
    let apiRequest = new ApiRequest<ClientUserAddressSM>();
    apiRequest.reqData = employeeAddress;
    return await this.addressClient.UpdateEmployeeAddressByAddressId(
      apiRequest
    );
  }

  /** Get Employee Address-Info By Address Id*/
  async getEmployeeAddressByAddressId(
    addressId: number
  ): Promise<ApiResponse<ClientUserAddressSM>> {
    return await this.addressClient.GetEmployeeAddressByAddressId(addressId);
  }

  /** Delete Employee Address-Info By Address Id*/
  async deleteEmployeeAddressByAddressId(
    addressId: number
  ): Promise<ApiResponse<DeleteResponseRoot>> {
    return await this.addressClient.DeleteEmployeeAddressInfoByAddressId(
      addressId
    );
  }
  /** Employee-Address Section Ends */

  /** Employee-Bank Section Starts */

  /** Get Employee Bank-Info By Employee Id oData*/
  async getEmployeeBankDetailsByCompanyIdAndEmployeeIdWithOData(viewModel: EmployeeBankInfoViewModel, companyId: number,
    employeeId: number
  ): Promise<ApiResponse<ClientEmployeeBankDetailSM[]>> {
    let queryFilter = new QueryFilter();
    queryFilter.skip = (viewModel.pagination.PageNo - 1) * viewModel.pagination.PageSize;
    queryFilter.top = viewModel.pagination.PageSize;
    return await this.bankClient.GetEmployeeBankByCompanyIdAndEmployeeIdWithOData(
      queryFilter,
      companyId,
      employeeId
    );
  }

  /** Get Employee Bank-Info By Employee Id */
  async getEmployeeBankByEmployeeId(
    employeeId: number
  ): Promise<ApiResponse<ClientEmployeeBankDetailSM[]>> {
    return await this.bankClient.GetEmployeeBankByEmployeeId(employeeId);
  }

  /** Add Employee Bank-Info */
  async addEmployeeBankInfo(
    employeeBank: ClientEmployeeBankDetailSM
  ): Promise<ApiResponse<ClientEmployeeBankDetailSM>> {
    let apiRequest = new ApiRequest<ClientEmployeeBankDetailSM>();
    apiRequest.reqData = employeeBank;
    return await this.bankClient.AddEmployeeBankInfo(apiRequest);
  }

  /** Update Employee Bank-Info By Bank Id */
  async updateEmployeeBankByBankId(
    employeeAddress: ClientEmployeeBankDetailSM
  ): Promise<ApiResponse<ClientEmployeeBankDetailSM>> {
    let apiRequest = new ApiRequest<ClientEmployeeBankDetailSM>();
    apiRequest.reqData = employeeAddress;
    return await this.bankClient.UpdateEmployeeBankByBankId(apiRequest);
  }

  /** Get Employee Bank-Info By Bank Id*/
  async getEmployeeBankByBankId(
    bankId: number
  ): Promise<ApiResponse<ClientEmployeeBankDetailSM>> {
    return await this.bankClient.GetEmployeeBankByBankId(bankId);
  }

  /** Delete Employee Bank-Info By Bank Id*/
  async deleteEmployeeBankByBankId(
    bankId: number
  ): Promise<ApiResponse<DeleteResponseRoot>> {
    return await this.bankClient.DeleteEmployeeBankByBankId(bankId);
  }

  /** Get Total Employee bank Count
   * @param id
   */
  async getEmployeeBankDetailCount(
    empId: number
  ): Promise<ApiResponse<IntResponseRoot>> {
    return await this.employeeClient.GetEmployeeBankDetailCount(empId);
  }

  /** Employee-Bank Section Ends */

  /** Employee-Salary Section Starts */

  /** Get Employee-Salary Details by Id */
  async getEmployeeSalaryInfoById(
    id: number
  ): Promise<ApiResponse<ClientEmployeeCTCDetailSM[]>> {
    return await this.salaryClient.GetEmployeeSalaryInfo(id);
  }
  async getEmployeeSalaryInfoByOdata(viewModel: EmployeeSalaryInfoViewModel, employeeId: number,): Promise<ApiResponse<ClientEmployeeCTCDetailSM[]>> {
    let queryFilter = new QueryFilter();
    queryFilter.skip = (viewModel.pagination.PageNo - 1) * viewModel.pagination.PageSize;
    queryFilter.top = viewModel.pagination.PageSize;
    return await this.salaryClient.GetEmployeeSalaryInfoByoData(
      queryFilter,
      employeeId
    );
  }

  /**Add Employee Salary */
  async addEmployeeSalary(
    employeeSalary: ClientEmployeeCTCDetailSM
  ): Promise<ApiResponse<ClientEmployeeCTCDetailSM>> {
    let apiRequest = new ApiRequest<ClientEmployeeCTCDetailSM>();
    apiRequest.reqData = employeeSalary;
    return await this.salaryClient.AddEmployeeSalary(apiRequest);
  }
  /** Get Total Employee Salary Count
   * @param id
   */
  async getEmployeeSalaryCount(
    empId: number
  ): Promise<ApiResponse<IntResponseRoot>> {
    return await this.salaryClient.GetEmployeeSalaryCount(empId);
  }
  /** Employee-Salary Section Ends */
}
