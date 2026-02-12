import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';

export class ResetPasswordRequestSM extends CoinManagementServiceModelBase<number> {
    newPassword!: string;
    authCode!: string;
}
