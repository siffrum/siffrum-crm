import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';
import { RoleTypeSM } from '../../enums/role-type-s-m.enum';
import { PermissionSM } from '../client/permission-s-m';

export class LicenseTypeSM extends CoinManagementServiceModelBase<number> {
    title!: string;
    description!: string;
    validityInDays!: number;
    amount!: number;
    licenseTypeCode!: string;
    stripePriceId!: string;
    isPredefined!: boolean;
    validFor!: RoleTypeSM;
    permissionIds!: Array<number>;
    permissions!: Array<PermissionSM>;
}
