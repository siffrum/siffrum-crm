import { SQLReportCellSM } from './s-q-l-report-cell-s-m';
import { SQLReportRowsSM } from './s-q-l-report-rows-s-m';

export class SQLReportDataModelSM {
    clientCompanyDetailId?: number;
    pageNo!: number;
    pageSize!: number;
    reportDataColumns!: Array<SQLReportCellSM>;
    reportDataRows!: Array<SQLReportRowsSM>;
}
