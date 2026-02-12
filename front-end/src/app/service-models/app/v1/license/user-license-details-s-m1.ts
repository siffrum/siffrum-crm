import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';

export class UserLicenseDetailsSM1 extends CoinManagementServiceModelBase<number> {
    stripeCustomerId!: string;
    stripeSubscriptionId!: string;
    stripeProductId!: string;
    stripePriceId!: string;
    productName!: string;
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
    clientUserId!: number;
    licenseTypeId!: number;
}
