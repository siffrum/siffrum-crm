import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AddEmployeeComponent } from "./components/add-employee/add-employee.component";
import { CompanyLettersComponent } from "./components/company-letters/company-letters.component";
import { CompanyOverviewComponent } from "./components/company-overview/company-overview.component";
import { DashboardComponent } from "./components/dashboard/dashboard.component";
import { EmployeeAttendanceComponent } from "./components/employee-attendance/employee-attendance.component";
import { EmployeeProfileComponent } from "./components/employee-profile/employee-profile.component";
import { EmployeesListComponent } from "./components/employees-list/employees-list.component";
import { LeaveReportsComponent } from "./components/reports/leave-reports/leave-reports.component";
import { LeavesComponent } from "./components/leaves/leaves.component";
import { LoginComponent } from "./components/login/login.component";
import { NotFoundComponent } from "./components/not-found/not-found.component";
import { PayrollStructureComponent } from "./components/payroll-structure/payroll-structure.component";
import { AddCompanyComponent } from "./components/super/add-company/add-company.component";
import { AdminDashboardComponent } from "./components/super/admin-dashboard/admin-dashboard.component";
import { AdminLoginComponent } from "./components/super/admin-login/admin-login.component";
import { CompanyListComponent } from "./components/super/company-list/company-list.component";
import { SuperCompanyOverviewComponent } from "./components/super/super-company-overview/super-company-overview.component";
import { TransactionsComponent } from "./components/transactions/transactions.component";
import { AuthGuard } from "./guard/auth.guard";
import { SideMenuComponent } from "./internal-components/side-menu/side-menu.component";
import { RoleTypeSM } from "./service-models/app/enums/role-type-s-m.enum";
import { CompanyAttendanceShiftComponent } from "./components/company-attendance-shift/company-attendance-shift.component";
import { PayrollReportsComponent } from "./components/reports/payroll-reports/payroll-reports.component";
import { ResetpasswordComponent } from "./components/password-configuration/resetpassword/resetpassword.component";
import { ForgotpasswordComponent } from "./components/password-configuration/forgotpassword/forgotpassword.component";
import { ModuleNameSM } from "./service-models/app/enums/module-name-s-m.enum";
import { PermissionType } from "./internal-models/common-models";
import { ChangePasswordComponent } from "./components/change-password/change-password.component";
import { SettingComponent } from "./components/setting/setting.component";
import { CompanyDepartmentsComponent } from "./components/company-departments/company-departments.component";
import { AttendanceReportComponent } from "./components/reports/attendance-report/attendance-report.component";
import { SqlReportComponent } from "./components/super/sql-report/sql-report.component";
import { DynamicSqlReportsComponent } from "./components/reports/dynamic-sql-reports/dynamic-sql-reports.component";
import { RegisterComponent } from "./website-components/register/register.component";
import { LicenseInfoComponent } from "./components/license-info/license-info.component";
import { FailurePaymentComponent } from "./components/license-info/failure-payment/failure-payment.component";
import { SuccessPaymentComponent } from "./components/license-info/success-payment/success-payment.component";
import { ContactUsComponent } from "./components/super/contact-us/contact-us.component";

