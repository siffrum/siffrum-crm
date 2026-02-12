import { ClientCompanyAddressSM } from "../service-models/app/v1/client/client-company-address-s-m";
import { ClientCompanyDetailSM } from "../service-models/app/v1/client/client-company-detail-s-m";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class CompanyOverviewViewModel extends BaseViewModel {
    override  PageTitle: string = 'Company-Overview';
    showTooltip: boolean = false;
    companyDetailList: ClientCompanyDetailSM[] = [];
    company: ClientCompanyDetailSM = new ClientCompanyDetailSM();
    companyAddress: ClientCompanyAddressSM = new ClientCompanyAddressSM();
    tabLocation: CompanyProfileTabs = CompanyProfileTabs.Overview;
    isCompanyDetailsDisabled: boolean = true;
    showCompanyDetailsUpdateButton: boolean = false;
    isCompanyAddressDisabled: boolean = true;
    showCompanyAddressUpdateButton: boolean = false;
    isReadonly = true;
    showCompanyAddressButton: boolean = false;
    companyLogo: string = '';
    showOverviewTab: boolean = false;
    showAddressTab: boolean = false;
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
            { type: 'maxlength', value: 30, message: 'Maximum Length is 30 Characters !' },
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


export enum CompanyProfileTabs {
    Overview = 0,
    Address = 1,
}