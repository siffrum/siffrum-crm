import { BaseReportFilterSM } from './base-report-filter-s-m';
import { LeaveTypeSM } from '../../enums/leave-type-s-m.enum';

export class LeaveReportRequestSM extends BaseReportFilterSM {
    clientEmployeeUserId!: number;
    leaveCount!: number;
    leaveType!: LeaveTypeSM;
}
