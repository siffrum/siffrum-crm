import { AppConstants } from "src/app-constants";
import { RoleTypeSM } from "../service-models/app/enums/role-type-s-m.enum";
import { ClientCompanyDetailSM } from "../service-models/app/v1/client/client-company-detail-s-m";
import { CompanyModulesSM } from "../service-models/app/v1/client/company-modules-s-m";
import { ModuleNameSM } from "../service-models/app/enums/module-name-s-m.enum";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
import { SideMenuViewModel } from "../view-models/side-menu.viewmodel";
import { ClientThemeSM } from "../service-models/app/v1/client/client-theme-s-m";

export interface ToastInfo {
  className?: string;
  header?: string;
  body: string;
  delay: number;
  animation?: boolean;
}

export interface LoaderInfo {
  message: string;
  showLoader: boolean;
}

export interface ConfirmModalInfo {
  title: string;
  subTitle: string;
  message: string;
  showModal: boolean;
}

export class LayoutViewModel {
  viewModel: SideMenuViewModel = new SideMenuViewModel();
  currentRoute: string = "";
  tokenRole: RoleTypeSM = RoleTypeSM.Unknown;
  showLeftSideMenu: boolean = false;
  clientTheme: ClientThemeSM = new ClientThemeSM();
  toggleSideMenu = "default";
  toogleWrapper = "wrapper";
  PageTitle!: string;
  loggedUserName!: string;
  company: ClientCompanyDetailSM = new ClientCompanyDetailSM();
  isAddMode: boolean = false;
  wizardLocation: AddCompanyWizards = AddCompanyWizards.addCompanyInfo;
  modulePermissionList: CompanyModulesSM[] = [];
  showModule: boolean = false;
  loginUser: string = "log";
  permissions: PermissionSM[] = [];
  permission: PermissionSM = new PermissionSM();

