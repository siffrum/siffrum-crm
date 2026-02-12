import { Injectable } from '@angular/core';
import *  as CryptoJS from 'crypto-js';
import { environment } from 'src/environments/environment';
import { QueryFilter } from '../service-models/foundation/api-contracts/query-filter';

@Injectable({
  providedIn: 'root'
})
export class BaseService {

  constructor() {
  }

  createQueryFilterObject(pgNo: number, pgSize: number): QueryFilter {
    let queryFilter = new QueryFilter();
    queryFilter.top = pgSize;
    queryFilter.skip = (pgNo - 1) * pgSize;
    return queryFilter;
  }

  encrypt(txt: string): string {
    return CryptoJS.AES.encrypt(txt, environment.encryptionKey).toString();
  }

  decrypt(txtToDecrypt: string) {
    return CryptoJS.AES.decrypt(txtToDecrypt, environment.encryptionKey).toString(CryptoJS.enc.Utf8);
  }

  convertToUtc(dateOfBirth: any): any {
   return new Date(dateOfBirth).toISOString(); 
  }
}