const routes: Routes = [
  { path: "", redirectTo: "website", pathMatch: "full" },
  
  { path: 'website', loadChildren: () => import('./website-components/website/website.module').then(m => m.WebsiteModule) },

  { path: "login", component: LoginComponent },


  { path: "side-menu", component: SideMenuComponent },

  {
    path: "dashboard",
    component: DashboardComponent,
    canActivate: [AuthGuard],
    data: {
      allowedRole: [RoleTypeSM.ClientAdmin, RoleTypeSM.ClientEmployee],
      moduleName: ModuleNameSM.DashBoard,
      permissionType: [PermissionType.view]

    },
  },
  {path:'failure/:info',component:FailurePaymentComponent},
  {path:'success/:info',component:SuccessPaymentComponent},

  {
    path: "license",
    component: LicenseInfoComponent,
  },
  {
    path: "employees-list",
    component: EmployeesListComponent,
    canActivate: [AuthGuard],
    data: {
      allowedRole: [RoleTypeSM.ClientAdmin, RoleTypeSM.ClientEmployee],
      moduleName: ModuleNameSM.EmployeeDirectory,
      permissionType: [PermissionType.view,PermissionType.edit]
    },
  },

  {
    path: "company-overview",
    component: CompanyOverviewComponent,
    canActivate: [AuthGuard],
    data: {
      allowedRole: [RoleTypeSM.ClientAdmin, RoleTypeSM.ClientEmployee],
      moduleName: ModuleNameSM.CompanyDetail,
      permissionType: [PermissionType.view]
    },
  },

  {
    path: "leaves",
    component: LeavesComponent,
    canActivate: [AuthGuard],
    data: {
      allowedRole: [RoleTypeSM.ClientAdmin, RoleTypeSM.ClientEmployee],
      moduleName: ModuleNameSM.Leave,
      permissionType: [PermissionType.view]

    },
  },

  {
    path: "leaves-report",
    component: LeaveReportsComponent,
    canActivate: [AuthGuard],
    data: {
      allowedRole: [RoleTypeSM.ClientAdmin, RoleTypeSM.ClientEmployee],
      moduleName: ModuleNameSM.Leave,
      permissionType: [PermissionType.view]
    },
  },
  {
    path: "payroll-report",
    component: PayrollReportsComponent,
    canActivate: [AuthGuard],
    data: {
      allowedRole: [RoleTypeSM.ClientAdmin, RoleTypeSM.ClientEmployee],
      moduleName: ModuleNameSM.Reports,
      permissionType: [PermissionType.view]

    },
  },
  {
    path: "attendance-report",
    component: AttendanceReportComponent,
    canActivate: [AuthGuard],
    data: {
      allowedRole: [RoleTypeSM.ClientAdmin, RoleTypeSM.ClientEmployee],
      moduleName: ModuleNameSM.Reports,
      permissionType: [PermissionType.view]

    },
  },
  {
    path: "sql-report",
    component: DynamicSqlReportsComponent,
    canActivate: [AuthGuard],
    data: {
      allowedRole: [RoleTypeSM.ClientAdmin, RoleTypeSM.ClientEmployee],
      moduleName: ModuleNameSM.Reports,
      permissionType: [PermissionType.view]

    },
  },
  {
    path: "company-letter",
    component: CompanyLettersComponent,
    canActivate: [AuthGuard],
    data: {
      allowedRole: [RoleTypeSM.ClientAdmin,RoleTypeSM.ClientEmployee],
      moduleName: ModuleNameSM.CompanyLetters,
      permissionType: [PermissionType.view]

    },
  },

  {
    path: "payroll-structure",
    component: PayrollStructureComponent,
    canActivate: [AuthGuard],
    data: {
      allowedRole: [RoleTypeSM.ClientAdmin,RoleTypeSM.ClientEmployee],
      moduleName: ModuleNameSM.EmployeeGenericPayroll,
      permissionType: [PermissionType.view]

    },
  },

  {
    path: "transactions",
    component: TransactionsComponent,
    canActivate: [AuthGuard],
    data: {
      allowedRole: [RoleTypeSM.ClientAdmin,RoleTypeSM.ClientEmployee],
      moduleName: ModuleNameSM.CompanyAccountTransaction,
      permissionType: [PermissionType.view]

    },
  },

  {
    path: "profile",
    component: EmployeeProfileComponent,
    canActivate: [AuthGuard],
    data: {
      allowedRole: [RoleTypeSM.ClientAdmin, RoleTypeSM.ClientEmployee],
      moduleName: ModuleNameSM.Employee,
      permissionType: [PermissionType.view]

    },
  },

  {
    path: "employee-profile/:Id",
    component: EmployeeProfileComponent,
    canActivate: [AuthGuard],
    data: {
      allowedRole: [RoleTypeSM.ClientAdmin, RoleTypeSM.ClientEmployee],
      moduleName: ModuleNameSM.Employee,
      permissionType: [PermissionType.view]
    },
  },

  {
    path: "add-employee/:Id",
    component: AddEmployeeComponent,
    canActivate: [AuthGuard],
    data: {
      allowedRole: [RoleTypeSM.ClientAdmin,RoleTypeSM.ClientEmployee],
      moduleName: ModuleNameSM.Employee,
      permissionType: [PermissionType.view]

    },
  },
  {
    path: "attendance",
    component: EmployeeAttendanceComponent,
    canActivate: [AuthGuard],
    data: {
      allowedRole: [RoleTypeSM.ClientAdmin, RoleTypeSM.ClientEmployee],
      moduleName: ModuleNameSM.Attendance,
      permissionType: [PermissionType.view]
    },
  },
  {
    path: "shift",
    component: CompanyAttendanceShiftComponent,
    canActivate: [AuthGuard],
    data: {
      allowedRole: [RoleTypeSM.ClientAdmin, RoleTypeSM.ClientEmployee],
      moduleName: ModuleNameSM.Attendance,
      permissionType: [PermissionType.view]

    },
  },
  {
    path: "setting",
    component: SettingComponent,
    canActivate: [AuthGuard],
    data: {
      allowedRole: [RoleTypeSM.ClientAdmin, RoleTypeSM.ClientEmployee],
      moduleName: ModuleNameSM.Setting,
      permissionType: [PermissionType.view]

    }
  },
  {
    path: "departments",
    component:CompanyDepartmentsComponent,
    canActivate: [AuthGuard],
    data: {
      allowedRole: [RoleTypeSM.ClientAdmin, RoleTypeSM.ClientEmployee],
      moduleName: ModuleNameSM.Setting,
      permissionType: [PermissionType.view]

    }
  },
  {path:"register",component:RegisterComponent},
  {
    path: "changePassword",
    component: ChangePasswordComponent,
  },
  {
    path: "ResetPassword",
    component: ResetpasswordComponent,
  },
  {
    path: "forgotPassword",
    component: ForgotpasswordComponent,
  },
  { path: "admin/login", component: AdminLoginComponent },

  {
    path: "admin",
    children: [
      { path: " ", redirectTo: "dashboard", pathMatch: "full" },
      { path: "dashboard", component: AdminDashboardComponent },
      { path: "companylist", component: CompanyListComponent},
      { path: "overview/:Id", component: SuperCompanyOverviewComponent },
      { path: "add-company", component: AddCompanyComponent },
      {path:"sql",component:SqlReportComponent},
      {path:"contact-us",component:ContactUsComponent}
    ],
    canActivate: [AuthGuard],
    data: {
      allowedRole: [RoleTypeSM.SuperAdmin, RoleTypeSM.SystemAdmin],
    },
  },




  { path: "**", pathMatch: "full", component: NotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
