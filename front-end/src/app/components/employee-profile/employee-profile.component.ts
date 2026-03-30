import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthGuard } from 'src/app/guard/auth.guard';
import { AccountService } from 'src/app/services/account.service';
import { CommonService } from 'src/app/services/common.service';
import { EmployeeService } from 'src/app/services/employee.service';
import { LogHandlerService } from 'src/app/services/log-handler.service';
import { EmployeeProfileTabs, EmployeeProfileViewModel } from 'src/app/view-models/employee-profile.viewmodel';
import { BaseComponent } from '../base.component';
import { ModuleNameSM } from 'src/app/service-models/app/enums/module-name-s-m.enum';
import { PermissionSM } from 'src/app/service-models/app/v1/client/permission-s-m';

@Component({
  selector: 'app-employee-profile',
  templateUrl: './employee-profile.component.html',
  styleUrls: ['./employee-profile.component.scss'],
  standalone: false
})
export class EmployeeProfileComponent extends BaseComponent<EmployeeProfileViewModel> implements OnInit {

  // ✅ Fix: use override for base class property
  override viewModel = new EmployeeProfileViewModel();

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private activatedRoute: ActivatedRoute,
    private employeeService: EmployeeService,
    private accountService: AccountService,
    private authGuard: AuthGuard
  ) {
    super(commonService, logService);
  }

  async ngOnInit() {
    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;

    const Id = Number(this.activatedRoute.snapshot.paramMap.get('Id'));
    if (!Id || Id <= 0) {
      await this.loadPageData(); // Load mine endpoint if Id missing
    } else {
      await this.loadPageDataWithId(Id);
    }
    await this.setPermissions();
  }

  /** Set Module Permissions for Tabs */
  async setPermissions() {
    this.viewModel.showEmployeeInfoTab = await this.checkTabPermission(EmployeeProfileTabs.employeeInfo, ModuleNameSM.Employee);
    this.viewModel.showEmployeeAddressTab = await this.checkTabPermission(EmployeeProfileTabs.employeeAddress, ModuleNameSM.EmployeeAddress);
    this.viewModel.showEmployeeBankDetailsTab = await this.checkTabPermission(EmployeeProfileTabs.employeeBankDetails, ModuleNameSM.BankDetail);
    this.viewModel.showEmployeeDocumentsTab = await this.checkTabPermission(EmployeeProfileTabs.employeeDocuments, ModuleNameSM.EmployeeDocument);
    this.viewModel.showEmployeeLeavesTab = await this.checkTabPermission(EmployeeProfileTabs.employeeLeaves, ModuleNameSM.Leave);
    this.viewModel.showEmployeeSalaryTab = await this.checkTabPermission(EmployeeProfileTabs.employeeSalary, ModuleNameSM.EmployeeCTC);
    this.viewModel.showEmployeeLettersTab = await this.checkTabPermission(EmployeeProfileTabs.employeeGenerateLetter, ModuleNameSM.GenerateLetters);
  }

  async checkTabPermission(tabLocation: EmployeeProfileTabs, moduleName: ModuleNameSM): Promise<boolean> {
    try {
      let resp: PermissionSM | any = await this.accountService.getMyModulePermissions(moduleName);
      return resp.view;
    } catch (error) {
      return false;
    }
  }

  /** Toggle Edit Mode */
  editDetails() {
    if (this.viewModel.isReadonly) {
      this.viewModel.isReadonly = false;
      this.viewModel.showButton = true;
    } else {
      this.saveEmployee(); // Save changes when clicking Done
    }
  }

  /** Save Employee Changes */
  async saveEmployee() {
    try {
      await this._commonService.presentLoading();
      const resp = await this.employeeService.updateEmployeeInfo(this.viewModel.employee);

      if (resp.isError) {
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: 'top-end',
          icon: 'error'
        });
      } else {
        this.viewModel.employee = resp.successData;
        this.viewModel.isReadonly = true;
        this._commonService.showSweetAlertToast({
          title: 'Success!',
          text: 'Employee details updated successfully',
          position: 'top-end',
          icon: 'success'
        });
      }
    } catch (error) {
      console.error(error);
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  /** Update Tab Location */
  updateTabLocation(tabLocation: EmployeeProfileTabs) {
    this.viewModel.tabLocation = tabLocation;
  }

  /** Load Employee by Mine Endpoint */
  override async loadPageData() {
    try {
      await this._commonService.presentLoading();
      const resp = await this.employeeService.getEmployeeByMineEndpoint();
      if (resp.isError) {
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: 'top-end',
          icon: 'error'
        });
      } else {
        this.viewModel.employee = resp.successData;
      }
    } catch (error) {
      console.error(error);
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  /** Load Employee by Id */
  async loadPageDataWithId(employeeId: number) {
    try {
      await this._commonService.presentLoading();
      const resp = await this.employeeService.getEmployeeByEmployeeId(employeeId);
      if (resp.isError) {
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: 'top-end',
          icon: 'error'
        });
      } else {
        this.viewModel.employee = resp.successData;
      }
    } catch (error) {
      console.error(error);
    } finally {
      await this._commonService.dismissLoader();
    }
  }

}