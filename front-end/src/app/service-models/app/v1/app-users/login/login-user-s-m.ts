import { CoinManagementServiceModelBase } from '../../../base/coin-management-service-model-base';
import { RoleTypeSM } from '../../../enums/role-type-s-m.enum';
import { LoginStatusSM } from '../../../enums/login-status-s-m.enum';

export class LoginUserSM extends CoinManagementServiceModelBase<number> {
    roleType!: RoleTypeSM;
    loginId!: string;
    firstName!: string;
    middleName!: string;
    lastName!: string;
    emailId!: string;
    passwordHash!: string;
    phoneNumber!: string;
    profilePicturePath!: string;
    isPhoneNumberConfirmed!: boolean;
    isEmailConfirmed!: boolean;
    loginStatus!: LoginStatusSM;
    dateOfBirth!: Date;
}
