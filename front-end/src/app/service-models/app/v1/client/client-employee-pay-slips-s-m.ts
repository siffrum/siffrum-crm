import { ClientCompanyDetailSM } from './client-company-detail-s-m';
import { ClientEmployeePayrollComponentSM } from './client-employee-payroll-component-s-m';

export class ClientEmployeePaySlipsSM extends ClientCompanyDetailSM {
    employeeName!: string;
    dateOfJoining!: Date;
    employeeEmail!: string;
    employeePhone!: string;
    designation!: string;
    paymentFor!: string;
    paymentAmount!: number;
    paymentPaid!: boolean;
    employeeCode!: string;
    ctcAmount!: number;
    companyAddress!: string;
    clientEmployeePayrollComponents!: Array<ClientEmployeePayrollComponentSM>;
}
