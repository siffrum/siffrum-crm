import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';

export class ContactUsSM extends CoinManagementServiceModelBase<number> {
    name!: string;
    subject!: string;
    message!: string;
    emailId!: string;
    phone!: string;
}
