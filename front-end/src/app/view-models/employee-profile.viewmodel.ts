
import { ClientUserSM } from "../service-models/app/v1/app-users/client-user-s-m";
import { LoginUserSM } from "../service-models/app/v1/app-users/login/login-user-s-m";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class EmployeeProfileViewModel extends BaseViewModel {
    override PageTitle: string = 'Employee-Info';
    employee: ClientUserSM = new ClientUserSM();
    Loggedemployee: LoginUserSM = new LoginUserSM();
    showButton: boolean = true;
    isReadonly: boolean = true;
    isAddMode: boolean = true;
    tabLocation: EmployeeProfileTabs = EmployeeProfileTabs.employeeInfo;
    showEmployeeInfoTab: boolean = false;
    showEmployeeAddressTab: boolean = false;
    showEmployeeBankDetailsTab: boolean = false;
    showEmployeeLeavesTab: boolean = false;
    showEmployeeDocumentsTab: boolean = false;
    showEmployeeSalaryTab: boolean = false;
    showEmployeeLettersTab: boolean = false;
}


export enum EmployeeProfileTabs {
    employeeInfo = 0,
    employeeAddress = 1,
    employeeBankDetails = 2,
    employeeDocuments = 3,
    employeeSalary = 4,
    employeeLeaves = 5,
    employeeGenerateLetter = 6,
}