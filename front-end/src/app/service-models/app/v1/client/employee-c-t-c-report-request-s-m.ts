import { BaseReportFilterSM } from './base-report-filter-s-m';

export class EmployeeCTCReportRequestSM extends BaseReportFilterSM {
    clientUserId!: number;
    employeeCtcCount!: number;
    currentlyActive!: boolean;
}
