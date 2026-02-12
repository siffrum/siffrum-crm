import { TokenRequestSM } from "src/app/service-models/app/token/token-request-s-m";
import { BaseViewModel } from "./base.viewmodel";
export class AdminLoginViewModel extends BaseViewModel {
    override PageTitle: string = 'Super Admin';
    showTooltip: boolean = false;
    tokenRequest: TokenRequestSM = new TokenRequestSM();
    rememberMe: boolean = false;
    eyeDefault = 'default';
    defaultTheme: string = "";
    hide: boolean = true;
    validations = {
        username: [
            { type: 'required', message: 'Username is Requierd' },
            { type: 'minlength', message: 'Username can not be empty' },
        ],
        password: [
            { type: 'required', message: 'Password is Requierd' },
            { type: 'minlength', message: 'Password can not be empty' },
        ],
    }
}