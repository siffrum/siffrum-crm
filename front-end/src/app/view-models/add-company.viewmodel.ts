import { ClientUserSM } from "../service-models/app/v1/app-users/client-user-s-m";
import { ClientCompanyAddressSM } from "../service-models/app/v1/client/client-company-address-s-m";
import { ClientCompanyDetailSM } from "../service-models/app/v1/client/client-company-detail-s-m";
import { CompanyModulesSM } from "../service-models/app/v1/client/company-modules-s-m";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class AddCompanyViewModel extends BaseViewModel {
    override PageTitle: string = "Add-Company";
    AddCompanyDetail: ClientCompanyDetailSM = new ClientCompanyDetailSM();
    AddCompanyAddress: ClientCompanyAddressSM = new ClientCompanyAddressSM();
    companyModulesDetail: CompanyModulesSM = new CompanyModulesSM();
    companyModulePermissions: PermissionSM[] = []
    addAdmin: ClientUserSM = new ClientUserSM();
    isReadonly: boolean = false;
    isAddMode: boolean = true;
    FormSubmitted: boolean = false;
    disabledDate!: string;
    maxDate = new Date();
    selectedDate: Date = new Date();
    selectAll: boolean = false;
    currentDate = new Date();
    formSubmitted: boolean = false;
    companyDetailsValidations = {
        companyCode: [
            { type: 'required', message: 'Code is Required !' },
            { type: 'minlength', value: 3, message: 'Minimum Length is 3 Characters !' },
            { type: 'maxlength', value: 10, message: 'Maximum Length is 20 Characters !' },
            { type: "pattern", message: "Not Valid Format !", }
        ],
        description: [
            { type: 'minlength', value: 3, message: 'Minimum Length is 3 Characters !' },
            { type: 'maxlength', value: 20, message: 'Maximum Length is 20 Characters !' },
            { type: "pattern", message: "Not Valid Format !" }
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
        ]
    };
    companyAddressValidations = {
        address1: [
            { type: 'required', message: 'permenant is Required !' },
            { type: 'minlength', value: 3, message: 'Minimum Length is 3 Characters !' },
            { type: 'maxlength', value: 20, message: 'Maximum Length is 20 Characters !' },
            { type: "pattern", message: "Not Valid Format !" }
        ],
        address2: [
            { type: 'minlength', value: 3, message: 'Minimum Length is 3 Characters !' },
            { type: 'maxlength', value: 20, message: 'Maximum Length is 20 Characters !' },
            { type: "pattern", message: "Not Valid Format !" }
        ],
        country: [
            { type: 'required', message: 'Country is Required !' },
            { type: 'minlength', value: 3, message: 'Minimum Length is 3 Characters !' },
            { type: 'maxlength', value: 20, message: 'Maximum Length is 20 Characters !' },
            { type: "pattern", message: "Not Valid Format !" }
        ],
        state: [
            { type: 'required', message: 'State is Required !' },
            { type: 'minlength', value: 3, message: 'Minimum Length is 3 Characters !' },
            { type: 'maxlength', value: 20, message: 'Maximum Length is 20 Characters !' },
            { type: "pattern", message: "Not Valid Format !" }
        ],
        city: [
            { type: 'required', message: 'City is Required !' },
            { type: 'minlength', value: 3, message: 'Minimum Length is 3 Characters !' },
            { type: 'maxlength', value: 20, message: 'Maximum Length is 20 Characters !' },
            { type: "pattern", message: "Not Valid Format !", }
        ],
        pinCode: [
            { type: 'required', message: 'Pin Code is Required !' },
            { type: 'minlength', value: 4, message: 'Minimum Length is 4 Characters !' },
            { type: 'maxlength', value: 8, message: 'Maximum Length is 8 Characters !' },
            { type: "pattern", message: "Not Valid Format !", }

        ]

    }
}


