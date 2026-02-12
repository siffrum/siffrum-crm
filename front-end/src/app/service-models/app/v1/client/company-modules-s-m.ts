import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';
import { ModuleNameSM } from '../../enums/module-name-s-m.enum';

export class CompanyModulesSM extends CoinManagementServiceModelBase<number> {
    moduleValue!: string;
    description!: string;
    moduleName!: ModuleNameSM;
    isEnabled?: boolean;
    standAlone?: boolean;
}
