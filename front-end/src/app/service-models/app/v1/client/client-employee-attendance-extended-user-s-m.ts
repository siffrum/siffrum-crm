import { ClientEmployeeAttendanceSM } from './client-employee-attendance-s-m';

export class ClientEmployeeAttendanceExtendedUserSM extends ClientEmployeeAttendanceSM {
    userName!: string;
    employeeCode!: string;
    presentCount!: number;
    absentCount!: number;
    leaveCount!: number;
}
