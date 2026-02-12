import { Injectable } from "@angular/core";
import { BaseApiClient } from "./base-client/base-api.client";
import { StorageService } from "../services/storage.service";
import { StorageCache } from "./helpers/storage-cache.helper";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { SQLReportMasterSM } from "../service-models/app/v1/client/s-q-l-report-master-s-m";
import { AppConstants } from "src/app-constants";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { DeleteResponseRoot } from "../service-models/foundation/common-response/delete-response-root";
import { SQLReportDataModelSM } from "../service-models/app/v1/client/s-q-l-report-data-model-s-m";
import { SQLReportResponseModel } from "../service-models/app/v1/client/s-q-l-report-response-model";

@Injectable({
  providedIn: "root",
})
/**
 * @Developer Musaib
 *  This TypeScript file contains the implementation of the SqlReportClient ,
 *  which provides methods for interacting with SQL reports.
 *  The Client  includes functions for fetching SQL query lists,
 *  retrieving query details by ID,
 *  adding new SQL queries,
 *  updating existing queries,
 *  deleting queries by ID, and previewing data using a SQL query string.
 */
export class SqlReportClient extends BaseApiClient {
  constructor(
    storageService: StorageService,
    storageCache: StorageCache,
    commonResponseCodeHandler: CommonResponseCodeHandler
  ) {
    super(storageService, storageCache, commonResponseCodeHandler);
  }
  /**
   * Get SQL queries List
   * @returns
   */
  GetAllSqlReportlist = async (): Promise<ApiResponse<SQLReportMasterSM[]>> => {
    let finalUrl = `${AppConstants.ApiUrls.SQL_REPORT}`;
    let resp = await this.GetResponseAsync<null, SQLReportMasterSM[]>(
      `${finalUrl}`,
      "GET"
    );
    return resp;
  };
  /**
   * Get SQL Query By Query Id
   * @param sqlQueryId
   * @returns
   */
  GetQueryDetailsById = async (
    sqlQueryId: number
  ): Promise<ApiResponse<SQLReportMasterSM>> => {
    let resp = await this.GetResponseAsync<number, SQLReportMasterSM>(
      `${AppConstants.ApiUrls.SQL_REPORT}/${sqlQueryId}`,
      "GET"
    );
    return resp;
  };
  /**
   * ADD New Sql Query
   * @param sqlQueryObj
   * @returns
   */
  AddSqlQuery = async (sqlQueryObj: ApiRequest<SQLReportMasterSM> ): Promise<ApiResponse<SQLReportMasterSM>> => {
    let resp = await this.GetResponseAsync<
      SQLReportMasterSM,
      SQLReportMasterSM>(`${AppConstants.ApiUrls.SQL_REPORT}`, "POST", sqlQueryObj);
    return resp;
  };
  /**
   * Update Sql Query
   * @param sqlQueryObj
   * @returns
   */
  UpdateSqlQuerById = async (
    sqlQueryObj: ApiRequest<SQLReportMasterSM>
  ): Promise<ApiResponse<SQLReportMasterSM>> => {
    let resp = await this.GetResponseAsync<
      SQLReportMasterSM,
      SQLReportMasterSM
    >(
      `${AppConstants.ApiUrls.SQL_REPORT}/${sqlQueryObj.reqData.id}`,
      "PUT",
      sqlQueryObj
    );
    return resp;
  };
  /**
   * Delete SQL Query By ID
   * @param sqlQueryId
   * @returns
   */
  DeleteSqlQueryById = async (
    sqlQueryId: number
  ): Promise<ApiResponse<DeleteResponseRoot>> => {
    let resp = await this.GetResponseAsync<number, DeleteResponseRoot>(
      `${AppConstants.ApiUrls.SQL_REPORT}/${sqlQueryId}`,
      "DELETE"
    );
    return resp;
  };
  /**
   * Get DATA preview By Query String
   * @param queryString
   * @returns
   */

  GetQueryPreviewByQuerystring = async (
    queryString: String
  ): Promise<ApiResponse<SQLReportResponseModel>> => {
    let finalUrl = `${AppConstants.ApiUrls.SQL_REPORT}`;
    let resp = await this.GetResponseAsync<String, SQLReportResponseModel>(
      `${finalUrl}/SqlReports?query=${queryString}`,
      "GET"
    );
    return resp;
  };
  //ADMIN
    /**
     * Get all SQL  reports List For Admin
     * @returns
     */
  GetAllSqlReportsForAdmin = async (): Promise<ApiResponse<SQLReportMasterSM[]>> => {
  let resp = await this.GetResponseAsync<null,SQLReportMasterSM[]>(`${AppConstants.ApiUrls.SQL_REPORT}`,"GET");
  return resp;
};
  /**
     * Get Report By Selected Report Name
     * @param pageNo
     * @param reportId
     * @returns
     */
GetlSqlReportBySelectedReportName = async (reportId:number,pageNo:number): Promise<ApiResponse<SQLReportDataModelSM>> => {
  let resp = await this.GetResponseAsync<null,SQLReportDataModelSM>(`${AppConstants.ApiUrls.SQL_REPORT}/SelectReportsById/${reportId}?pageNo=${pageNo}`,"GET");
  return resp;
};
}
