import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';
import { ReimbursementTypeSM } from '../../enums/reimbursement-type-s-m.enum';

export class ClientEmployeeAdditionalReimbursementLogSM extends CoinManagementServiceModelBase<number> {
    clientUserId?: number;
    reimbursementType!: ReimbursementTypeSM;
    reimburseDocumentName!: string;
    extension!: string;
    reimbursementDescription!: string;
    reimbursementAmount!: number;
    reimbursementDate!: Date;
    reimbursementDocumentPath!: string;
    clientCompanyDetailId!: number;
}
