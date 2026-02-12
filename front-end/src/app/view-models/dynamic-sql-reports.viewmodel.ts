import { SQLReportDataModelSM } from "../service-models/app/v1/client/s-q-l-report-data-model-s-m";
import { SQLReportMasterSM } from "../service-models/app/v1/client/s-q-l-report-master-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class DynamicSqlReportsViewModel extends BaseViewModel{
  sqlReportList: SQLReportMasterSM[] = [];
  sqlReportObj: SQLReportMasterSM = new SQLReportMasterSM();
  sqlReportDataModelObj: SQLReportDataModelSM = new SQLReportDataModelSM();
  selectedQueryId: number = 0;
  selectedReportName: string = "";
  showTable: boolean = false;
  showPreviuosBtn:boolean=false;
}
