import { Component, OnInit, ViewEncapsulation } from "@angular/core";
import { AttendanceStatusSM } from "src/app/service-models/app/enums/attendance-status-s-m.enum";
import { CommonService } from "src/app/services/common.service";
import { EmployeeAttendanceService } from "src/app/services/employee-attendance.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { EmployeeAttendanceViewModel } from "src/app/view-models/employee-attendance.viewmodel";
import { BaseComponent } from "../base.component";
import {
  addMonths,
  endOfMonth,
  startOfMonth,
  subMonths,
  format,
  isSameDay
} from "date-fns";
import { CompanyAttendanceShiftService } from "src/app/services/company-attendance-shift.service";
import { ModuleNameSM } from "src/app/service-models/app/enums/module-name-s-m.enum";
import { PermissionSM } from "src/app/service-models/app/v1/client/permission-s-m";
import { AccountService } from "src/app/services/account.service";

@Component({
    selector: "app-employee-attendance",
    templateUrl: "./employee-attendance.component.html",
    encapsulation: ViewEncapsulation.None,
    styleUrls: ["./employee-attendance.component.scss"],
    standalone: false
})
export class EmployeeAttendanceComponent
  extends BaseComponent<EmployeeAttendanceViewModel>
  implements OnInit
{
  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private employeeAttendanceService: EmployeeAttendanceService,
    private companyAttendanceShiftService: CompanyAttendanceShiftService,
    private accountService:AccountService
  ) {
    super(commonService, logService);
    this.viewModel = new EmployeeAttendanceViewModel();
  }

  async ngOnInit() {
    await this.loadPageData();
  }

    //Check For Actions permissions
    async getPermissions(){
      let ModuleName=ModuleNameSM.Attendance
       let resp:PermissionSM | any =await this.accountService.getMyModulePermissions(ModuleName)
       this.viewModel.Permission=resp
      }
  /**
   * load calendar data with employee status (Present/Absent/None)  from api
   * @returns status
   */
  override async loadPageData() {
    try {
      await this._commonService.presentLoading();
      this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;
      this.viewModel.attendanceStatus =
        this._commonService.EnumToStringArray(AttendanceStatusSM);
      let resp = await this.employeeAttendanceService.getEmployeeAttendance(
        this.viewModel.currentDate
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
        this.getCompanyAttendanceShift()
        this.viewModel.attendanceList = resp.successData;
        this.generateEvents();
        await this.getPermissions()
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Get Attendance Shift 
   */
  async getCompanyAttendanceShift() {
    try {
      await this._commonService.presentLoading();
      let resp = await this.companyAttendanceShiftService.getAllCompanyAttendanceShiftDetails();
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
        let currentTime = new Date();
        let currentShift = null;
        let currentShiftTimeDifference = Infinity;
        let isWithinShiftHours = false; // Flag to check if current time is within shift hours
  
        for (let i = 0; i < this.viewModel.companyAttendanceShiftDetailsList.length; i++) {
          let shift = this.viewModel.companyAttendanceShiftDetailsList[i];
          let shiftFrom = new Date(shift.shiftFrom);
          let shiftTo = new Date(shift.shiftTo);
          let currentHours = currentTime.getHours();
          let currentMinutes = currentTime.getMinutes();
          let shiftFromHours = shiftFrom.getHours();
          let shiftFromMinutes = shiftFrom.getMinutes();
          let shiftToHours = shiftTo.getHours();
          let shiftToMinutes = shiftTo.getMinutes();
          
          // Check if current time is within shift hours
          if (
            (currentHours > shiftFromHours || (currentHours === shiftFromHours && currentMinutes >= shiftFromMinutes)) &&
            (currentHours < shiftToHours || (currentHours === shiftToHours && currentMinutes <= shiftToMinutes))
          ) {
            // Calculate the time difference between current time and shift start time
            const timeDifference = Math.abs(currentHours - shiftFromHours) * 60 + Math.abs(currentMinutes - shiftFromMinutes);
            if (timeDifference < currentShiftTimeDifference) {
              currentShift = shift;
              currentShiftTimeDifference = timeDifference;
              isWithinShiftHours = true;
            }
          }
        }
        
        this.viewModel.currentShift = currentShift; // Store the current shift in a variable
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Generate events (attendanceStatus)  viz: Present/Absent/None
   * @returns status
   */
  async generateEvents() {
    let eventsMap = new Map();
    this.viewModel.attendanceList.forEach((element) => {
      let title = element.attendanceStatus.toString();
      let apiDate = format(new Date(element.attendanceDate), 'MM/dd/yyyy'); // Use format function from date-fns
      let today = format(new Date(), 'MM/dd/yyyy'); // Use format function from date-fns
  
      if (isSameDay(new Date(element.attendanceDate), new Date())) { // Use isSameDay function from date-fns
        this.viewModel.attendance = element;
      }
  
      let event = {
        start: new Date(element.attendanceDate),
        title: title,
      };
      if (eventsMap.has(apiDate)) {
        let existingEvent = eventsMap.get(apiDate);
        existingEvent.title = title;
      } else {
        eventsMap.set(apiDate, event);
      }
      if (isSameDay(new Date(), new Date(element.attendanceDate)) && element.checkIn) { // Use isSameDay function from date-fns
        this.viewModel.isCheckInButton = false;
        this.viewModel.isCheckOutButton = true;
      }
    });
    this.viewModel.events = [...eventsMap.values()];
    this.viewModel.viewDate = new Date();
  }

  /**
   * Add check In time of the employee
   * @param date
   * @returns
   */
  async checkIn(date: Date) {
    try {
      await this._commonService.presentLoading();

      if (this.viewModel.attendance.checkIn) {
        await this._commonService.ShowToastAtTopEnd(
          "Already checked in!",
          "error"
        );
        this.viewModel.isCheckInButton=false;
      }
      //disable CheckIn on weekend
      else if (
        this.viewModel.currentDate.getDay() === 6 ||
        this.viewModel.currentDate.getDay() === 0
      ) {
        await this._commonService.ShowToastAtTopEnd(
          "!Oops Its a Weekend ",
          "error"
        );
      } 
      //Disable CheckIn after 11 am 
      else if (this.viewModel.currentDate.getHours() >= 18) {
        await this._commonService.ShowToastAtTopEnd("Time Out", "error");
      }
    
       else {
        //set time Format  example"00:00:00 AM "
        this.viewModel.attendance.checkIn = format(date, "hh:mm:ss a");
        let resp = await this.employeeAttendanceService.addEmployeeCheckInTime(
          this.viewModel.attendance
        );
        if (resp.isError) {
          await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
        }
         else {
          this.viewModel.attendance = resp.successData;
          this.viewModel.isCheckInButton = false;
          this.viewModel.isCheckOutButton=true;
          await this._commonService.ShowToastAtTopEnd(
            "Added CheckIn Time  Successfully",
            "success"
          );
          return;
        }
      }
    } 
    catch (error)
     {
      throw error;
    }
     finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Update Check Out Time of the employee
   * @returns response.successData
   */
  async checkOut(date: Date) {
    try {
      await this._commonService.presentLoading();
      if (this.viewModel.attendance.checkOut) {
        await this._commonService.ShowToastAtTopEnd(
          "Already checked out!",
          "error"
        );
        this.viewModel.isCheckOutButton= false
      }
      //Disable Check Out on Weekends
     else if (
        this.viewModel.currentDate.getDay() === 6 ||
        this.viewModel.currentDate.getDay() === 0
      ) {
        await this._commonService.ShowToastAtTopEnd(
          "!Oops its a Weekend ",
          "error"
        );
      }
      //Disable Check Out after 6pm
       else if (this.viewModel.currentDate.getHours() >= 18) {
        await this._commonService.ShowToastAtTopEnd("Time Out", "error");
      }
       else {
        //set time Format  example"00:00:00 AM "
        this.viewModel.attendance.checkOut = format(date, "hh:mm:ss a");
        this.viewModel.attendance.attendanceStatus = AttendanceStatusSM.P;
        let resp = await this.employeeAttendanceService.updateEmployeeCheckOutTime(
            this.viewModel.attendance);
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
          this.viewModel.isCheckOutButton= false
          this.loadPageData();
          await this._commonService.ShowToastAtTopEnd(
            "Added CheckOut Time  Successfully",
            "success"
          );
          return;
        }
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Go to previous Month of the Calendar
   * Subtract one month from Current month
   */
  async prevMonth() {
    try {
      await this._commonService.presentLoading();
      let currentMonthStart = startOfMonth(this.viewModel.viewDate);
      let prevMonthStartDay = startOfMonth(subMonths(currentMonthStart, 1));
      let prevMonthEndDay = endOfMonth(prevMonthStartDay);
      this.viewModel.attendanceStatus =
        this._commonService.EnumToStringArray(AttendanceStatusSM);
      let resp = await this.employeeAttendanceService.getEmployeeAttendance(
        prevMonthEndDay
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
        this.viewModel.attendanceList = resp.successData;
        this.generateEvents();
        this.viewModel.viewDate = prevMonthEndDay;
      }
      return;
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Get next Month of the Calendar
   * add one month for the current date
   */
  async nextMonth() {
    try {
      await this._commonService.presentLoading();
      let currentMonthStart = startOfMonth(this.viewModel.viewDate);
      let nextMonthStartDay = startOfMonth(addMonths(currentMonthStart, 1));
      let nextMonthEndDay = endOfMonth(nextMonthStartDay);
      if (nextMonthStartDay >= this.viewModel.currentDate) {
        this.viewModel.isNextButtonDisabled = true;
      } else {
        this.viewModel.isNextButtonDisabled = false;
        this.viewModel.attendanceStatus =
          this._commonService.EnumToStringArray(AttendanceStatusSM);
        let resp = await this.employeeAttendanceService.getEmployeeAttendance(
          nextMonthEndDay
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
          this.viewModel.attendanceList = resp.successData;
          this.generateEvents();
          this.viewModel.viewDate = nextMonthEndDay;
        }
      }
      return;
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
}
