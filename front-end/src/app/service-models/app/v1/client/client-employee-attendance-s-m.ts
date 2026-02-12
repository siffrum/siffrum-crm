import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';
import { AttendanceStatusSM } from '../../enums/attendance-status-s-m.enum';

export class ClientEmployeeAttendanceSM extends CoinManagementServiceModelBase<number> {
    attendanceDate!: Date;
    checkIn!: string;
    checkOut!: string;
    attendanceStatus!: AttendanceStatusSM;
    location!: string;
    clientUserId!: number;
    clientCompanyDetailId!: number;
    errorMessageInUpload!: string;
}
