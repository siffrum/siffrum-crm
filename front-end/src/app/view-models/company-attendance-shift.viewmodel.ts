import { ClientCompanyAttendanceShiftSM } from "../service-models/app/v1/client/client-company-attendance-shift-s-m";
import { ClientCompanyDetailSM } from "../service-models/app/v1/client/client-company-detail-s-m";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class CompanyAttendanceShiftViewModel extends BaseViewModel {
  override  PageTitle: string = 'Attendance Shift ';
  companyAttendanceShiftDetailsList: ClientCompanyAttendanceShiftSM[] = [];
  companyAttendanceShiftDetails: ClientCompanyAttendanceShiftSM = new ClientCompanyAttendanceShiftSM();
  company: ClientCompanyDetailSM = new ClientCompanyDetailSM();
  displayStyle = "none";
  showTooltip: boolean = false;
  showButton: boolean = true;
  editMode: boolean = false;
  formSubmitted: boolean = false;
  validations = {
    shiftName: [
      { type: "required", message: "Shift Name is Required !" },
      { type: 'minlength', value: 3, message: 'Minimum Length is 3 Characters !' },
      { type: 'maxlength', value: 20, message: 'Maximum Length is 20 Characters !' },
      { type: "pattern", message: "Not Valid Format !" },
    ],
    shiftFrom: [
      { type: "required", message: "Shift From is Required !" },
    ],
    shiftTo: [
      { type: "required", message: "shiftTo is Required !" },
    ],
    shiftDescription: [
      { type: 'minlength', value: 3, message: 'Minimum Length is 3 Characters !' },
      { type: 'maxlength', value: 50, message: 'Maximum Length is 50 Characters !' },
      { type: "pattern", message: "Not Valid Format !" },
    ]
  };
}