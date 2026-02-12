import { ClientThemeSM } from "../service-models/app/v1/client/client-theme-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class SettingViewModel extends BaseViewModel {
    override PageTitle: string = "Settings";
    clientTheme: ClientThemeSM[] = [];
    tablocation: settingTabs = settingTabs.accountSetting;
    showTooltip: boolean = false;
}

export enum settingTabs {
    accountSetting = 0,
    security = 1,
    notification = 2
}