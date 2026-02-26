import axios, { AxiosRequestConfig, Method, AxiosResponse } from 'axios';
import { DictionaryCollection } from 'src/app/internal-models/dictionary-collection';
import { IDictionaryCollection } from 'src/app/internal-models/Idictionary-collection';

export abstract class BaseAjaxClient {

  constructor() {}

  protected GetHttpDataAsync = async <Req>(
    fullReqUrl: string,
    method: Method,
    reqBody: Req | null,
    headers: IDictionaryCollection<string, string>,
    contentType: string
  ): Promise<AxiosResponse> => {

    if (contentType !== '' && contentType !== 'application/json') {
      throw new Error('Content Type other then JSON not supported at the moment.');
    }

    if (headers == null) {
      headers = new DictionaryCollection<string, string>();
    }

    // keep your original behavior
    headers.Add('Content-Type', contentType);

    let reqBodyTxt = '';
    reqBodyTxt = JSON.stringify(reqBody);

    let response = await this.FetchAsync(fullReqUrl, method, headers, reqBodyTxt);
    if (response == null) {
      throw new Error('Response null after api call. please report the event to administrator.');
    }
    return response;
  }

  /**
   * EG - Headers > { 'content-type': 'application/json' };
   */
  private FetchAsync = async (
    fullReqUrl: string,
    reqMethod: Method,
    headersToAdd: IDictionaryCollection<string, string>,
    reqBody: string
  ): Promise<AxiosResponse<any, any> | null> => {

    let hdrs: any = {};

    if (headersToAdd != null && headersToAdd.Count() > 0) {
      headersToAdd.Keys().forEach(key => {
        hdrs[key] = headersToAdd.Item(key);
      });
    }

    // ✅ IMPORTANT FIX FOR NGROK:
    // Forces ngrok to return real API JSON instead of the HTML warning page.
    hdrs['ngrok-skip-browser-warning'] = 'true';

    let config: AxiosRequestConfig<string> = this.GetAxiosConfig();
    config.url = fullReqUrl;
    config.method = reqMethod;
    config.headers = hdrs;

    // keep original behavior (send body as string)
    config.data = reqBody;

    let response = await axios.request(config);
    return response;
  }

  private GetAxiosConfig = (): AxiosRequestConfig => {
    let config = {
      url: '',
      method: 'get', // default

      // NOTE: axios uses baseURL not apiBaseUrl, but you are passing absolute urls anyway.
      apiBaseUrl: '',

      timeout: 0,
      withCredentials: false,
      responseType: 'json',

      onUploadProgress(progressEvent) {},
      onDownloadProgress(progressEvent) {},

      maxContentLength: 2000,

      validateStatus(status) {
        // do not throw exception from the promise, if needed handle at single place in base client.
        return true;
      },

      maxRedirects: 5,
    } as AxiosRequestConfig;

    return config;
  }

  protected IsSuccessCode = (respStatusCode: number): boolean => {
    return respStatusCode >= 200 && respStatusCode < 300;
  }
}
