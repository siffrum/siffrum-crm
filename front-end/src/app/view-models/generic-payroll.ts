import { ComponentPeriodTypeSM } from "../service-models/app/enums/component-period-type-s-m.enum";
import { ClientGenericPayrollComponentSM } from "../service-models/app/v1/client/client-generic-payroll-component-s-m";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class GenericPayrollViewModel extends BaseViewModel {
  override PageTitle: string = 'Payroll-Structure';
  showTooltip: boolean = false;
  genericPayrolls: ClientGenericPayrollComponentSM[] = [];
  selectedGenericPayroll: ClientGenericPayrollComponentSM = new ClientGenericPayrollComponentSM();
  componentPeriodTypeList = new Array<string>();
  componentPeriodType!: ComponentPeriodTypeSM;
  readonlyText: string = "readonly";
  showButton: boolean = true;
  displayStyle = "none";
  addMode: boolean = false;
  editMode: boolean = false;
  formSubmitted: boolean = false;
  validations = {
    name: [
      { type: "required", message: "Name is Required !" },
      { type: 'minlength', value: 2, message: 'Minimum Length is 2 Characters !' },
      { type: 'maxlength', value: 20, message: 'Maximum Length is 20 Characters !' },
      { type: "pattern", message: "Not Valid Format !" },
    ],
    percentage: [
      { type: "required", message: "Percentage is Required !" },
      { type: 'minlength', value: 1, message: 'Minimum Length is 1 Characters !' },
      { type: 'maxlength', value: 3, message: 'Maximum Length is 3 Characters !' },
      { type: "pattern", message: "InValid !" },
    ],
    description: [
      { type: 'minlength', value: 3, message: 'Minimum Length is 3 Characters !' },
      { type: 'maxlength', value: 50, message: 'Maximum Length is 50 Characters !' },
      { type: "pattern", message: "Not Valid Format !" },
    ],
    componentPeriodType: [
      { type: "required", message: "Period Type is Required !" }
    ]
  };
}
