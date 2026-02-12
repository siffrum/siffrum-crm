import { UpdateCredentialSM } from "../service-models/app/v1/app-users/update-credential-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class ChangePasswordViewModel extends BaseViewModel {
    override PageTitle: string = "Update Credentials";
    updateCredentials: UpdateCredentialSM = new UpdateCredentialSM();
    newPassword: string = ''
    hide: boolean = true;
    eyeDefault = 'default';
    showTooltip: boolean = false;
    validations = {
        oldPassword: [
            { type: 'required', message: 'Old Password is Requierd' },
            { type: 'minlength', message: 'Old password can not be empty' },
        ],
        newPassword: [
            { type: 'required', message: 'New password is Required' },
            { type: 'minlength', message: 'New password can not be empty' },
        ],
        confirmPassword: [
            { type: 'required', message: 'Confirm Password is required' },
            { type: 'minlength', message: 'confirm password can not be empty' },
        ]
    }

}