import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';
import { ModuleNameSM } from '../../enums/module-name-s-m.enum';
import { RoleTypeSM } from '../../enums/role-type-s-m.enum';

export class PermissionSM extends CoinManagementServiceModelBase<number> {
    view!: boolean;
    add!: boolean;
    edit!: boolean;
    delete!: boolean;
    isEnabledForClient!: boolean;
    moduleName!: ModuleNameSM;
    roleType!: RoleTypeSM;
    companyModulesId!: number;
    clientCompanyDetailId!: number;
    clientUserId?: number;
    licenseTypeId?: number;
    isStandAlone!: boolean;
}
