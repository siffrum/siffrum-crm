import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';

export class ClientEmployeeBankDetailSM extends CoinManagementServiceModelBase<number> {
    bankName!: string;
    branch!: string;
    accountNo!: number;
    ifscCode!: string;
    clientUserId!: number;
    clientCompanyDetailId!: number;
}
