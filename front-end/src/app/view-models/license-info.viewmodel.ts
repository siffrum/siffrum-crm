import { CompanyLicenseDetailsSM } from "../service-models/app/v1/license/company-license-details-s-m";
import { LicenseTypeSM } from "../service-models/app/v1/license/license-type-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class LicenseViewModel extends BaseViewModel {
  override  PageTitle: string = "Pricing";
  // LicenseType!: LicenseTypeSM;
  LicenseTypeObj: LicenseTypeSM = new LicenseTypeSM();
  LicenseTypeList: LicenseTypeSM[] = [];
  buyBtn: string = 'Buy Now';
  purchasedBtn: string = 'Purchased';
  paymentInfo!: PaymentStatusInfo;
  emailId: string = 'info@siffrum.com'
  activeLicense: CompanyLicenseDetailsSM = new CompanyLicenseDetailsSM();
  checkActiveLicense!: boolean;
  checkValidUser!: boolean;
}

export interface PaymentStatusInfo {
  licenseId: number;
  paymentDate: Date;
}
