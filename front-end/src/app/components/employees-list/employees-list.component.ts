import { Component, OnInit } from "@angular/core";
import { AppConstants } from "src/app-constants";
import { CommonService } from "src/app/services/common.service";
import { EmployeeService } from "src/app/services/employee.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { EmployeesListViewModel } from "src/app/view-models/employees-list.viewmodel";
import { BaseComponent } from "../base.component";
import { PermissionSM } from "src/app/service-models/app/v1/client/permission-s-m";
import { ModuleNameSM } from "src/app/service-models/app/enums/module-name-s-m.enum";
import { AccountService } from "src/app/services/account.service";

@Component({
    selector: "app-employees-list",
    templateUrl: "./employees-list.component.html",
    styleUrls: ["./employees-list.component.scss"],
    standalone: false
})
export class EmployeesListComponent
  extends BaseComponent<EmployeesListViewModel>
  implements OnInit {

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private employeeService: EmployeeService,
    private accountService: AccountService
  ) {
    super(commonService, logService);
    this.viewModel = new EmployeesListViewModel();
  }

  async ngOnInit() {
    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;
    await this.loadPageData();
    this.getPermissions()
  }
  //Check  permissions
  async getPermissions() {
    let ModuleName = ModuleNameSM.EmployeeDirectory
    let resp: PermissionSM | any = await this.accountService.getMyModulePermissions(ModuleName)
    this.viewModel.Permission = resp
  }
  override async loadPageData() {
    try {
      await this._commonService.presentLoading();
      await this.getEmployeeCountOfCompany();
      await this._commonService.getCompanyIdFromStorage();
      // this.viewModel.totalPages = this.getPagesCountArray(this.viewModel.totalCount, this.viewModel.PageSize)
      // this.viewModel.pagination.totalPages = this.getPagesCountArray(this.viewModel.pagination.totalCount, this.viewModel.pagination.PageSize)
      // Access the company ID from the common service
      let companyId = this._commonService.layoutVM.company.id;
      let resp = await this.employeeService.getAllEmployeeByCompanyIdAndOdata(
        this.viewModel, companyId);
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {

        this.viewModel.employeesList = resp.successData;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  async deleteEmployee(employeeId: number) {
    let deleteEmployeeConfirmation =
      await this._commonService.showConfirmationAlert(
        AppConstants.DefaultMessages.EmployeeDeleteAlert,
        " ",

        true,
        "warning"
      );
    if (deleteEmployeeConfirmation) {
      try {
        await this._commonService.presentLoading();
        let resp = await this.employeeService.deleteEmployee(employeeId);
        if (resp.isError) {
          await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
        } else {
          this._commonService.ShowToastAtTopEnd(
            resp.successData.deleteMessage,
            "success"
          );
          this.loadPageData();
        }
      } catch (error) {
      } finally {
        await this._commonService.dismissLoader();
      }
    }
  }
  /**
  * GET Total Count Of Companies
   */
  async getEmployeeCountOfCompany() {
    try {
      await this._commonService.presentLoading();
      let resp = await this.employeeService.getEmployeeCountOfCompany();
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        // this.viewModel.totalCount = resp.successData.intResponse;
        this.viewModel.pagination.totalCount = resp.successData.intResponse;
      }
    } catch (error) {
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  // async loadPagedataWithPagination(event: any) {
  //   this.viewModel.PageNo = event;
  //   await this.loadPageData();
  // }

  async loadPagedataWithPagination(pageNumber: number) {
    if (pageNumber && pageNumber > 0) {
      // this.viewModel.PageNo = pageNumber;
      this.viewModel.pagination.PageNo = pageNumber;
      await this.loadPageData();
    }

}
 }