import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';

export class ClientCompanyDetailSM extends CoinManagementServiceModelBase<number> {
    companyCode!: string;
    name!: string;
    description!: string;
    companyContactEmail!: string;
    companyMobileNumber!: string;
    companyWebsite!: string;
    companyLogoPath!: string;
    isEnabled!: boolean;
    isTrialUsed!: boolean;
    trailLastDate?: Date;
    companyDateOfEstablishment!: Date;
}
