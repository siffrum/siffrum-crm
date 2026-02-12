import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';

export class ClientEmployeeUserCountsResponseSM extends CoinManagementServiceModelBase<number> {
    allEmployeeCount!: number;
    adminEmployeeCount!: number;
    simpleEmployeeCount!: number;
    activeEmployeeCount!: number;
    resignedEmployeeCount!: number;
    suspendedEmployeeUserCount!: number;
}
