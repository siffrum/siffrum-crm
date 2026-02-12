import { AxiosResponse, Method } from 'axios';
import { AdditionalRequestDetails, Authentication } from '../../internal-models/additional-request-details';
import { StorageService } from 'src/app/services/storage.service';
import { AppConstants } from '../../../app-constants';
import { StorageCache } from '../helpers/storage-cache.helper';
import { BaseAjaxClient } from './base-ajax.client';
import { DictionaryCollection } from 'src/app/internal-models/dictionary-collection';
import { IDictionaryCollection } from 'src/app/internal-models/Idictionary-collection';
import { ApiRequest } from 'src/app/service-models/foundation/api-contracts/base/api-request';
import { ApiResponse } from 'src/app/service-models/foundation/api-contracts/base/api-response';
import { ErrorData } from 'src/app/service-models/foundation/api-contracts/error-data';
import { QueryFilter } from 'src/app/service-models/foundation/api-contracts/query-filter';
import { ApiErrorTypeSM } from 'src/app/service-models/foundation/enums/api-error-type-s-m.enum';
import { CommonResponseCodeHandler } from '../helpers/common-response-code-handler.helper';
import { CommonUtils } from '../helpers/common-utils.helper';
import { environment } from 'src/environments/environment';

export abstract class BaseApiClient extends BaseAjaxClient {

