import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';
import { DateFilterTypeSM } from '../../enums/date-filter-type-s-m.enum';

export class BaseReportFilterSM extends CoinManagementServiceModelBase<number> {
    dateFrom!: Date;
    dateTo!: Date;
    searchString!: string;
    dateFilterType!: DateFilterTypeSM;
}
