import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';
import { EmployeeDocumentTypeSM } from '../../enums/employee-document-type-s-m.enum';

export class ClientEmployeeDocumentSM extends CoinManagementServiceModelBase<number> {
    clientUserId!: number;
    clientCompanyDetailId!: number;
    name!: string;
    employeeDocumentPath!: string;
    employeeDocumentType!: EmployeeDocumentTypeSM;
    documentDescription!: string;
    extension!: string;
}
