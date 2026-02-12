import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';
import { ExpenseModeSM } from '../../enums/expense-mode-s-m.enum';

export class CompanyAccountsTransactionSM extends CoinManagementServiceModelBase<number> {
    expenseName!: string;
    expensePurpose!: string;
    expenseAmount!: number;
    expenseDate!: Date;
    currencyCode!: string;
    expenseMode!: ExpenseModeSM;
    expensePaid!: boolean;
    clientCompanyDetailId!: number;
}
