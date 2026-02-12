import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';

export class ForgotPasswordSM extends CoinManagementServiceModelBase<number> {
    companyCode!: string;
    userName!: string;
    expiry!: Date;
}
