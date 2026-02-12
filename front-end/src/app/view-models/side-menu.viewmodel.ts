import { ClientThemeSM } from "../service-models/app/v1/client/client-theme-s-m";
import { SQLReportMasterSM } from "../service-models/app/v1/client/s-q-l-report-master-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class SideMenuViewModel extends BaseViewModel {
  override PageTitle: string = "Side-Menu";
  showTooltip: boolean = false;
  tokenRole: string = "";
  userName!: string;
  userProfilePic: string = "";
  isProfilePicUploaded: boolean = false;
  clientTheme: ClientThemeSM[] = [];
  sqlReportList: SQLReportMasterSM[] = [];
  sqlReportObj: SQLReportMasterSM = new SQLReportMasterSM();
}
