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

  // keep if you still use on dashboard page
  dashboardItems: {
    itemRoute: string;
    permission: boolean;
    imgSrc: string;
    altText: string;
    itemName: string;
    moduleName: ModuleNameSM;
  }[] = [
    {
      itemRoute: AppConstants.WebRoutes.PROFILE,
      permission: false,
      imgSrc: "assets/images/directory2.webp",
      altText: "Image Not Found",
      itemName: "Profile",
      moduleName: ModuleNameSM.Employee,
    },
    {
      itemRoute: AppConstants.WebRoutes.SETTINGS,
      permission: false,
      imgSrc: "assets/images/setting.webp",
      altText: "Image Not Found",
      itemName: "Settings",
      moduleName: ModuleNameSM.Setting,
    },
    {
      itemRoute: AppConstants.WebRoutes.ATTENDANCE,
      permission: false,
      imgSrc: "assets/images/attendance.webp",
      altText: "Image Not Found",
      itemName: "Attendace",
      moduleName: ModuleNameSM.Attendance,
    },
  ];

  /**
 * ✅ SIDEBAR SHOULD SHOW ONLY:
 * Dashboard, CRM, Reports, Notifications, Profile, Settings
 */
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
    moduleName: ModuleNameSM.DashBoard,
  },

  {
    itemRoute: AppConstants.WebRoutes.INTERNAL,
    permission: true,
    iconName: "bi bi-grid sideNavIcon d-inline-block",
    itemName: "CRM",
    moduleName: ModuleNameSM.DashBoard,
  },

  {
    itemRoute: "",
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
      },
    ],
  },

  {
    itemRoute: AppConstants.WebRoutes.DASHBOARD,
    permission: false,
    iconName: "bi bi-bell sideNavIcon d-inline-block",
    itemName: "Notifications",
    moduleName: ModuleNameSM.DashBoard,
  },

  {
    itemRoute: AppConstants.WebRoutes.PROFILE,
    permission: false,
    iconName: "bi bi-person-check sideNavIcon d-inline-block",
    itemName: "Profile",
    moduleName: ModuleNameSM.Employee,
  },

  {
    itemRoute: AppConstants.WebRoutes.SETTINGS,
    permission: false,
    iconName: "bi bi-gear sideNavIcon d-inline-block",
    itemName: "Settings",
    moduleName: ModuleNameSM.Setting,
  },
];
  /**
   * ✅ Super admin menu unchanged
   */
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
      iconName: "bi bi-grid-1x2-fill sideNavIcon d-inline-block",
      itemName: "Overview",
      module: ModuleNameSM.CompanyDetail.toString(),
    },
    {
      itemRoute: AppConstants.WebRoutes.ADMIN.ADD_COMPANY,
      isActive: true,
      iconName: "bi bi-building-add sideNavIcon d-inline-block",
      itemName: "Add Company",
      module: ModuleNameSM.CompanyDetail.toString(),
    },
    {
      itemRoute: AppConstants.WebRoutes.ADMIN.COMPANIES,
      isActive: true,
      iconName: "bi bi-buildings sideNavIcon d-inline-block",
      itemName: "Companies",
      module: ModuleNameSM.CompanyDetail.toString(),
    },
    {
      itemRoute: AppConstants.WebRoutes.ADMIN.SQL,
      isActive: true,
      iconName: "bi bi-bar-chart-line sideNavIcon d-inline-block",
      itemName: "SQL Reports",
      module: ModuleNameSM.CompanyDetail.toString(),
    },
    {
      itemRoute: AppConstants.WebRoutes.ADMIN.CONTACT_US,
      isActive: true,
      iconName: "bi bi-chat-left-text sideNavIcon d-inline-block",
      itemName: "Contact Inbox",
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
