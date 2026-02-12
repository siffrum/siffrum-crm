import { ClientGenericPayrollComponentSM } from './client-generic-payroll-component-s-m';

export class ClientEmployeePayrollComponentSM extends ClientGenericPayrollComponentSM {
    amountYearly!: number;
    clientEmployeeCTCDetailId?: number;
}
