import { ClientUserAddressSM } from "../service-models/app/v1/app-users/client-user-address-s-m";
import { ClientCompanyDetailSM } from "../service-models/app/v1/client/client-company-detail-s-m";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class EmployeeAddressInfoViewModel extends BaseViewModel {
    override PageTitle: string = 'Employee-Address-Info';
    showTooltip: boolean = false;
    employeeAddressList: ClientUserAddressSM[] = [];
    employeeAddress: ClientUserAddressSM = new ClientUserAddressSM();
    company: ClientCompanyDetailSM = new ClientCompanyDetailSM();
    showButton: boolean = true;
    editMode: boolean = false;
    displayStyle = "none";
    formSubmitted: boolean = false;
    validations = {
        country: [
            { type: 'required', message: 'Country is Required' },
            { type: 'minlength', value: 3, message: 'Minimum Length is 3 Characters' },
            { type: 'maxlength', value: 20, message: 'Maximum Length is 20 Characters' },

            {
                type: "pattern",
                message: "Not Valid Format !",
            }
        ],
        state: [
            { type: 'required', message: 'State is Required' },
            { type: 'minlength', value: 2, message: 'Minimum Length is 2 Characters' },
            { type: 'maxlength', value: 20, message: 'Maximum Length is 20 Characters' },
            {
                type: "pattern",
                message: "Not Valid Format !",
            }
        ],

        city: [
            { type: 'required', message: 'City is Required' },
            { type: 'minlength', value: 3, message: 'Minimum Length is 3 Characters' },
            { type: 'maxlength', value: 30, message: 'Maximum Length is 30 Characters' },
            { type: "pattern", message: "Not Valid Format !" }
        ],
        pincode: [
            { type: 'required', message: 'Pin Code is Required' },
            { type: 'minlength', value: 4, message: 'Minimum Length is 4 Characters' },
            { type: 'maxlength', value: 20, message: 'Maximum Length is 20 Characters' },
            { type: "pattern", message: "Not Valid Format !" }
        ],
        address1: [
            { type: 'required', message: 'permenant is Required' },
            { type: 'minlength', value: 3, message: 'Minimum Length is 3 Characters' },
            { type: 'maxlength', value: 50, message: 'Maximum Length is 50 Characters' },
            { type: "pattern", message: "Not Valid Format !" }
        ],
        address2: [
            { type: 'minlength', value: 3, message: 'Minimum Length is 3 Characters' },
            { type: 'maxlength', value: 50, message: 'Maximum Length is 50 Characters' },
            { type: "pattern", message: "Not Valid Format !" }
        ]
    }
}