  dashboardItems:{
    itemRoute: string;
    permission: boolean;
    imgSrc: string;
    altText:string;
    itemName: string;
    moduleName: ModuleNameSM;
  }[]=[
    {
      itemRoute: AppConstants.WebRoutes.PROFILE,
      permission: false,
      imgSrc: "assets/images/directory2.webp",
      altText:"Image Not Found",
      itemName: "Profile",
      moduleName: ModuleNameSM.Employee,
    },
    {
      itemRoute: AppConstants.WebRoutes.SETTINGS,
      permission: false,
      imgSrc: "assets/images/setting.webp",
      altText:"Image Not Found",
      itemName: "Settings",
      moduleName: ModuleNameSM.Setting,
    },
    {
      itemRoute: AppConstants.WebRoutes.ATTENDANCE,
      permission: false,
      imgSrc: "assets/images/attendance.webp",
      altText:"Image Not Found",
      itemName: "Attendace",
      moduleName: ModuleNameSM.Attendance,
    }

  ]
  sideMenuItems: {
    itemRoute: string;
    permission: boolean;
    iconName: string;
    itemName: string;
    moduleName: ModuleNameSM;
    subItems?: {
      itemRoute: string;
      permission: boolean;
      iconName: string;
      itemName: string;
      moduleName: ModuleNameSM;
    }[];
    showSubItems?: boolean;
  }[] = [
    {
      itemRoute: AppConstants.WebRoutes.DASHBOARD,
      permission: false,
      iconName: "bi bi-house sideNavIcon d-inline-block",
      itemName: "Dashboard",
      moduleName: ModuleNameSM.CompanyDetail,
    },

    {
      itemRoute: AppConstants.WebRoutes.COMPANYOVERVIEW,
      permission: false,
      iconName: "bi bi-building sideNavIcon d-inline-block",
      itemName: "Overview",
      moduleName: ModuleNameSM.CompanyDetail,
    },
    {
      itemRoute: AppConstants.WebRoutes.PROFILE,
      permission: false,
      iconName: "bi bi-person-check sideNavIcon d-inline-block",
      itemName: "Profile",
      moduleName: ModuleNameSM.Employee,
    },
    {
      itemRoute: AppConstants.WebRoutes.ATTENDANCE,
      permission: false,
      iconName: "bi bi-person-bounding-box sideNavIcon d-inline-block",
      itemName: "Attendance",
      moduleName: ModuleNameSM.Attendance,
    },
    {
      itemRoute: AppConstants.WebRoutes.LICENSE,
      permission: false,
      iconName: "bi bi-cash-coin sideNavIcon d-inline-block",
      itemName: "Pricing",
      moduleName: ModuleNameSM.CompanyDetail,
    },
    {
      itemRoute: AppConstants.WebRoutes.EMPLOYEELIST,
      permission: false,
      iconName: "bi bi-people sideNavIcon d-inline-block",
      itemName: "Directory",
      moduleName: ModuleNameSM.EmployeeDirectory,
    },
    {
      itemRoute: AppConstants.WebRoutes.COMPANYLETTERS,
      permission: false,
      iconName: "bi bi-card-text sideNavIcon d-inline-block",
      itemName: "Letters",
      moduleName: ModuleNameSM.CompanyLetters,
    },
    {
      itemRoute: AppConstants.WebRoutes.LEAVES,
      permission: false,
      iconName: "bi bi-file-text sideNavIcon d-inline-block",
      itemName: "Leaves",
      moduleName: ModuleNameSM.Leave,
    },


    {
      itemRoute: AppConstants.WebRoutes.DEPARTMENTS,
      permission: false,
      iconName: "bi bi-stack sideNavIcon d-inline-block",
      itemName: "Departments",
      moduleName: ModuleNameSM.CompanyDepartment,
    },
    {
      itemRoute: AppConstants.WebRoutes.ATTENDANSHIFT,
      permission: false,
      iconName: "bi bi-shift sideNavIcon d-inline-block",
      itemName: "Attendance Shift",
      moduleName: ModuleNameSM.AttendanceShift,
    },
    {
      itemRoute: AppConstants.WebRoutes.PAYROLLSTRUCTURE,
      permission: false,
      iconName: "bi bi-wallet2 sideNavIcon d-inline-block",
      itemName: "Payroll Structure",
      moduleName: ModuleNameSM.EmployeeGenericPayroll,
    },
    {
      itemRoute: AppConstants.WebRoutes.TRANSACTIONS,
      permission: false,
      iconName: "bi bi-cash sideNavIcon d-inline-block",
      itemName: "Transactions",
      moduleName: ModuleNameSM.PayrollTransacton,
    },

    {
      itemRoute: '',
      permission: false,
      iconName: "bi bi-file-earmark-person sideNavIcon d-inline-block",
      itemName: "Reports",
      moduleName: ModuleNameSM.Reports,
      subItems: [
        {
          itemRoute: AppConstants.WebRoutes.REPORTS.ATTENDANCE_REPORT,
          permission: false,
          iconName: "bi bi-caret-right sideNavIcon d-inline-block",
          itemName: "Attendance Report",
          moduleName: ModuleNameSM.Reports,
        },
        {
          itemRoute: AppConstants.WebRoutes.REPORTS.LEAVEREPORTS,
          permission: false,
          iconName: "bi bi-caret-right sideNavIcon d-inline-block",
          itemName: "Leave Report",
          moduleName: ModuleNameSM.Reports,
        },
        {
          itemRoute: AppConstants.WebRoutes.REPORTS.PAYROLLREPORTS,
          permission: false,
          iconName: "bi bi-caret-right sideNavIcon d-inline-block",
          itemName: "Payroll Report",
          moduleName: ModuleNameSM.Reports,
        }
      ],
    },
    {
      itemRoute: AppConstants.WebRoutes.SETTINGS,
      permission: false,
      iconName: "bi bi-gear  sideNavIcon d-inline-block",
      itemName: "Settings",
      moduleName: ModuleNameSM.Setting,
    }

  ];
  superAdminSideMenuItems: {
    itemRoute: string;
    isActive: boolean;
    iconName: string;
    itemName: string;
    module: string;
  }[] = [
    {
      itemRoute: AppConstants.WebRoutes.ADMIN.DASHBOARD,
      isActive: true,
      iconName: "bi bi-house sideNavIcon d-inline-block",
      itemName: "Dashboard",
      module: ModuleNameSM.CompanyDetail.toString(),
    },
    {
      itemRoute: AppConstants.WebRoutes.ADMIN.COMPANIES,
      isActive: true,
      iconName: "bi bi-building sideNavIcon d-inline-block",
      itemName: "Companies",
      module: ModuleNameSM.CompanyDetail.toString(),
    },
    {
      itemRoute: AppConstants.WebRoutes.ADMIN.SQL,
      isActive: true,
      iconName: "bi bi-graph-up sideNavIcon d-inline-block",
      itemName: "Sql-Report",
      module: ModuleNameSM.CompanyDetail.toString(),
    },
    {
      itemRoute: AppConstants.WebRoutes.ADMIN.CONTACT_US,
      isActive: true,
      iconName: "bi bi-chat-left-dots sideNavIcon d-inline-block",
      itemName: "Contact",
      module: ModuleNameSM.CompanyDetail.toString(),
    },
  ];
}

export enum AddCompanyWizards {
  addCompanyInfo = 0,
  addCompanyAddress = 1,
  addModules = 2,
  addCompanyAdminDetails = 3,
}

export enum PermissionType {
  none = 0,
  view = 1,
  add = 2,
  edit = 3,
  delete = 4,
}
