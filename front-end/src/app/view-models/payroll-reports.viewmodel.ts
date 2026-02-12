import { ClientUserSM } from "../service-models/app/v1/app-users/client-user-s-m";
import { GeneratePayrollTransactionSM } from "../service-models/app/v1/client/generate-payroll-transaction-s-m";
import { PayrollTransactionReportSM } from "../service-models/app/v1/client/payroll-transaction-report-s-m";
import { BaseViewModel } from "./base.viewmodel";


export class PayrollTransactionReportViewmodel extends BaseViewModel {
    override PageTitle: string = 'Payroll-Report';
    PayrollTransactionReportList: GeneratePayrollTransactionSM[] = [];
    PayrollTransactionReportRequest: PayrollTransactionReportSM = new PayrollTransactionReportSM();
    months: string[] = [];
    years: number[] = [];
    showMonthlyDropdown: boolean = true;
    showYearlyDropdown: boolean = false;
    showCustomCalendar: boolean = false;
    showTable: boolean = false;
    dateFilterTypeList: string[] = [];
    selectedMonth!: string;
    selectedYear!: number;
    ClientCompanyEmployeeList: ClientUserSM[] = [];
    SelectedEmployeeOfTheCompany: ClientUserSM = new ClientUserSM();
    loginIds: string[] = [];
}


