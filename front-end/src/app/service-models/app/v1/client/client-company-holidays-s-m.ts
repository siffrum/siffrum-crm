import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';

export class ClientCompanyHolidaysSM extends CoinManagementServiceModelBase<number> {
    name!: string;
    description!: string;
    dateTime!: Date;
    clientCompanyDetailId!: number;
}
