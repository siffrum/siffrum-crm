import { SQLReportDataModelSM } from "../service-models/app/v1/client/s-q-l-report-data-model-s-m";
import { SQLReportMasterSM } from "../service-models/app/v1/client/s-q-l-report-master-s-m";
import { SQLReportResponseModel } from "../service-models/app/v1/client/s-q-l-report-response-model";
import { BaseViewModel } from "./base.viewmodel";

export class SqlReportViewModel extends BaseViewModel {
    override PageTitle: string = 'SQL REPORT'
    // sqlReportDataModel: SQLReportDataModelSM = new SQLReportDataModelSM();
    sqlReportDataModel: SQLReportResponseModel = new  SQLReportResponseModel()
    sqlReportMasterList: SQLReportMasterSM[] = [];
    sqlReportMaster: SQLReportMasterSM = new SQLReportMasterSM();
    listdata: any;
    showTooltip: boolean = false;
    editMode: boolean = false;
    addMode: boolean = false;
    form: boolean = false;
    preViewTable: boolean = false;
    displayStyle = 'none'
    formSubmitted = false;
    validations = {
        reportName: [
            { type: 'required', message: 'Report Name is Required' },
            { type: 'minlength', value: 3, message: 'Minimum Length is 3 Characters' },
            { type: 'maxlength', value: 20, message: 'Maximum Length is 20 Characters' },
            { type: "pattern", message: "Not Valid Format !" }
        ],
        sQLQuery: [
            { type: 'required', message: 'Branch Name is Required' },

        ]
    }
    syncedData: any;
}