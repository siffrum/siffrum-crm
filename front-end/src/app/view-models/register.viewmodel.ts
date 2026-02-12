import { ClientUserSM } from "../service-models/app/v1/app-users/client-user-s-m";
import { ClientCompanyDetailSM } from "../service-models/app/v1/client/client-company-detail-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class RegisterViewModel extends BaseViewModel {
  override PageTitle: string = "Register New Comapany";
  AddCompanyDetail: ClientCompanyDetailSM = new ClientCompanyDetailSM();
  addAdmin: ClientUserSM = new ClientUserSM();
  formSubmitted: boolean = false;
  disabledDate!: string;
  maxDate = new Date();
  companyDetailsValidations = {
    companyCode: [
      { type: 'required', message: 'Code is Required !' },
      { type: 'minlength', value: 3, message: 'Minimum Length is 3 Characters !' },
      { type: 'maxlength', value: 10, message: 'Maximum Length is 20 Characters !' },
      { type: "pattern", message: "Not Valid Format !", }
    ],

    name: [
      { type: 'required', message: 'Name is Required !' },
      { type: 'minlength', value: 3, message: 'Minimum Length is 3 Characters !' },
      { type: 'maxlength', value: 20, message: 'Maximum Length is 20 Characters !' },
      { type: "pattern", message: "Not Valid Format !" }
    ],

    companyDateOfEstablishment: [
      { type: 'required', message: 'Date Of Establishment is Required' },
      { type: 'minlength', value: 3, message: 'Minimum Length is 3 Characters' },
      { type: 'maxlength', value: 20, message: 'Maximum Length is 20 Characters' },
      { type: "pattern", message: "Not Valid Format !", }
    ],
    companyWebsite: [
      { type: 'required', message: 'Website is Required !' },
      { type: 'minlength', value: 6, message: 'Minimum Length is 6 Characters !' },
      { type: 'maxlength', value: 30, message: 'Maximum Length is 20 Characters !' },
      { type: "pattern", message: "Not Valid Format !", }

    ],
    contactEmail: [
      { type: 'required', message: 'Contact Email is Required !' },
      { type: 'minlength', value: 6, message: 'Minimum Length is 6 Characters !' },
      { type: 'maxlength', value: 20, message: 'Maximum Length is 20 Characters !' },
      { type: "pattern", message: "Not Valid Format !" }
    ],
    companyMobileNumber: [
      { type: 'required', message: 'Mobile Numberis Required !' },
      { type: 'minlength', value: 10, message: 'Minimum Length is 10 Characters !' },
      { type: 'maxlength', value: 10, message: 'Maximum Length is 10 Characters !' },
      { type: "pattern", message: "Not Valid Format !" }
    ],

    firstName: [
      { type: "required", message: "First Name is Required !" },
      { type: "minlength", value: 3, message: "Minimum Length is 3 Characters !" },
      { type: "minlength", value: 15, message: "Maximum Length is 15 Characters !" },
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