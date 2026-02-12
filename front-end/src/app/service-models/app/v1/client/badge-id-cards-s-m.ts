import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';

export class BadgeIdCardsSM extends CoinManagementServiceModelBase<number> {
    employeeName!: string;
    employeeMail!: string;
    employeePhone!: string;
    employeeDesignation!: string;
    profilePicture!: string;
    companyName!: string;
    companyEmail!: string;
    companyPhone!: string;
    companyWebsite!: string;
    issuedDate!: Date;
    expiryDate!: Date;
}
