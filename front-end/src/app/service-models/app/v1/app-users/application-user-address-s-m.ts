import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';

export class ApplicationUserAddressSM extends CoinManagementServiceModelBase<number> {
    country!: string;
    state!: string;
    city!: string;
    address1!: string;
    address2!: string;
    pinCode!: string;
    applicationUserId!: number;
}
