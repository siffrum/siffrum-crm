import { RoleTypeSM } from "../service-models/app/enums/role-type-s-m.enum";
import { LoginUserSM } from "../service-models/app/v1/app-users/login/login-user-s-m";
import { DashBoardSM } from "../service-models/app/v1/client/dash-board-s-m";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class DashboardViewModel extends BaseViewModel {
    override PageTitle: string = 'Dashboard';
    tokenRole!: RoleTypeSM;
    employee: LoginUserSM = new LoginUserSM();
    adminDashboardVM: DashBoardSM = new DashBoardSM()
}