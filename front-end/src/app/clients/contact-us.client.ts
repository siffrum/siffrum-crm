import { Injectable } from "@angular/core";
import { BaseApiClient } from "./base-client/base-api.client";
import { StorageService } from "../services/storage.service";
import { StorageCache } from "./helpers/storage-cache.helper";
import { CommonResponseCodeHandler } from "./helpers/common-response-code-handler.helper";
import { ApiRequest } from "../service-models/foundation/api-contracts/base/api-request";
import { ApiResponse } from "../service-models/foundation/api-contracts/base/api-response";
import { AppConstants } from "src/app-constants";
import { DeleteResponseRoot } from "../service-models/foundation/common-response/delete-response-root";
import { AdditionalRequestDetails, Authentication } from "../internal-models/additional-request-details";
import { ContactUsSM } from "../service-models/app/v1/general/contact-us-s-m";

@Injectable({
    providedIn: "root",
  })
  export class ContactUsClient extends BaseApiClient{
    constructor( storageService: StorageService,
        storageCache: StorageCache,
        commonResponseCodeHandler: CommonResponseCodeHandler
      ) {
        super(storageService, storageCache, commonResponseCodeHandler);

    }

         /**  Get All Contact US Details
      * @DEV Musaib
   */
  GetContactUsData = async (): Promise<ApiResponse<ContactUsSM[]>> => {
  let resp = await this.GetResponseAsync<null,ContactUsSM[]>(
    `${AppConstants.ApiUrls.CONTACT_US}`,"GET");
  return resp;
};

  /** Get Contact  Details  By Id (GET method)
   * @param Id
   */
  GetContactUsById = async (Id: number ): Promise<ApiResponse<ContactUsSM>> => {
    let resp = await this.GetResponseAsync<number,ContactUsSM >(`${AppConstants.ApiUrls.CONTACT_US}/${Id}`);
    return resp;
  };
  /** Add New  Contact Us  Details
   * (POST method)
   */
  AddNewContactUsDetails = async (
    addcontactUs: ApiRequest<ContactUsSM> ): Promise<ApiResponse<ContactUsSM>> => {
    let resp = await this.GetResponseAsync< ContactUsSM, ContactUsSM >(`${AppConstants.ApiUrls.CONTACT_US}`, "POST", addcontactUs,new AdditionalRequestDetails<ContactUsSM>(false, Authentication.false ));
    return resp;
  };

  /**delete contact Details
   * @DEV : Musaib
   */
  DeleteContactUsById = async (
    Id: number
  ): Promise<ApiResponse<DeleteResponseRoot>> => {
    let resp = await this.GetResponseAsync<number, DeleteResponseRoot>(
      `${AppConstants.ApiUrls.CONTACT_US}/${Id}`,
      "DELETE"
    );
    return resp;
  };
  }