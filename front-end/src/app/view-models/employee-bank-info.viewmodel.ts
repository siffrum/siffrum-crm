import { ClientCompanyDetailSM } from "../service-models/app/v1/client/client-company-detail-s-m";
import { ClientEmployeeBankDetailSM } from "../service-models/app/v1/client/client-employee-bank-detail-s-m";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class EmployeeBankInfoViewModel extends BaseViewModel {
    override  PageTitle: string = 'Employee-Bank-Info';
    showTooltip: boolean = false;
    employeeBank: ClientEmployeeBankDetailSM = new ClientEmployeeBankDetailSM();
    employeeBankList: ClientEmployeeBankDetailSM[] = [];
    company: ClientCompanyDetailSM = new ClientCompanyDetailSM();
    showButton: boolean = true;
    isReadonly = true;
    editMode: boolean = false;
    displayStyle = "none";
    formSubmitted: boolean = false;
    validations = {
        bankName: [
            { type: 'required', message: 'Bank Name is Required' },
            { type: 'minlength', value: 3, message: 'Minimum Length is 3 Characters' },
            { type: 'maxlength', value: 30, message: 'Maximum Length is 30 Characters' },
            { type: "pattern", message: "Not Valid Format !" }
        ],
        branchName: [
            { type: 'required', message: 'Branch Name is Required' },
            { type: 'minlength', value: 3, message: 'Minimum Length is 3 Characters' },
            { type: 'maxlength', value: 20, message: 'Maximum Length is 20 Characters' },
            { type: "pattern", message: "Not Valid Format !" }
        ],

        accountNo: [
            { type: 'required', message: 'Account No. is Required' },
            { type: 'minlength', value: 5, message: 'Minimum Length is 5 Characters' },
            { type: 'maxlength', value: 20, message: 'Maximum Length is 20 Characters' },
            { type: "pattern", message: "Not Valid Format !" }
        ],
        ifscCode: [
            { type: 'required', message: 'IFSC Code is Required' },
            { type: 'minlength', value: 5, message: 'Minimum Length is 3 Characters' },
            { type: 'maxlength', value: 12, message: 'Maximum Length is 20 Characters' },
            { type: "pattern", message: "Not Valid Format !" }
        ]
    }
}