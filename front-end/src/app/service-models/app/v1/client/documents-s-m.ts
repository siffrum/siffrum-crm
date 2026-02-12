import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';

export class DocumentsSM extends CoinManagementServiceModelBase<number> {
    override id!: number;
    name!: string;
    description!: string;
    letterData!: string;
    extension!: string;
    clientCompanyDetailId!: number;
}
