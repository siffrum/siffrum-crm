import { ClientUserSM } from "../service-models/app/v1/app-users/client-user-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class AddEmployeeViewModel extends BaseViewModel {
    override PageTitle: string = 'Add-Employee';
    employee: ClientUserSM = new ClientUserSM();
    isReadonly: boolean = false;
    isAddMode: boolean = true;
    wizardLocation: AddEmployeeWizards = AddEmployeeWizards.addEmployeeInfo;
}


export enum AddEmployeeWizards {
    addEmployeeInfo = 0,
    addEmployeeAddress = 1,
    addEmployeeBankDetails = 2,
    addEmployeeDocuments = 3,
    addEmployeeSalary = 4,
}