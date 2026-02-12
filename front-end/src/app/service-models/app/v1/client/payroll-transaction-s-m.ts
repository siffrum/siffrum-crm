import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';
import { PaymentModeSM } from '../../enums/payment-mode-s-m.enum';
import { PaymentTypeSM } from '../../enums/payment-type-s-m.enum';

export class PayrollTransactionSM extends CoinManagementServiceModelBase<number> {
    paymentAmount!: number;
    paymentFor!: string;
    paymentMode!: PaymentModeSM;
    paymentType!: PaymentTypeSM;
    toPay!: number;
    toPaid!: number;
    paymentPaid!: boolean;
    clientUserId!: number;
    clientEmployeeCTCDetailId!: number;
    clientCompanyDetailId!: number;
    errorInGeneration!: boolean;
    errorMessage!: string;
}
