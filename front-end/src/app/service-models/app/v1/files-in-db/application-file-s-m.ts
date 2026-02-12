import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';

export class ApplicationFileSM extends CoinManagementServiceModelBase<number> {
    fileName!: string;
    fileType!: string;
    fileDescription!: string;
    fileBytes!: string;
}
