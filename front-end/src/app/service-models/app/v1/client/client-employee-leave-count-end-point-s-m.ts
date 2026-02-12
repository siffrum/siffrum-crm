import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';

export class ClientEmployeeLeaveCountEndPointSM extends CoinManagementServiceModelBase<number> {
    allEmployeeLeaveCount!: number;
    approvedLeaveCount!: number;
    rejectedLeaveCount!: number;
    approvedEmployeeLeaveCount!: number;
    leaveReportCount!: number;
}
