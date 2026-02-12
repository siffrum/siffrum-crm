import { ClientCompanyDepartmentSM } from "../service-models/app/v1/client/client-company-department-s-m";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class CompanyDepartmentViewModel extends BaseViewModel {
  override PageTitle: string = "Departments";
  departmentList: ClientCompanyDepartmentSM[] = [];
  department: ClientCompanyDepartmentSM = new ClientCompanyDepartmentSM();
  displayStyle = "none";
  showTooltip: boolean = false;
  showButton: boolean = true;
  editMode: boolean = false;
  formSubmitted: boolean = false;
  validations = {
    departmentCode: [
      { type: "required", message: "Code is Required !" },
      { type: 'minlength', value: 3, message: 'Minimum Length is 3 Characters !' },
      { type: 'maxlength', value: 8, message: 'Maximum Length is 8 Characters !' }
    ],
    departmentName: [
      { type: "required", message: "Department Name is Required !" },
      { type: 'minlength', value: 2, message: 'Minimum Length is 2 Characters !' },
      { type: 'maxlength', value: 50, message: 'Maximum Length is 50 Characters !' },
      { type: "pattern", message: "Not Valid Format !" },
    ],
    departmentManager: [
      { type: "required", message: "Manager Name is Required !" },
      { type: 'minlength', value: 3, message: 'Minimum Length is 3 Characters !' },
      { type: 'maxlength', value: 30, message: 'Maximum Length is 30 Characters !' },
      { type: "pattern", message: "Not Valid Format !" },
    ],
    departmenntLocation: [
      { type: "required", message: "Department location is Required !" },
      { type: 'minlength', value: 5, message: 'Minimum Length is 5 Characters !' },
      { type: 'maxlength', value: 50, message: 'Maximum Length is 50 Characters !' },
      { type: "pattern", message: "Not Valid Format !" },
    ],
    departmentDescription: [
      { type: 'minlength', value: 5, message: 'Minimum Length is 5 Characters !' },
      { type: 'maxlength', value: 50, message: 'Maximum Length is 50 Characters !' },
      { type: "pattern", message: "Not Valid Format !" },
    ]
  };
}