import { Component, OnInit, ViewEncapsulation } from "@angular/core";
import { AttendanceStatusSM } from "src/app/service-models/app/enums/attendance-status-s-m.enum";
import { CommonService } from "src/app/services/common.service";
import { EmployeeAttendanceService } from "src/app/services/employee-attendance.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { EmployeeAttendanceViewModel } from "src/app/view-models/employee-attendance.viewmodel";
import { BaseComponent } from "../base.component";
import { addMonths, endOfMonth, startOfMonth, subMonths, format, isSameDay } from "date-fns";
import { CompanyAttendanceShiftService } from "src/app/services/company-attendance-shift.service";
import { ModuleNameSM } from "src/app/service-models/app/enums/module-name-s-m.enum";
import { PermissionSM } from "src/app/service-models/app/v1/client/permission-s-m";
import { AccountService } from "src/app/services/account.service";

@Component({
  selector: "app-employee-attendance",
  templateUrl: "./employee-attendance.component.html",
  styleUrls: ["./employee-attendance.component.scss"],
  encapsulation: ViewEncapsulation.None,
  standalone: false
})
export class EmployeeAttendanceComponent extends BaseComponent<EmployeeAttendanceViewModel> implements OnInit {

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private employeeAttendanceService: EmployeeAttendanceService,
    private companyAttendanceShiftService: CompanyAttendanceShiftService,
    private accountService: AccountService
  ) {
    super(commonService, logService);
    this.viewModel = new EmployeeAttendanceViewModel();
  }

  async ngOnInit() {
    await this.loadPageData();
  }

  // Check permissions for attendance actions
  async getPermissions() {
    const ModuleName = ModuleNameSM.Attendance;
    const resp: PermissionSM | any = await this.accountService.getMyModulePermissions(ModuleName);
    this.viewModel.Permission = resp;
  }

  // Load attendance calendar data
  override async loadPageData() {
    try {
      await this._commonService.presentLoading();
      this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;

      this.viewModel.attendanceStatus = this._commonService.EnumToStringArray(AttendanceStatusSM);

      const resp = await this.employeeAttendanceService.getEmployeeAttendance(this.viewModel.currentDate);
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.getCompanyAttendanceShift();
        this.viewModel.attendanceList = resp.successData;
        this.generateEvents();
        await this.getPermissions();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  // Load company shift details
  async getCompanyAttendanceShift() {
    try {
      await this._commonService.presentLoading();
      const resp = await this.companyAttendanceShiftService.getAllCompanyAttendanceShiftDetails();

      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.companyAttendanceShiftDetailsList = resp.successData;
        this.viewModel.currentShift = this.findCurrentShift(resp.successData);
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  // Generate calendar events
  async generateEvents() {
    const eventsMap = new Map();
    this.viewModel.attendanceList.forEach(element => {
      const title = element.attendanceStatus.toString();
      const apiDate = format(new Date(element.attendanceDate), 'MM/dd/yyyy');

      if (isSameDay(new Date(element.attendanceDate), new Date())) {
        this.viewModel.attendance = element;
        if (element.checkIn) {
          this.viewModel.isCheckInButton = false;
          this.viewModel.isCheckOutButton = true;
        }
      }

      const event = { start: new Date(element.attendanceDate), title };
      eventsMap.set(apiDate, event);
    });

    this.viewModel.events = [...eventsMap.values()];
    this.viewModel.viewDate = new Date();
  }

  // Employee Check-In
  async checkIn(date: Date) {
    try {
      await this._commonService.presentLoading();

      if (this.viewModel.attendance.checkIn) {
        await this._commonService.ShowToastAtTopEnd("Already checked in!", "error");
        this.viewModel.isCheckInButton = false;
      } else if ([0, 6].includes(this.viewModel.currentDate.getDay())) {
        await this._commonService.ShowToastAtTopEnd("!Oops Its a Weekend", "error");
      } else if (!this.viewModel.currentShift) {
        await this._commonService.ShowToastAtTopEnd("No active shift right now", "error");
      } else if (!this.isCurrentTimeWithinShiftWindow(this.viewModel.currentShift, 120)) {
        await this._commonService.ShowToastAtTopEnd("Time Out", "error");
      } else {
        this.viewModel.attendance.checkIn = format(date, "hh:mm:ss a");
        const resp = await this.employeeAttendanceService.addEmployeeCheckInTime(this.viewModel.attendance);

        if (resp.isError) {
          await this._exceptionHandler.logObject(resp.errorData);
          this._commonService.showSweetAlertToast({
            title: 'Error!',
            text: resp.errorData.displayMessage,
            position: "top-end",
            icon: "error"
          });
        } else {
          this.viewModel.attendance = resp.successData;
          this.viewModel.isCheckInButton = false;
          this.viewModel.isCheckOutButton = true;
          await this._commonService.ShowToastAtTopEnd("Check-In added successfully", "success");
        }
      }
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  // Employee Check-Out
  async checkOut(date: Date) {
    try {
      await this._commonService.presentLoading();

      if (this.viewModel.attendance.checkOut) {
        await this._commonService.ShowToastAtTopEnd("Already checked out!", "error");
        this.viewModel.isCheckOutButton = false;
      } else if ([0, 6].includes(this.viewModel.currentDate.getDay())) {
        await this._commonService.ShowToastAtTopEnd("!Oops Its a Weekend", "error");
      } else if (!this.viewModel.currentShift) {
        await this._commonService.ShowToastAtTopEnd("No active shift right now", "error");
      } else if (!this.isCurrentTimeWithinShiftWindow(this.viewModel.currentShift, 240)) {
        await this._commonService.ShowToastAtTopEnd("Time Out", "error");
      } else {
        this.viewModel.attendance.checkOut = format(date, "hh:mm:ss a");
        this.viewModel.attendance.attendanceStatus = AttendanceStatusSM.P;

        const resp = await this.employeeAttendanceService.updateEmployeeCheckOutTime(this.viewModel.attendance);
        if (resp.isError) {
          await this._exceptionHandler.logObject(resp.errorData);
          this._commonService.showSweetAlertToast({
            title: 'Error!',
            text: resp.errorData.displayMessage,
            position: "top-end",
            icon: "error"
          });
        } else {
          this.viewModel.attendance = resp.successData;
          this.viewModel.isCheckOutButton = false;
          this.generateEvents(); // refresh calendar events
          await this._commonService.ShowToastAtTopEnd("Check-Out added successfully", "success");
        }
      }
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  // Navigate to previous month
  async prevMonth() {
    try {
      await this._commonService.presentLoading();
      const prevMonthStart = startOfMonth(subMonths(this.viewModel.viewDate, 1));
      const prevMonthEnd = endOfMonth(prevMonthStart);

      const resp = await this.employeeAttendanceService.getEmployeeAttendance(prevMonthEnd);
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
      } else {
        this.viewModel.attendanceList = resp.successData;
        this.generateEvents();
        this.viewModel.viewDate = prevMonthEnd;
      }
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  // Navigate to next month
  async nextMonth() {
    try {
      await this._commonService.presentLoading();
      const nextMonthStart = startOfMonth(addMonths(this.viewModel.viewDate, 1));
      const nextMonthEnd = endOfMonth(nextMonthStart);

      if (nextMonthStart > this.viewModel.currentDate) {
        this.viewModel.isNextButtonDisabled = true;
        return;
      }

      const resp = await this.employeeAttendanceService.getEmployeeAttendance(nextMonthEnd);
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
      } else {
        this.viewModel.attendanceList = resp.successData;
        this.generateEvents();
        this.viewModel.viewDate = nextMonthEnd;
      }
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

  private findCurrentShift(shifts: any[]): any {
    const now = new Date();
    const currentMinutes = now.getHours() * 60 + now.getMinutes();
    let matchedShift: any = null;
    let closestTimeDiff = Number.POSITIVE_INFINITY;

    shifts.forEach((shift) => {
      const shiftFrom = this.extractMinutes(shift.shiftFrom);
      const shiftTo = this.extractMinutes(shift.shiftTo);

      if (shiftFrom == null || shiftTo == null) {
        return;
      }

      const isOvernight = shiftTo < shiftFrom;
      const isWithinShift = isOvernight
        ? currentMinutes >= shiftFrom || currentMinutes <= shiftTo
        : currentMinutes >= shiftFrom && currentMinutes <= shiftTo;

      if (!isWithinShift) {
        return;
      }

      const timeDiff = currentMinutes >= shiftFrom
        ? currentMinutes - shiftFrom
        : 24 * 60 - shiftFrom + currentMinutes;

      if (timeDiff < closestTimeDiff) {
        closestTimeDiff = timeDiff;
        matchedShift = shift;
      }
    });

    return matchedShift;
  }

  private isCurrentTimeWithinShiftWindow(shift: any, graceMinutes: number): boolean {
    const shiftFrom = this.extractMinutes(shift?.shiftFrom);
    const shiftTo = this.extractMinutes(shift?.shiftTo);

    if (shiftFrom == null || shiftTo == null) {
      return false;
    }

    const now = new Date();
    const currentMinutes = now.getHours() * 60 + now.getMinutes();
    const overnight = shiftTo < shiftFrom;

    if (!overnight) {
      return currentMinutes >= shiftFrom && currentMinutes <= shiftTo + graceMinutes;
    }

    const overnightEnd = shiftTo + graceMinutes;
    return currentMinutes >= shiftFrom || currentMinutes <= overnightEnd;
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
