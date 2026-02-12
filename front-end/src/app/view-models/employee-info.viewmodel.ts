import { Validators } from "@angular/forms";
import { ClientUserSM } from "../service-models/app/v1/app-users/client-user-s-m";
import { ClientCompanyAttendanceShiftSM } from "../service-models/app/v1/client/client-company-attendance-shift-s-m";
import { ClientCompanyDepartmentSM } from "../service-models/app/v1/client/client-company-department-s-m";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class EmployeeInfoViewModel extends BaseViewModel {
  testDate!: Date;
  override  PageTitle: string = "Employee-Info";
  showTooltip: boolean = false;
  employee: ClientUserSM = new ClientUserSM();
  showButton: boolean = true;
  wizardLocation: AddEmployeeWizards = AddEmployeeWizards.addEmployeeInfo;
  isDisabled: boolean = false;
  attendanceShiftList: ClientCompanyAttendanceShiftSM[] = [];
  selectedShiftId: number = 0;
  selectedShift: string = "";
  departmentList: ClientCompanyDepartmentSM[] = [];
  selectedDepartmentId: number = 0;
  selectedDepartment: string = "";
  empId: number = 0;
  formSubmitted: boolean = false;
  dateOfJoining!: string;
  validations = {
    employeeCode: [
      { type: "required", message: "Employee Code is Required !" },
      { type: "minlength", value: 3, message: "Minimum Length is 3 Characters !" },
      { type: "maxlength", value: 8, message: "Maximum Length is 8 Characters !" },
      { type: "pattern", message: "Not Valid Format !" }
    ],
    firstName: [
      { type: "required", message: "First Name is Required !" },
      { type: "minlength", value: 3, message: "Minimum Length is 3 Characters !" },
      { type: "maxlength", value: 15, message: "Maximum Length is 15 Characters !" },
      { type: "pattern", message: "Not Valid Format !" },
    ],
    middleName: [
      { type: "minlength", value: 3, message: "Minimum Length is 3 Characters !" },
      { type: "maxlength", value: 15, message: "Maximum Length is 15 Characters !" },
      { type: "pattern", message: "Not Valid Format !" },
    ],
    lastName: [
      { type: "required", message: "Last Name is Required !" },
      { type: "minlength", value: 3, message: "Minimum Length is 3 Characters !" },
      { type: "maxlength", value: 15, message: "Maximum Length is 15 Characters !" },
      { type: "pattern", message: "Not Valid Format !" },
    ],
    // Other fields...

    personalEmail: [
      { type: "required", message: "Personal Email ID is Required !" },
      { type: "minlength", value: 6, message: "Minimum Length is 6 Characters !" },
      { type: "maxlength", value: 50, message: "Maximum Length is 50 Characters !" },
      { type: "pattern", message: "Format Not Valid!" },
    ],
    phoneNumber: [
      { type: "required", message: "Mobile Number is Required !" },
      { type: "minlength", value: 10, message: "Minimum Length is 10 Characters !" },
      { type: "maxlength", value: 10, message: "Maximum Length is 10 Characters !" },
      { type: "pattern", message: "Format Not Valid!" },
    ],
    email: [
      { type: "required", message: "email is Required !" },
      { type: "minlength", value: 6, message: "Minimum Length is 6 Characters !" },
      { type: "maxlength", value: 50, message: "Maximum Length is 50 Characters !" },
      { type: "pattern", message: "Format Not Valid !" },
    ],
    designation: [
      { type: "required", message: "Designation is Required !" },
      { type: "minlength", value: 2, message: "Minimum Length is 2 Characters !" },
      { type: "maxlength", value: 20, message: "Maximum Length is 20 Characters !" },
      { type: "pattern", message: "Format Not Valid !" },
    ],

    dateOfBirth: [
      { type: "required", message: "D-O-B is Required !" },
      { type: "max", message: "D-O-B is Required !" },
      { type: "min", message: "D-O-B is Required !" },
    ],
    dateOfJoining: [
      { type: "required", message: "Date Of Joining  is Required !" },
      { type: "max", message: "Date Of Joining is Required !" },
      { type: "min", message: "Date Of Joining is Required !" },
    ],
  };
}
export enum AddEmployeeWizards {
  addEmployeeInfo = 0,
  addEmployeeAddress = 1,
  addEmployeeDocuments = 2,
  addEmployeeBankDetails = 3,
  addEmployeeSalary = 4,
}
