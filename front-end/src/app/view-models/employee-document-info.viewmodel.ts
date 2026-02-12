import { ClientCompanyDetailSM } from "../service-models/app/v1/client/client-company-detail-s-m";
import { ClientEmployeeDocumentSM } from "../service-models/app/v1/client/client-employee-document-s-m";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
import { BaseViewModel } from "./base.viewmodel";


export class EmployeeDocumentInfoViewModel extends BaseViewModel {
    override PageTitle: string = 'Employee-Document-Info';
    showTooltip: boolean = false;
    employeeDocumentsList: ClientEmployeeDocumentSM[] = [];
    employeeDocument: ClientEmployeeDocumentSM = new ClientEmployeeDocumentSM();
    company: ClientCompanyDetailSM = new ClientCompanyDetailSM();
    editMode: boolean = false;
    documentType = new Array<string>();
    displayStyle = "none";
    formSubmitted: boolean = false;
    validations = {
        name: [
            { type: 'required', message: 'Document Name is Required' },
            { type: 'minlength', value: 2, message: 'Minimum Length is 3 Characters' },
            { type: 'maxlength', value: 15, message: 'Maximum Length is 15 Characters' },
            { type: "pattern", message: "Not Valid Format !" }
        ],
        employeeDocumentType: [
            { type: 'required', message: 'Select Document Type' },
        ],
        documentDescription: [
            { type: 'minlength', value: 5, message: 'Minimum Length is 5 Characters' },
            { type: 'maxlength', value: 50, message: 'Maximum Length is 50 Characters' },
            { type: "pattern", message: "Not Valid Format !" }
        ],
        employeeDocumentPath: [
            { type: 'required', message: 'Document path Requierd' },
        ]
    }
}