import { ClientUserSM } from "../service-models/app/v1/app-users/client-user-s-m";
import { ClientCompanyDetailSM } from "../service-models/app/v1/client/client-company-detail-s-m";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class AddCompanyAdminViewModel extends BaseViewModel {
  override  PageTitle: string = "Add-Admin";
  addAdmin: ClientUserSM = new ClientUserSM();
  initialAddModePermissionCompanyDetailId!: number;
  addAdminForm: boolean = false;
  permissionTableModal: boolean = false;
  company: ClientCompanyDetailSM = new ClientCompanyDetailSM();
  disabledDate!: string;
  maxDate = new Date();
  companyUserList: ClientUserSM[] = [];
  showTooltip: boolean = false;
  companyUser: ClientUserSM = new ClientUserSM();
  modulePermissionList: PermissionSM[] = [];
  clientUserId: number = 0
  selectAll: boolean = false;
  isChecked: boolean = false;
  displayStyle = "none";
  editMode: boolean = false;
  permissionsModal: boolean = false;
  adminDetailsModal: boolean = false;
  formSubmitted: boolean = false;
  validations = {
    firstName: [
      { type: "required", message: "First Name is Required !" },
      { type: "minlength", value: 3, message: "Minimum Length is 3 Characters !" },
      { type: "minlength", value: 15, message: "Maximum Length is 15 Characters !" },
      { type: "pattern", message: "Not Valid Format !" }

    ],
    middleName: [
      { type: "minlength", value: 3, message: "Minimum Length is 3 Characters !" },
      { type: "maxlength", value: 15, message: "Maximum Length is 15 Characters !" },
      { type: "pattern", message: "Not Valid Format !" }

    ],
    lastName: [
      { type: "required", message: "Last Name is Required !" },
      { type: "minlength", value: 3, message: "Minimum Length is 3 Characters !" },
      { type: "maxlength", value: 15, message: "Maximum Length is 15 Characters !" },
      { type: "pattern", message: "Not Valid Format !" }

    ],
    // Other fields...
    personalEmail: [
      { type: "required", message: "Personal Email ID is Required !" },
      { type: "minlength", value: 6, message: "Minimum Length is 6 Characters !" },
      { type: "maxlength", value: 50, message: "Maximum Length is 30 Characters !" },
      { type: "pattern", message: "Format Not Valid!" }

    ],
    phoneNumber: [
      { type: "required", message: "Mobile Number is Required !" },
      { type: "minlength", value: 10, message: "Minimum Length is 10 Characters !" },
      { type: "maxlength", value: 10, message: "Maximum Length is 10 Characters !" },
      { type: "pattern", message: "Format Not Valid!" }
    ],
    email: [
      { type: "required", message: "Email is Required !" },
      { type: "minlength", value: 6, message: "Minimum Length is 6 Characters !" },
      { type: "maxlength", value: 50, message: "Maximum Length is 30 Characters !" },
      { type: "pattern", message: "Format Not Valid !" }
    ],
    designation: [
      { type: "required", message: "Designation is Required !" },
      { type: "minlength", value: 3, message: "Minimum Length is 3 Characters !" },
      { type: "maxlength", value: 20, message: "Maximum Length is 20 Characters !" },
      { type: "pattern", message: "Format Not Valid !" }
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