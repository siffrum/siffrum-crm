import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { ContactUsClient } from '../clients/contact-us.client';
import { ApiResponse } from '../service-models/foundation/api-contracts/base/api-response';
import { AppConstants } from 'src/app-constants';
import { ApiRequest } from '../service-models/foundation/api-contracts/base/api-request';
import { DeleteResponseRoot } from '../service-models/foundation/common-response/delete-response-root';
import { ContactUsSM } from '../service-models/app/v1/general/contact-us-s-m';

@Injectable({
  providedIn: 'root'
})
export class ContactUsService  extends BaseService{

  constructor( private contactUsClient:ContactUsClient) {
    super();

   }
/**
 * get All Contact Us Details
 * @returns
 */
   async getAllcontactUsData ():Promise<ApiResponse<ContactUsSM[]>> {
    return this.contactUsClient.GetContactUsData()
  }
    /**
* Get Contact Us Details by Selected Contact
* @param companyId
* @returns
*/
async getcontactUsById(
  Id: number
): Promise<ApiResponse<ContactUsSM>> {
  return await this.contactUsClient.GetContactUsById(Id);
}

   /** Add Contact Us Details
 * @return Success
  */
   async addNewcontactUsDetails(user:ContactUsSM): Promise<ApiResponse<ContactUsSM>> {
    let apiRequest = new ApiRequest<ContactUsSM>();
    apiRequest.reqData =user;
    return await this.contactUsClient.AddNewContactUsDetails(apiRequest);
  }

   /**Delete Contact Us Details
 * @DEV : Musaib
*/
async DeletecontactUs(
  id: number
): Promise<ApiResponse<DeleteResponseRoot>> {
  if (id <= 0) {
    throw new Error(AppConstants.ErrorPrompts.Delete_Data_Error);
  }
  return await this.contactUsClient.DeleteContactUsById(
    id
  );
}
}
