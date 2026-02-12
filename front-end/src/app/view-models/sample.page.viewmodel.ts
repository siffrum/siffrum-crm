import { TokenRequestSM } from "../service-models/app/token/token-request-s-m";
import { TokenResponseSM } from "../service-models/app/token/token-response-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class SamplePageViewModel extends BaseViewModel {
    override PageTitle: string = 'Sample';
    userId!: string;
    pwd!: string;
    tokenRequest!: TokenRequestSM;
    tokenResponse!: TokenResponseSM;
}