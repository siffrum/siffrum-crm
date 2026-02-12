import { Injectable } from '@angular/core';
import { AppConstants } from 'src/app-constants';
import { AccountsClient } from '../clients/accounts.client';
import { RoleTypeSM } from '../service-models/app/enums/role-type-s-m.enum';
import { TokenRequestSM } from '../service-models/app/token/token-request-s-m';
import { TokenResponseSM } from '../service-models/app/token/token-response-s-m';
import { ApiRequest } from '../service-models/foundation/api-contracts/base/api-request';
import { ApiResponse } from '../service-models/foundation/api-contracts/base/api-response';
import { BaseService } from './base.service';

@Injectable({
  providedIn: 'root'
})
export class SampleService extends BaseService {

  constructor(private accountClient: AccountsClient) {
    super();
  }

  async generateToken(tokenReq: TokenRequestSM): Promise<ApiResponse<TokenResponseSM>> {
    if (tokenReq == null || tokenReq.loginId == null)// null checks
    {
      throw new Error(AppConstants.ErrorPrompts.Invalid_Input_Data);
    }
    else {
      let apiRequest = new ApiRequest<TokenRequestSM>();
      tokenReq.companyCode = '123';
      tokenReq.roleType = RoleTypeSM.ClientAdmin;
      apiRequest.reqData = tokenReq
      return await this.accountClient.GenerateToken(apiRequest);
    }
  }
}
