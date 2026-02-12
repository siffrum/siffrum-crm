import { ClientEmployeeAttendanceExtendedUserSM } from './client-employee-attendance-extended-user-s-m';

export class ClientExcelFileSummarySM {
    attendanceSummary!: Array<ClientEmployeeAttendanceExtendedUserSM>;
    fromDate!: string;
    toDate!: string;
    totalRecordsCount!: number;
    numberOfRecordsNotAdded!: number;
    employeeRecordsAddedCount!: number;
}
