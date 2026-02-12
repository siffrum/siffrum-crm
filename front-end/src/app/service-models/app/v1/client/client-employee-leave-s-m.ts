import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';
import { LeaveTypeSM } from '../../enums/leave-type-s-m.enum';

export class ClientEmployeeLeaveSM extends CoinManagementServiceModelBase<number> {
    clientUserId!: number;
    clientCompanyDetailId!: number;
    leaveType!: LeaveTypeSM;
    employeeComment!: string;
    isApproved?: boolean;
    approvedByUserName!: string;
    approvalComment!: string;
    leaveDateFromUTC!: Date;
    leaveDateToUTC!: Date;
}
