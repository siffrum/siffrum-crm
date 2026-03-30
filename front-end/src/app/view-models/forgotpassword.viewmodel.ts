import { ForgotPasswordSM } from "../service-models/app/v1/app-users/forgot-password-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class ForgotPasswordViewModel extends BaseViewModel {
  override PageTitle: string = "Forgot Password";
  forgotPasswordDetails: ForgotPasswordSM = new ForgotPasswordSM();
  FormSubmitted = false;
  showTooltip: boolean = false;

  ValidationData = {
    companyCode: [
      { type: "required", message: "Company Code is Required" },
    ],
    userName: [
      { type: "required", message: "Username is Required" },
    ],
  };
}