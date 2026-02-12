import { ClientCompanyDetailSM } from "../service-models/app/v1/client/client-company-detail-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class SuperCompanyListViewModel extends BaseViewModel {
    override PageTitle: string = 'Company Details';
    showTooltip: boolean = false;
    isDisabled: boolean = true;
    isReadonly = true;
    showCompanyDetailButton: boolean = false;
    showCompanyAddressButton: boolean = false;
    clientCompanyDetaillist: ClientCompanyDetailSM[] = [];
    clientCompanyDetail: ClientCompanyDetailSM = new ClientCompanyDetailSM();
    isTrue: boolean = false;
}


