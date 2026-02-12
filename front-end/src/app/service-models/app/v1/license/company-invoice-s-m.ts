import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';

export class CompanyInvoiceSM extends CoinManagementServiceModelBase<number> {
    stripeInvoiceId!: string;
    startDateUTC!: Date;
    expiryDateUTC!: Date;
    discountInPercentage!: number;
    actualPaidPrice!: number;
    currency!: string;
    companyLicenseDetailsId!: number;
    stripeCustomerId!: string;
    amountDue!: number;
    amountRemaining!: number;
}
