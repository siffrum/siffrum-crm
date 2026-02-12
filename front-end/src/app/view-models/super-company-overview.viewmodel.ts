import { ClientUserSM } from "../service-models/app/v1/app-users/client-user-s-m";
import { ClientCompanyAddressSM } from "../service-models/app/v1/client/client-company-address-s-m";
// import { ClientCompanyAttendanceShiftSM } from "../service-models/app/v1/client/client-company-attendance-shift-s-m";
import { ClientCompanyDetailSM } from "../service-models/app/v1/client/client-company-detail-s-m";
import { CompanyModulesSM } from "../service-models/app/v1/client/company-modules-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class SuperCompanyOverviewViewModel extends BaseViewModel {
  override PageTitle: string = "Super-Company-Overview";
  showTooltip:boolean=false;
  // companyDetailList: ClientCompanyDetailSM[] = [];
  companyUserList: ClientUserSM[] = [];
  // companyUser: ClientUserSM = new ClientUserSM();
  company: ClientCompanyDetailSM = new ClientCompanyDetailSM();
  companyAddress: ClientCompanyAddressSM = new ClientCompanyAddressSM();
  companyModulesDetail: CompanyModulesSM = new CompanyModulesSM();
  companyModulesDetailList: CompanyModulesSM[] = [];
  // companyAttendanceShiftDetailsList:ClientCompanyAttendanceShiftSM[]=[]
  // companyAttendanceShiftDetails:ClientCompanyAttendanceShiftSM=new ClientCompanyAttendanceShiftSM()
  // addAdmin: ClientUserSM = new ClientUserSM();
  isReadonly: boolean = false;
  // tabLocation: SuperCompanyProfileTabs = SuperCompanyProfileTabs.Overview;
  isAddMode:boolean =true;
  isDisabled: boolean = true;
  showCompanyDetailButton: boolean = false;
  showCompanyAddressButton: boolean = false;
  // selectAll: boolean = false;
  FormSubmitted: boolean = false;
  disabledDate!: string;
  maxDate = new Date();
  selectedDate: Date = new Date();
  // totalCount!: number;
  // showButton: boolean = true;
  editMode: boolean = false;
  // displayStyle = "none";
}

export enum SuperCompanyProfileTabs {
  Overview = 0,
  Address = 1,
  Module = 2,
  Users = 3,
  AddUser = 4,
}
