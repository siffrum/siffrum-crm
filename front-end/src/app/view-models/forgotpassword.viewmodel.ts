import { ForgotPasswordSM } from "../service-models/app/v1/app-users/forgot-password-s-m";
import { BaseViewModel } from "./base.viewmodel";
export class ForgotPasswordViewModel extends BaseViewModel {
    override PageTitle: string = 'Forgot Password';
    forgotPasswordDetails: ForgotPasswordSM = new ForgotPasswordSM();
    FormSubmitted = false;
    showTooltip: boolean = false;
    ValidationData = {
        loginId: [
            { type: 'required', message: 'Login Id is Required' },
        ],
    }
}