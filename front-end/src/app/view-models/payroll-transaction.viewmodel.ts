import { GeneratePayrollTransactionSM } from "../service-models/app/v1/client/generate-payroll-transaction-s-m";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class TransactionViewModel extends BaseViewModel {
    override PageTitle: string = 'Transactions';
    payrollTransactions: GeneratePayrollTransactionSM = new GeneratePayrollTransactionSM();
    payrollTransactionList: GeneratePayrollTransactionSM[] = [];
    months: string[] = [];
    selectedMonth!: string;
    showTable: boolean = false;
    showGenerateAllBtn: boolean = false;
    showPayAllBtn: boolean = false;
    override Permission?: PermissionSM | undefined;
}