import { Component, Input, OnInit } from '@angular/core';
import { BaseComponent } from 'src/app/components/base.component';
import { ModuleNameSM } from 'src/app/service-models/app/enums/module-name-s-m.enum';
import { ClientUserSM } from 'src/app/service-models/app/v1/app-users/client-user-s-m';
import { PermissionSM } from 'src/app/service-models/app/v1/client/permission-s-m';
import { AccountService } from 'src/app/services/account.service';
import { CommonService } from 'src/app/services/common.service';
import { GenerateLetterService } from 'src/app/services/generate-letter.service';
import { LogHandlerService } from 'src/app/services/log-handler.service';
import { EmployeeGenerateLetterViewModel } from 'src/app/view-models/employee-generateLetter-info.viewmodel';

@Component({
    selector: 'app-employee-generate-letter-info',
    templateUrl: './employee-generate-letter-info.component.html',
    styleUrls: ['./employee-generate-letter-info.component.scss'],
    standalone: false
})
export class EmployeeGenerateLetterInfoComponent extends BaseComponent<EmployeeGenerateLetterViewModel> implements OnInit {

  @Input() employee: ClientUserSM = new ClientUserSM();
  @Input() isaddmode!: boolean;

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private generateLetterService: GenerateLetterService,
    private accountService:AccountService,
  ) {
    super(commonService, logService);
    this.viewModel = new EmployeeGenerateLetterViewModel();
  }


  async ngOnInit() {
    await this.loadPageData();
    await this.getMonths();
    this.viewModel.selectedMonth;
  }
/**
 * GET PaySlip By selected Month
 */
  async onMonthChange(){
    try {
          this.viewModel.dateFrom = await this._commonService.getISODateFromMonthYear(this.viewModel.selectedMonth);
          let date =  this.viewModel.dateFrom
          let empId=this.employee.id;
          let resp = await this.generateLetterService.GetPaySlipByDate(empId,date);
          if (resp.isError) {
            await this._exceptionHandler.logObject(resp);
            await this._commonService.ShowToastAtTopEnd(resp.errorData.displayMessage, 'error');
          } else {
            this._commonService.downloadDocument(resp.successData,"PaySlip",".pdf","")
          }
      }
     catch (error) {
    }
    finally {
      await this._commonService.dismissLoader();
    }
  }

/**GET list of monnths */
    async getMonths() {
      var date = new Date();
      var months = [],
        monthNames = [
          "January",
          "February",
          "March",
          "April",
          "May",
          "June",
          "July",
          "August",
          "September",
          "October",
          "November",
          "December",
        ];
      for (var i = 0; i < 12; i++) {
        months.push(monthNames[date.getMonth()] + ' ' + date.getFullYear());
        date.setMonth(date.getMonth() - 1);
      }
      this.viewModel.months = months;
    }
      //get permissions
  async getPermissions(){

    let ModuleName=ModuleNameSM.GenerateLetters
     let resp:PermissionSM | any =await this.accountService.getMyModulePermissions(ModuleName)
     this.viewModel.Permission=resp
    }
    /**
     * GET List of Letters
     */
  override async loadPageData() {
    try {
      await this._commonService.presentLoading();
      let resp = await this.generateLetterService.getLetterList();
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
        this.viewModel.letterList = resp.successData;
        this.getPermissions()
      }
    } catch (error) {
      throw error;
    }
    finally {
      await this._commonService.dismissLoader()
    }
  }
/**Download employee  letters */


  async downloadEmployeeLetter(letterId: number) {
    try {
      await this._commonService.presentLoading();
      let resp = await this.generateLetterService.downloadEmployeeLetter(this.employee.id, letterId);
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
        this._commonService.downloadDocument(resp.successData.letterData, resp.successData.name, resp.successData.extension, this.employee.loginId);
      }
    } catch (error) {
      throw error;
    }
    finally {
      await this._commonService.dismissLoader()
    }
  }

}
