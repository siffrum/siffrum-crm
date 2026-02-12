import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';

export class ClientThemeSM extends CoinManagementServiceModelBase<number> {
    name!: string;
    css!: string;
    isSelected?: boolean;
}
