import { ClientUserSM } from "../service-models/app/v1/app-users/client-user-s-m";
import { ClientCompanyDetailSM } from "../service-models/app/v1/client/client-company-detail-s-m";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class EmployeesListViewModel extends BaseViewModel {
    override  PageTitle: string = 'Employee-List';
    showTooltip: boolean = false;
    employeesList: ClientUserSM[] = [];
    company: ClientCompanyDetailSM = new ClientCompanyDetailSM()
}