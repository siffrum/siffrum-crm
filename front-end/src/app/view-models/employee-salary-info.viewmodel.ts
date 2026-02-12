import { CurrencyCode } from "../internal-models/currency-codes";
import { ClientCompanyDetailSM } from "../service-models/app/v1/client/client-company-detail-s-m";
import { ClientEmployeeCTCDetailSM } from "../service-models/app/v1/client/client-employee-c-t-c-detail-s-m";
import { ClientEmployeePayrollComponentSM } from "../service-models/app/v1/client/client-employee-payroll-component-s-m";

import { BaseViewModel } from "./base.viewmodel";

export class EmployeeSalaryInfoViewModel extends BaseViewModel {
  employeeSalaryList: ClientEmployeeCTCDetailSM[] = [];
  employeeSalary: ClientEmployeeCTCDetailSM = new ClientEmployeeCTCDetailSM();
  selectedEmployeeSalary: ExtendedClientEmployeeSalarySM = new ExtendedClientEmployeeSalarySM();
  company: ClientCompanyDetailSM = new ClientCompanyDetailSM();
  showTooltip: boolean = false;
  isDisabled: boolean = false;
  selectedEmployeePayrollComponent!: ExtendedClientEmployeePayrollComponentSM;
  listPayrollCalculationType: string[] = [];
  listCompomentPeriodType: string[] = [];
  listCurrencyCodes = CurrencyCode;
  override  PageTitle: string = "Employee-Salary-Info";
  showButton: boolean = true;
  isReadonly = true;
  ctcViewMode: boolean = true;
  enableComponentsEdit: boolean = false;
  isAddMode: boolean = false;
  displayStyle = "none";
  hideAddButton: boolean = false;
  formSubmitted: boolean = false;
  showPercentagValidationMessage:boolean=false;
  percentagValidationMessage:string="";
  startDateUTC:string="";
  showPastDates:boolean=false;
  validations = {
    ctcAmount: [
      { type: 'required', message: 'CTC Amount is Required !' },
      { type: 'pattern', message: 'Invalid Format !' }
    ],
    currencyCode: [
      { type: 'required', message: 'Currency Code is Required !' }
    ],
    startDateUTC: [
      { type: 'required', message: 'Start Date is Required !' }
    ],
    // endDateUTC: [
    //   { type: 'required', message: 'End Date is Required !' }
    // ],
  }
}

// export class EmployeePayrollComponentsVM extends ClientEmployeePayrollComponentSM {
//   // selectedGenericPayrollcomponent: ClientGenericPayrollComponentSM =
//   // new ClientGenericPayrollComponentSM();
//   amountMonthlyUI!: number;
//   amountYearlyUI!: number;
//   percentageUI!: number;
// }

export class ExtendedClientEmployeeSalarySM extends ClientEmployeeCTCDetailSM {
  amountMonthly: number = 0;
  override clientEmployeePayrollComponents: ExtendedClientEmployeePayrollComponentSM[] = [];
  otherComponents: number = 0;
  percentageUI: number = 0;
  ctcMonthly: number = 0;
}

export class ExtendedClientEmployeePayrollComponentSM extends ClientEmployeePayrollComponentSM {
  amountMonthlyUI: number = 0;
  amountYearlyUI: number = 0;
  percentageUI: number = 0;
  amountMonthly: number = 0;
}

export enum UpdateComponentsType {
  CTC = 0,
  ComponentAmountMonthly = 1,
  ComponentAmountYearly = 2,
  ComponentPercentage = 3,
  ComponentCalculationType = 4,
  ComponentCalculationPeriod = 5,
}
