import { Component, OnInit } from "@angular/core";
import { CompanyAttendanceShiftViewModel } from "src/app/view-models/company-attendance-shift.viewmodel";
import { BaseComponent } from "../base.component";
import { CommonService } from "src/app/services/common.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { CompanyAttendanceShiftService } from "src/app/services/company-attendance-shift.service";
import { AppConstants } from "src/app-constants";
import { ClientCompanyAttendanceShiftSM } from "src/app/service-models/app/v1/client/client-company-attendance-shift-s-m";
import { ModuleNameSM } from "src/app/service-models/app/enums/module-name-s-m.enum";
import { PermissionSM } from "src/app/service-models/app/v1/client/permission-s-m";
import { AccountService } from "src/app/services/account.service";
import { NgForm } from "@angular/forms";
@Component({
    selector: "app-company-attendance-shift",
    templateUrl: "./company-attendance-shift.component.html",
    styleUrls: ["./company-attendance-shift.component.scss"],
    standalone: false
})
export class CompanyAttendanceShiftComponent
  extends BaseComponent<CompanyAttendanceShiftViewModel>
  implements OnInit
{
  constructor(
    _commonService: CommonService,
    logService: LogHandlerService,
    private companyAttendanceShiftService: CompanyAttendanceShiftService,
    private accountService: AccountService
  ) {
    super(_commonService, logService);
    this.viewModel = new CompanyAttendanceShiftViewModel();
  }

  async ngOnInit() {
    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;
    await this.loadPageData();
  }
  //Check For Action permissions
  async getPermissions() {
    let ModuleName = ModuleNameSM.AttendanceShift;
    let resp: PermissionSM | any =
      await this.accountService.getMyModulePermissions(ModuleName);
    this.viewModel.Permission = resp;
  }
  /**
   * Get Company Shift Details By Company Id
   */

  override async loadPageData() {
    try {
      await this._commonService.presentLoading();
      let resp =
        await this.companyAttendanceShiftService.getAllCompanyAttendanceShiftDetails();
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.companyAttendanceShiftDetailsList = resp.successData.map(
          (shift) => this.normalizeShiftForDisplay(shift)
        );
        this.getPermissions();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Get Company Shift Details By Company Id
   */
  async getcompanyAttendanceShiftDetailsById(id: number) {
    try {
      await this._commonService.presentLoading();
      let resp = await this.companyAttendanceShiftService.getCompanyShiftById(
        id
      );
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.companyAttendanceShiftDetails =
          this.normalizeShiftForForm(resp.successData);
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**open Modal to Add/update CompanyAttendanceShift
   * @DEV : Musaib
   */

  async openCompanyAttendanceShiftModal(id: number) {
    this.viewModel.companyAttendanceShiftDetails =
      new ClientCompanyAttendanceShiftSM();
    if (id > 0) {
      this.viewModel.editMode = true;
      this.getcompanyAttendanceShiftDetailsById(id);
    } else {
      this.viewModel.editMode = false;
    }
    // this.modalService.open(content, { size: "lg", windowClass: "modal-lg" });
    this.viewModel.displayStyle = "block";
  }
  closePopup(shiftForm: NgForm) {
    this.viewModel.displayStyle = "none";
    this.viewModel.formSubmitted = false; // Clear the formSubmitted flag
    if (shiftForm) {
      shiftForm.reset(); // Reset the form
    }
  }

  /**ADD Attendance Shift
   * @DEV : Musaib
   */
  async addCompanyAddtendanceShift(shiftForm: NgForm) {
    this.viewModel.formSubmitted = true;
    try {
      if (shiftForm.invalid) {
        await this._commonService.showSweetAlertToast({
          title: "Form Fields Are Not valid !",
          icon: "error",
        });
        return; // Stop execution here if form is not valid
      }
      await this._commonService.presentLoading();
      const shiftPayload = this.getShiftPayload();
      shiftPayload.clientCompanyDetailId =
        this.viewModel.company.id;
      let resp =
        await this.companyAttendanceShiftService.addCompanyShift(
          shiftPayload
        );
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.displayStyle = "none";
        this.loadPageData();
        await this._commonService.ShowToastAtTopEnd(
          "Added Shift Successfully",
          "success"
        );

        return;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Update Company Attendance Shift
   * @param id
   * @returns success
   * @developer Musaib
   */

  async updateCompanyAttendanceShiftDetails(shiftForm: NgForm) {
    this.viewModel.formSubmitted = true;
    try {
      if (shiftForm.invalid) {
        await this._commonService.showSweetAlertToast({
          title: "Form Fields Are Not valid !",
          icon: "error",
        });
        return; // Stop execution here if form is not valid
      }
      let resp =
        await this.companyAttendanceShiftService.updateCompanyShift(
          this.getShiftPayload()
        );
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.displayStyle = "none";
        this.loadPageData();
        await this._commonService.ShowToastAtTopEnd(
          "Updated   Successfully",
          "success"
        );
        return;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  formatShiftTime(time: string | Date): string {
    const minutes = this.extractMinutes(time);
    if (minutes == null) return "";

    const hours24 = Math.floor(minutes / 60);
    const mins = minutes % 60;
    const meridiem = hours24 >= 12 ? "PM" : "AM";
    const hours12 = hours24 % 12 || 12;
    return `${hours12}:${mins.toString().padStart(2, "0")} ${meridiem}`;
  }
  /**
   * Delete Selected Shift
   * @DEV : Musaib
   * @param id
   * @returns Success Message
   */
  async deleteCompanyShift(id: number) {
    let deleteConfirmation = await this._commonService.showConfirmationAlert(
      AppConstants.DefaultMessages.DeleteCompanyShift,
      " ",
      true,
      "warning"
    );
    if (deleteConfirmation) {
      try {
        let resp = await this.companyAttendanceShiftService.deleteCompanyShift(
          id
        );
        if (resp.isError) {
          await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
        } else {
          await this._commonService.ShowToastAtTopEnd(
            "Deleted Shift  Successfully",
            "success"
          );
          this.loadPageData();
          return;
        }
      } catch (error) {
        throw error;
      } finally {
        await this._commonService.dismissLoader();
      }
    }
  }

  private getShiftPayload(): ClientCompanyAttendanceShiftSM {
    return {
      ...this.viewModel.companyAttendanceShiftDetails,
      shiftFrom: this.normalizeTimeForApi(
        this.viewModel.companyAttendanceShiftDetails.shiftFrom
      ) as any,
      shiftTo: this.normalizeTimeForApi(
        this.viewModel.companyAttendanceShiftDetails.shiftTo
      ) as any,
    };
  }

  private normalizeShiftForDisplay(
    shift: ClientCompanyAttendanceShiftSM
  ): ClientCompanyAttendanceShiftSM {
    return {
      ...shift,
      shiftFrom: this.normalizeTimeForApi(shift.shiftFrom) as any,
      shiftTo: this.normalizeTimeForApi(shift.shiftTo) as any,
    };
  }

  private normalizeShiftForForm(
    shift: ClientCompanyAttendanceShiftSM
  ): ClientCompanyAttendanceShiftSM {
    return {
      ...shift,
      shiftFrom: this.normalizeTimeForInput(shift.shiftFrom) as any,
      shiftTo: this.normalizeTimeForInput(shift.shiftTo) as any,
    };
  }

  private normalizeTimeForInput(value: string | Date): string {
    const minutes = this.extractMinutes(value);
    if (minutes == null) return "";

    const hours = Math.floor(minutes / 60)
      .toString()
      .padStart(2, "0");
    const mins = (minutes % 60).toString().padStart(2, "0");
    return `${hours}:${mins}`;
  }

  private normalizeTimeForApi(value: string | Date): string {
    const time = this.normalizeTimeForInput(value);
    return time ? `1970-01-01T${time}:00` : "";
  }

  private extractMinutes(value: string | Date | null | undefined): number | null {
    if (!value) return null;

    if (value instanceof Date && !Number.isNaN(value.getTime())) {
      return value.getHours() * 60 + value.getMinutes();
    }

    const rawValue = String(value).trim();
    if (!rawValue) return null;

    const hhmmMatch = rawValue.match(/(\d{1,2}):(\d{2})/);
    if (!hhmmMatch) return null;

    let hours = Number(hhmmMatch[1]);
    const minutes = Number(hhmmMatch[2]);
    const upperValue = rawValue.toUpperCase();

    if (upperValue.includes("PM") && hours < 12) {
      hours += 12;
    }
    if (upperValue.includes("AM") && hours === 12) {
      hours = 0;
    }

    if (hours > 23 || minutes > 59) return null;
    return hours * 60 + minutes;
  }
}
