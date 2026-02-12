import { ContactUsSM } from "../service-models/app/v1/general/contact-us-s-m";
import { LicenseTypeSM } from "../service-models/app/v1/license/license-type-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class WebsiteViewModel extends BaseViewModel {
  override PageTitle: string = '';
  displayStyle = 'none';
  formSubmitted: boolean = false;
  contactUsObj = new ContactUsSM();
  LicenseTypeList: LicenseTypeSM[] = [];

  validations = {
    name: [
      { type: "required", message: "Name is Required !" },
      { type: 'minlength', value: 3, message: 'Minimum Length is 3 Characters !' },
      { type: 'maxlength', value: 50, message: 'Maximum Length is 50 Characters !' }
    ],
    email: [
      { type: "required", message: "email is Required !" },
      { type: 'minlength', value: 6, message: 'Minimum Length is 2 Characters !' },
      { type: 'maxlength', value: 50, message: 'Maximum Length is 50 Characters !' },
      { type: "pattern", message: "Not Valid Format !" },
    ],
    subject: [
      { type: "required", message: "Subject is Required !" },
      { type: 'minlength', value: 3, message: 'Minimum Length is 3 Characters !' },
      { type: 'maxlength', value: 50, message: 'Maximum Length is 50 Characters !' },
      { type: "pattern", message: "Not Valid Format !" },
    ],
    message: [
      { type: "required", message: "message is Required !" },
      { type: 'minlength', value: 5, message: 'Minimum Length is 5 Characters !' },
      { type: 'maxlength', value: 200, message: 'Maximum Length is 200 Characters !' },
      { type: "pattern", message: "Not Valid Format !" },
    ],
    departmentDescription: [
      { type: 'minlength', value: 5, message: 'Minimum Length is 5 Characters !' },
      { type: 'maxlength', value: 200, message: 'Maximum Length is 50 Characters !' },
      { type: "pattern", message: "Not Valid Format !" },
    ]
  };
}