import { AxiosResponse } from "axios";
import { AppConstants } from "src/app-constants";
import { DictionaryCollection } from "src/app/internal-models/dictionary-collection";
import { IDictionaryCollection } from "src/app/internal-models/Idictionary-collection";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { ErrorLog } from "../service-models/internal/error-log";
import { BaseAjaxClient } from "./base-client/base-ajax.client";


export class LoggerClient extends BaseAjaxClient {


    constructor() {
        super();
    }

    public SendLogsToServerAsync = async (logsArr: Array<ErrorLog>, headers: IDictionaryCollection<string, string> | null)
        : Promise<AxiosResponse> => {
        // think if we nee Base Req Here, if so, move class out of helpers.
        let apiReq = new ApiRequest<Array<ErrorLog>>();
        apiReq.reqData = logsArr;
        if (headers == null)
            headers = new DictionaryCollection<string, string>();
        headers.Add(AppConstants.HeadersName.ApiType, AppConstants.HeadersValue.ApiType);
        headers.Add(AppConstants.HeadersName.DevApk, AppConstants.HeadersValue.DevApk);
        headers.Add(AppConstants.HeadersName.AppVersion, AppConstants.HeadersValue.AppVersion);
        return this.GetHttpDataAsync<ApiRequest<Array<ErrorLog>>>(AppConstants.ApiUrls.LOG_URL,
            'POST', apiReq, headers, 'application/json');
    }
}