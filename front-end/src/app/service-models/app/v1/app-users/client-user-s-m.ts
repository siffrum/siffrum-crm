import { LoginUserSM } from './login/login-user-s-m';
import { GenderSM } from '../../enums/gender-s-m.enum';
import { EmployeeStatusSM } from '../../enums/employee-status-s-m.enum';

export class ClientUserSM extends LoginUserSM {
    gender!: GenderSM;
    personalEmailId!: string;
    dateOfJoining!: Date;
    lastWorkingDay!: Date;
    dateOfResignation!: Date;
    employeeCode!: string;
    clientCompanyDetailId!: number;
    userSettingId?: number;
    clientCompanyDepartmentId?: number;
    clientCompanyAttendanceShiftId?: number;
    designation!: string;
    department!: string;
    attendanceShift!: string;
    employeeStatus!: EmployeeStatusSM;
    isPaymentAdmin!: boolean;
}
