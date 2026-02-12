import { ClientUserSM } from "../service-models/app/v1/app-users/client-user-s-m";
import { DocumentsSM } from "../service-models/app/v1/client/documents-s-m";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class EmployeeGenerateLetterViewModel extends BaseViewModel {
    override  PageTitle: string = 'Employee-GenerateLetter-Info';
    letterList: DocumentsSM[] = [];
    employeeDocument: DocumentsSM = new DocumentsSM();
    employee: ClientUserSM = new ClientUserSM();
    showButton: boolean = true;
    isReadonly = true;
    selectedMonth: string = ''
    months: string[] = [];
    paySlip: string = '';
    dateFrom!: Date;
    compId: number = 0
}
