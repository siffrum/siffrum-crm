import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';
import { CompanyInvoiceSM } from './company-invoice-s-m';

export class CompanyLicenseDetailsSM extends CoinManagementServiceModelBase<number> {
    stripeCustomerId!: string;
    stripeSubscriptionId!: string;
    stripeProductId!: string;
    stripePriceId!: string;
    subscriptionPlanName!: string;
    validityInDays!: number;
    discountInPercentage!: number;
    actualPaidPrice!: number;
    currency!: string;
    status!: string;
    isSuspended!: boolean;
    isCancelled!: boolean;
    cancelAt?: Date;
    cancelledOn?: Date;
    startDateUTC!: Date;
    expiryDateUTC!: Date;
    clientCompanyId!: number;
    licenseTypeId!: number;
    companyInvoices!: Array<CompanyInvoiceSM>;
}
