import { TokenRequestSM } from "../service-models/app/token/token-request-s-m";
import { ClientThemeSM } from "../service-models/app/v1/client/client-theme-s-m";
import { CompanyModulesSM } from "../service-models/app/v1/client/company-modules-s-m";
import { CompanyLicenseDetailsSM } from "../service-models/app/v1/license/company-license-details-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class LoginViewModel extends BaseViewModel {
    override PageTitle: string = 'Login';
    showTooltip: boolean = false;
    hide: boolean = true;
    rememberMe: boolean = false;
    eyeDefault = 'default';
    clientTheme: ClientThemeSM = new ClientThemeSM();
    defaultTheme: string = "";
    listRoles = new Array<string>();
    tokenRequest: TokenRequestSM = new TokenRequestSM();
    modulePermissionList: CompanyModulesSM[] = [];
    mineUserActiveLicense: CompanyLicenseDetailsSM = new CompanyLicenseDetailsSM();
    // userActiveTrialLicense: CompanyLicenseDetailsSM = new CompanyLicenseDetailsSM();

    validations = {
        Companycode: [
            { type: 'required', message: 'Company Code is Requierd' },
            { type: 'minlength', message: 'Company Code can not be empty' },
        ],
        SelectRole: [
            { type: 'required', message: 'Selected Role is Required' },
            { type: 'minlength', message: 'Selected Role can not be empty' },
        ],
        password: [
            { type: 'required', message: 'Password is required' },
            { type: 'minlength', message: ' password can not be empty' },
        ],
        username: [
            { type: 'required', message: 'Username is required' },
            { type: 'minlength', message: 'Username can not be empty' },
        ]
    }
}
