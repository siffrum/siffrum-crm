import { LeaveTypeSM } from "../service-models/app/enums/leave-type-s-m.enum";
import { ClientEmployeeLeaveCountEndPointSM } from "../service-models/app/v1/client/client-employee-leave-count-end-point-s-m";
import { ClientEmployeeLeaveSM } from "../service-models/app/v1/client/client-employee-leave-s-m";

import { BaseViewModel } from "./base.viewmodel";

export class LeavesViewmodel extends BaseViewModel {
  override PageTitle: string = 'Leaves';
  leavesList: ClientEmployeeLeaveSM[] = [];
  selectedLeave: ClientEmployeeLeaveSM = new ClientEmployeeLeaveSM();
  leavesCount: ClientEmployeeLeaveCountEndPointSM = new ClientEmployeeLeaveCountEndPointSM();
  leaveTypeList = new Array<string>();
  leaveType!: LeaveTypeSM;
  showButton!: boolean;
  isaddmode!: boolean;

}
