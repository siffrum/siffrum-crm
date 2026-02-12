import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ClientUserAddressSM } from 'src/app/service-models/app/v1/app-users/client-user-address-s-m';
import { ClientUserSM } from 'src/app/service-models/app/v1/app-users/client-user-s-m';
import { ClientEmployeeBankDetailSM } from 'src/app/service-models/app/v1/client/client-employee-bank-detail-s-m';
import { CommonService } from 'src/app/services/common.service';
import { EmployeeService } from 'src/app/services/employee.service';
import { LogHandlerService } from 'src/app/services/log-handler.service';
import { AddEmployeeViewModel } from 'src/app/view-models/add-employee.viewmodel';
import { BaseComponent } from '../base.component';

@Component({
    selector: 'app-add-employee',
    templateUrl: './add-employee.component.html',
    styleUrls: ['./add-employee.component.scss'],
    standalone: false
})
export class AddEmployeeComponent extends BaseComponent<AddEmployeeViewModel> implements OnInit {

  employeeInfo: ClientUserSM = new ClientUserSM();

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private employeeService: EmployeeService,
    private activatedRoute: ActivatedRoute,
  ) {
    super(commonService, logService);
    this.viewModel = new AddEmployeeViewModel();
  }


  async ngOnInit() {
    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;
    const Id = Number(this.activatedRoute.snapshot.paramMap.get('Id'));
    if (Id == undefined || Id == 0) {
      // show error
      return;
    }
  }


  async recieveEmployeeFromChild(employee: ClientUserSM) {
    if (employee.id > 0) {
      await this.getEmployee(employee.id);
      await this.nextWizardLocation();
    }
  }


  async recieveAddressFromChild(employeeAddress: ClientUserAddressSM) {
    if (employeeAddress.id > 0) {
      await this.nextWizardLocation();
    }
  }

  async recieveBankFromChild(employeeBank: ClientEmployeeBankDetailSM) {
    if (employeeBank.id > 0) {
      await this.nextWizardLocation();
    }
  }
  

/**
 * Get Employee Details
 * @param employeeId
 */
  async getEmployee(employeeId: number) {
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
        this.employeeInfo = resp.successData;
      }
    } catch (error) {
      throw error;
    }
    finally {
      await this._commonService.dismissLoader()
    }
  }


  nextWizardLocation() {
    this.viewModel.wizardLocation = this.viewModel.wizardLocation + 1;
  }


  previousWizardLocation() {
    this.viewModel.wizardLocation = this.viewModel.wizardLocation - 1;
  }



}
