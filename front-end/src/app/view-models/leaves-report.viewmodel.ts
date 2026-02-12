import { ClientEmployeeLeaveExtendedUserSM } from "../service-models/app/v1/client/client-employee-leave-extended-user-s-m";
import { LeaveReportRequestSM } from "../service-models/app/v1/client/leave-report-request-s-m";
import { BaseViewModel } from "./base.viewmodel";


export class LeavesReportViewmodel extends BaseViewModel {
    override PageTitle: string = 'Leave-Report';
    leavesReportList: ClientEmployeeLeaveExtendedUserSM[] = [];
    leaveReportRequest: LeaveReportRequestSM = new LeaveReportRequestSM();
    months: string[] = [];
    years: number[] = [];
    showMonthlyDropdown: boolean = true;
    showYearlyDropdown: boolean = false;
    showCustomCalendar: boolean = false;
    showTable: boolean = false;
    dateFilterTypeList: string[] = [];
    selectedMonth!: string;
    selectedYear!: number;
}


