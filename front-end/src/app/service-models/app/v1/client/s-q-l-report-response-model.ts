import { SQLReportColumns } from './s-q-l-report-columns';

export class SQLReportResponseModel {
    id!: number;
    reportName!: string;
    pageNo!: number;
    pageSize!: number;
    totalCount!: number;
    dataColumns!: Array<SQLReportColumns>;
    reportData: any;
}
