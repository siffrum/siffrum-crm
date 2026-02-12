import { CalendarEvent, CalendarView } from "angular-calendar";
import { addDays, startOfMonth } from "date-fns";
import { ClientEmployeeAttendanceSM } from "../service-models/app/v1/client/client-employee-attendance-s-m";
import { BaseViewModel } from "./base.viewmodel";
import { ClientCompanyAttendanceShiftSM } from "../service-models/app/v1/client/client-company-attendance-shift-s-m";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
export class EmployeeAttendanceViewModel extends BaseViewModel {
  override PageTitle: string = "Employee Attendance";
  currentDate: Date = new Date();
  previousMonth = addDays(startOfMonth(this.currentDate), -1);
  attendance: ClientEmployeeAttendanceSM = new ClientEmployeeAttendanceSM();
  attendanceList: ClientEmployeeAttendanceSM[] = [];
  companyAttendanceShiftDetailsList: ClientCompanyAttendanceShiftSM[] = [];
  companyAttendanceShiftDetail: ClientCompanyAttendanceShiftSM = new ClientCompanyAttendanceShiftSM()
  attendanceStatus = Array<string>();
  checkedIn: boolean = false;
  isCheckInButton: boolean = true;
  isCheckOutButton: boolean = false;
  isNextButtonDisabled: boolean = false;
  viewDate: Date = new Date();
  view: CalendarView = CalendarView.Month;
  CalendarView = CalendarView;
  events: CalendarEvent[] = [];
  viewDays!: { name: string, shortName: string }[];
  currentShift: any;

}


