import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';

export class UserInvoiceSM1 extends CoinManagementServiceModelBase<number> {
    stripeInvoiceId!: string;
    startDateUTC!: Date;
    expiryDateUTC!: Date;
    discountInPercentage!: number;
    actualPaidPrice!: number;
    currency!: string;
    userLicenseDetailsId!: number;
    stripeCustomerId!: string;
    amountDue!: number;
    amountRemaining!: number;
}
