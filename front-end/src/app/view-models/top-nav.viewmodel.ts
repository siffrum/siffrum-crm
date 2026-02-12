import { ClientThemeSM } from "../service-models/app/v1/client/client-theme-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class TopNavViewModel extends BaseViewModel {
    override PageTitle: string = 'Top-Nav';
    userName!: string;
    clientTheme: ClientThemeSM[] = [];
    showTooltip: boolean = false;

}