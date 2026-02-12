import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';

export class SQLReportMasterSM extends CoinManagementServiceModelBase<number> {
    reportName!: string;
    sqlQuery!: string;
    clientCompanyDetailId?: number;
}
