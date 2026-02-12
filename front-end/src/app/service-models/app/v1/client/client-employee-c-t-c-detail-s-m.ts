import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';
import { ClientEmployeePayrollComponentSM } from './client-employee-payroll-component-s-m';

export class ClientEmployeeCTCDetailSM extends CoinManagementServiceModelBase<number> {
    ctcAmount!: number;
    currencyCode!: string;
    startDateUTC!: Date;
    endDateUTC?: Date;
    currentlyActive!: boolean;
    clientUserId!: number;
    clientEmployeePayrollComponents!: Array<ClientEmployeePayrollComponentSM>;
}
