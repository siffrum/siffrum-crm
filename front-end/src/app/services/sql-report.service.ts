import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { SqlReportClient } from '../clients/sql-report.client';
import { ApiResponse } from '../service-models/foundation/api-contracts/base/api-response';
import { SQLReportMasterSM } from '../service-models/app/v1/client/s-q-l-report-master-s-m';
import { ApiRequest } from '../service-models/foundation/api-contracts/base/api-request';
import { DeleteResponseRoot } from '../service-models/foundation/common-response/delete-response-root';
import { SQLReportDataModelSM } from '../service-models/app/v1/client/s-q-l-report-data-model-s-m';
import { SQLReportResponseModel } from '../service-models/app/v1/client/s-q-l-report-response-model';

@Injectable({
  providedIn: 'root'
})
export class SqlReportService extends BaseService {
/**@dev Musaib
 * This TypeScript file defines the SqlReportService,
 *  The service responsible for interacting with SQL reports using the SqlReportClient.
 * The service includes methods for fetching SQL report lists,
 * retrieving query details by ID, adding new SQL queries,
 * updating existing queries, deleting queries by ID, and previewing data using SQL query strings.
*/
  constructor(private sqlReportClient:SqlReportClient) {
    super()
   }
/**
 * get query Report List
 * @returns
 */
   async getAllSqlReportList(): Promise<ApiResponse<SQLReportMasterSM[]>> {
    return await this.sqlReportClient.GetAllSqlReportlist();
  }
/**
 * Get SQl Quer Details By Query Id
 * @param queryId
 * @returns 
 */
  async getQueryDetailsById(queryId:number):Promise<ApiResponse<SQLReportMasterSM>>{
    return await this.sqlReportClient.GetQueryDetailsById(queryId);
  }
  /**
   * ADD New Query  String
   * @param sqlQueryObj
   * @returns 
   */
  async addSqlQuery(sqlQueryObj:SQLReportMasterSM): Promise<ApiResponse<SQLReportMasterSM>> {
    let apiRequest = new ApiRequest<SQLReportMasterSM>();
    apiRequest.reqData=sqlQueryObj
    return await this.sqlReportClient.AddSqlQuery(apiRequest);
}
    /**
     * Update SQL Query By Query Id
     * @param sqlQueryObj
     * @returns 
     */
    async updateSqlQueryById(sqlQueryObj: SQLReportMasterSM): Promise<ApiResponse<SQLReportMasterSM>> {
      let apiRequest = new ApiRequest<SQLReportMasterSM>
      apiRequest.reqData=sqlQueryObj
      return await this.sqlReportClient.UpdateSqlQuerById(apiRequest);
  }
/**
 * Delete Query String By Query Id
 * @param sqlQueryId
 * @returns 
 */

  async deleteSqlQueryById(sqlQueryId: number): Promise<ApiResponse<DeleteResponseRoot>> {
      return await this.sqlReportClient.DeleteSqlQueryById(sqlQueryId);
  }

    /**
   *GET Query Preview for Selected Query
   * @param queryString
   * @returns
   */
    async getQueryPreviewByQuerystring(queryString:String):Promise<ApiResponse<SQLReportResponseModel>>{
      return await this.sqlReportClient.GetQueryPreviewByQuerystring(queryString);
    }
    //Admin
    /**
     * Get all SQL  reports List For Admin
     * @returns
     */
    async getAllSqlReportsForAdmin(): Promise<ApiResponse<SQLReportMasterSM[]>> {
      return await this.sqlReportClient.GetAllSqlReportsForAdmin();
    }
    /**
     * Get Report By Selected Report Name
     * @param pageNo
     * @param reportId
     * @returns
     */
    async getlSqlReportBySelectedReportName(reportId:number,pageNo:number): Promise<ApiResponse<SQLReportDataModelSM>> {
      return await this.sqlReportClient.GetlSqlReportBySelectedReportName(reportId,pageNo);
    }
}
