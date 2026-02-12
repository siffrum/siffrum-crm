
import { ForgotPasswordSM } from "../service-models/app/v1/app-users/forgot-password-s-m";
import { ResetPasswordRequestSM } from "../service-models/app/v1/app-users/reset-password-request-s-m";
import { BaseViewModel } from "./base.viewmodel";
export class ResetPasswordViewModel extends BaseViewModel {
  override PageTitle: string = 'Reset Password';
  FormSubmitted = false;
  forgotPasswordDetails: ForgotPasswordSM = new ForgotPasswordSM();
  resetPasswordDetails: ResetPasswordRequestSM = new ResetPasswordRequestSM();
  confirmPassword: string = ''
  validateAuthCode!: number;
  hidePassword: boolean = true;
  showTooltip: boolean = false;
  eyeDefault = 'default';
  hide: boolean = true;
  invalid: boolean = false;
  ValidationData = {
    newPassword: [
      { type: 'required', message: 'New Password is Required' },

    ],
    confirmPassword: [
      { type: 'required', message: 'Confirm Password is Required' },

    ]
  }
}



