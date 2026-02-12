import { PayrollTransactionSM } from './payroll-transaction-s-m';
import { EmployeeStatusSM } from '../../enums/employee-status-s-m.enum';

export class GeneratePayrollTransactionSM extends PayrollTransactionSM {
    employeeName!: string;
    designation!: string;
    employeeStatus!: EmployeeStatusSM;
}
