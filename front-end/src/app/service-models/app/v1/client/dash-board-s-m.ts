import { ClientCompanyDepartmentReportSM } from './client-company-department-report-s-m';

export class DashBoardSM {
    numberOfEmployees!: number;
    numberOfAdmins!: number;
    numberOfLeavesApproved!: number;
    numberOfLeavesPending!: number;
    numberOfLeavesRejected!: number;
    numberOfDepartments!: number;
    numberOfEmployeesPresent!: number;
    numberOfEmployeesAbsent!: number;
    numberOfEmployeeOnLeave!: number;
    clientCompanyDepartment!: Array<ClientCompanyDepartmentReportSM>;
}
