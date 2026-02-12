import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';
import { ComponentCalculationTypeSM } from '../../enums/component-calculation-type-s-m.enum';
import { ComponentPeriodTypeSM } from '../../enums/component-period-type-s-m.enum';

export class ClientGenericPayrollComponentSM extends CoinManagementServiceModelBase<number> {
    name!: string;
    description!: string;
    percentage!: number;
    componentCalculationType!: ComponentCalculationTypeSM;
    componentPeriodType!: ComponentPeriodTypeSM;
    clientCompanyDetailId!: number;
}
