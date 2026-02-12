import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private activatedRoute: ActivatedRoute,
    private employeeService: EmployeeService,
    private accountService: AccountService,
    private authGuard: AuthGuard,
  ) {
    super(commonService, logService);
    this.viewModel = new EmployeeProfileViewModel();
  }
 // *@Dev Musaib
  async ngOnInit() {
    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;
    const Id = Number(this.activatedRoute.snapshot.paramMap.get('Id'));
    if (Id == undefined) {
      await this._commonService.ShowToastAtTopEnd("Something Went Wrong", "error");
    } else if (Id > 0) {
      await this.loadPageDataWithId(Id);
      await this.setPermissions();
    } else {
      await this.loadPageData();
      await this.setPermissions();
    }
    this._commonService.layoutVM.company.id;
  }
  /**
   * Set Module Permissions Handle Show/Hide Tabs
   */
  async setPermissions() {
    // Check permissions for each tab and set show properties
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
      return resp.view; // Return true or false based on the 'view' permission
    } catch (error) {
      // Handle error if needed
      return false; // Return false in case of an error
    }
  }
//check permissions
async getPermissions(){
  let ModuleName=ModuleNameSM.Employee
   let resp:PermissionSM | any =await this.accountService.getMyModulePermissions(ModuleName)
   this.viewModel.Permission=resp
  }

    // Update Tab Location
    updateTabLocation(tabLocation: EmployeeProfileTabs) {
      switch (tabLocation) {
        case EmployeeProfileTabs.employeeGenerateLetter:
          break;
        case EmployeeProfileTabs.employeeLeaves:
          break;
        case EmployeeProfileTabs.employeeSalary:
          break;
        case EmployeeProfileTabs.employeeDocuments:

          break;
        case EmployeeProfileTabs.employeeBankDetails:
          break;

        case EmployeeProfileTabs.employeeAddress:
          break;
        case EmployeeProfileTabs.employeeInfo:
        default:
          break;
      }
      this.viewModel.tabLocation = tabLocation;
    }

  override async loadPageData() {
    try {
      await this._commonService.presentLoading();
      let resp = await this.employeeService.getEmployeeByMineEndpoint();
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      }
      else {
        this.viewModel.employee = resp.successData;
        
      }
    } catch (error) {
      throw error;
    }
    finally {
      await this._commonService.dismissLoader()
    }
  }

/**
 * Load Employee
 * @param employeeId
 */
  async loadPageDataWithId(employeeId: number) {
    try {
      await this._commonService.presentLoading();
      let resp = await this.employeeService.getEmployeeByEmployeeId(employeeId);
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      }
      else {
        this.viewModel.employee = resp.successData;
      }
    } catch (error) {
      throw error;
    }
    finally {
      await this._commonService.dismissLoader()
    }
  }
  /**
   * GEt Logged In User
   * @returns
   */
  async getloggedInUser() {
    try {
      await this._commonService.presentLoading();
      let tokenValid = await this.authGuard.IsTokenValid();
      if (!tokenValid) {
        return false;
      } else {
        let user = await this.accountService.getUserFromStorage();
        if (user == "") {
          return false;
        }
        this.viewModel.Loggedemployee = user;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
    return;
  }
  editDetails() {
    this.viewModel.isReadonly = !this.viewModel.isReadonly;
    this.viewModel.showButton = !this.viewModel.showButton;
  }
}
