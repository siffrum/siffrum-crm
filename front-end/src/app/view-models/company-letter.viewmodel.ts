import { DocumentsSM } from "../service-models/app/v1/client/documents-s-m";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class CompanyLetterViewModel extends BaseViewModel {
    override PageTitle: string = 'Company-Letter';
    showTooltip: boolean = false;
    companyLetterList: DocumentsSM[] = [];
    companyLetter: DocumentsSM = new DocumentsSM();
    displayStyle = "none";

}