    constructor(protected storageservice: StorageService,
        protected storageCacheHelper: StorageCache, private commonResponseCodeHandler: CommonResponseCodeHandler) {
        super();
    }
    protected GetResponseAsync = async <InReq, OutResp>(relativeUrl: string, reqMethod: Method = 'GET', reqBody: ApiRequest<InReq> | null = null,
        additionalRequestDetails: AdditionalRequestDetails<OutResp> = new AdditionalRequestDetails<OutResp>(false))
        : Promise<ApiResponse<OutResp>> => {

        let responseEntity: ApiResponse<OutResp> | null = null;
        let axiosResp: AxiosResponse<any> | null = null;
        ;

        if (additionalRequestDetails == null)
            throw new Error('AdditionalRequestDetails cannot be null, do not pass if not required.');

        try {
            const fullUrlToHit = CommonUtils.CombineUrl(environment.apiBaseUrl, relativeUrl);
            responseEntity = await this.storageCacheHelper.GetResponseFromDbIfPresent<OutResp>(
                fullUrlToHit, reqMethod, additionalRequestDetails);
            if (responseEntity != null)
                return responseEntity;
            else {
                // add headers and all. and call base Ajax
                additionalRequestDetails.headers = await this.AddCommonHeaders(additionalRequestDetails.headers);
                if (additionalRequestDetails.enableAuth === Authentication.true) {
                    let token = await this.storageservice.getTokenFromStorage();
                    if (token == null || token === '')
                        throw new Error(`Token not found for URL - '${relativeUrl}'.`);
                    else
                        additionalRequestDetails.headers.Add('Authorization', 'Bearer ' + token);
                }
                if (reqMethod === 'GET') {
                    // validations
                    // eg no body, proper url etc
                } else if (reqMethod === 'POST') {
                    // validations
                }
                else if (reqMethod === 'DELETE') {
                    // validations
                }

                axiosResp = await this.GetHttpDataAsync<ApiRequest<InReq>>(
                    fullUrlToHit, reqMethod, reqBody, additionalRequestDetails.headers, additionalRequestDetails.contentType);

                if (this.commonResponseCodeHandler.handlerDict.Keys().includes(axiosResp.status.toString())) {
                    let errMessage = this.commonResponseCodeHandler.handlerDict.Item(axiosResp.status.toString())(axiosResp)
                    return this.CreateGenericApiResponseObject(errMessage);
                }

                responseEntity = await this.CreateResponseEntityFromAxiosResp<OutResp>(
                    axiosResp, additionalRequestDetails.custRespResolver);
                if (responseEntity == null) { throw new Error('Null Response Formed.'); }


                // add response to cache if applicable
                await this.storageCacheHelper.AddOrUpdateResponseInCacheIfApplicable<OutResp>(
                    fullUrlToHit, reqMethod, additionalRequestDetails, responseEntity);
                return responseEntity;
            }

        } catch (x) {
            let msg = '';
            if (x instanceof Error)
                msg = x.message;
            else
                msg = JSON.stringify(x);
            const resp = this.CreateGenericApiResponseObject<OutResp>(msg);
            resp.axiosResponse = axiosResp;
            return resp;
        }
    }
    private CreateResponseEntityFromAxiosResp = async<OutResp>(axiosResp: AxiosResponse,
        respResolver: ((resp: AxiosResponse) => ApiResponse<OutResp>) | null): Promise<ApiResponse<OutResp> | null> => {

        let retObject: ApiResponse<OutResp> | null = null;
        if (this.IsSuccessCode(axiosResp.status)) {
            if (respResolver != null) {
                const data = respResolver(axiosResp);
                data.axiosResponse = axiosResp;
                retObject = data;
            } else {
                const data = axiosResp.data as ApiResponse<OutResp>; // need to check this how to remove additional props
                data.axiosResponse = axiosResp;
                retObject = data;
            }
        } else {
            // either response has boday as formatted response or not.
            if (axiosResp.data != null && axiosResp.data.isError !== undefined) {
                retObject = axiosResp.data as ApiResponse<OutResp>;
                retObject.axiosResponse = axiosResp;
            }
        }

        if (retObject == null) {
            retObject = this.CreateGenericApiResponseObject<OutResp>(`Unknown error occured - status code '${axiosResp.status}'`);
            retObject!.axiosResponse = axiosResp;
        }
        return retObject;

    }
    protected CreateGenericApiResponseObject = <OutResp>(addMsg: string): ApiResponse<OutResp> => {
        const resp = new ApiResponse<OutResp>();
        resp.isError = true;
        resp.errorData = new ErrorData();
        resp.errorData.displayMessage = addMsg;
        resp.errorData.apiErrorType = ApiErrorTypeSM.FrameworkException_Log;
        return resp;
    }
    protected ApplyQueryFilterToUrl(currentUrlToHit: string, queryFilter?: QueryFilter | null): string {

        if (queryFilter === undefined || queryFilter === null) {
            return currentUrlToHit;
        }
        let urlQuery: string = '';
        //code for search, orderby etc to be added needs to be as per the odata query format        
        if (queryFilter.skip!=undefined && queryFilter.skip >= 0 && queryFilter.top!=undefined && queryFilter.top > 0)
            urlQuery += `$skip=${queryFilter.skip}&$top=${queryFilter.top}`;

        // if (queryFilter.searchText != null && queryFilter.searchText != undefined && queryFilter.searchText.length > 0) {
        //     if (urlQuery != '' && urlQuery.length > 0)
        //         urlQuery += '&';
        //     urlQuery += `search=${queryFilter.searchText}`;
        // }
        // add other query like orderby etc as per odata
        if (currentUrlToHit.indexOf('?') > 0)
            currentUrlToHit = `${currentUrlToHit}&${urlQuery}`;
        else
            currentUrlToHit = `${currentUrlToHit}?${urlQuery}`;

        return currentUrlToHit;
    }
    private async AddCommonHeaders(commonHeaders: IDictionaryCollection<string, string> | null): Promise<IDictionaryCollection<string, string>> {
        if (commonHeaders == null)
            commonHeaders = new DictionaryCollection<string, string>();
        commonHeaders.Add(AppConstants.HeadersName.ApiType, AppConstants.HeadersValue.ApiType);
        commonHeaders.Add(AppConstants.HeadersName.DevApk, AppConstants.HeadersValue.DevApk);
        commonHeaders.Add(AppConstants.HeadersName.AppVersion, AppConstants.HeadersValue.AppVersion);
        return commonHeaders;
    }
}



