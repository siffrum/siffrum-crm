import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { NotFoundComponent } from "./components/not-found/not-found.component";
import { LogHandlerService } from "./services/log-handler.service";
import { SpinnerComponent } from "./internal-components/spinner/spinner.component";
import { LoginComponent } from "./components/login/login.component";
import { DashboardComponent } from "./components/dashboard/dashboard.component";
import { SideMenuComponent } from "./internal-components/side-menu/side-menu.component";
import { EmployeesListComponent } from "./components/employees-list/employees-list.component";
import { CompanyOverviewComponent } from "./components/company-overview/company-overview.component";
import { EmployeeInfoComponent } from "./child-components/employee-info/employee-info.component";
import { EmployeeAddressInfoComponent } from "./child-components/employee-address-info/employee-address-info.component";
import { EmployeeDocumentInfoComponent } from "./child-components/employee-document-info/employee-document-info.component";
import { EmployeeBankInfoComponent } from "./child-components/employee-bank-info/employee-bank-info.component";
import { EmployeeSalaryInfoComponent } from "./child-components/employee-salary-info/employee-salary-info.component";
import { EmployeeLeavesInfoComponent } from "./child-components/employee-leaves-info/employee-leaves-info.component";
import { EmployeeGenerateLetterInfoComponent } from "./child-components/employee-generate-letter-info/employee-generate-letter-info.component";
import { EmployeeProfileComponent } from "./components/employee-profile/employee-profile.component";
import { AddEmployeeComponent } from "./components/add-employee/add-employee.component";
import { PayrollStructureComponent } from "./components/payroll-structure/payroll-structure.component";
import { DatePipe, DecimalPipe } from "@angular/common";
import { LeavesComponent } from './components/leaves/leaves.component';
import { JwtHelperService, JWT_OPTIONS } from "@auth0/angular-jwt";
import { TransactionsComponent } from './components/transactions/transactions.component';
import { LeaveReportsComponent } from './components/reports/leave-reports/leave-reports.component';
import { CompanyLettersComponent } from "./components/company-letters/company-letters.component";
import { TopNavComponent } from './internal-components/top-nav/top-nav.component';
import { ErrorHandler, NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxSpinnerModule } from "ngx-spinner";
import { AdminLoginComponent } from './components/super/admin-login/admin-login.component';
import { EmployeeAttendanceComponent } from './components/employee-attendance/employee-attendance.component';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { CompanyListComponent } from "./components/super/company-list/company-list.component";
import { AdminDashboardComponent } from "./components/super/admin-dashboard/admin-dashboard.component";
import { SuperCompanyOverviewComponent } from "./components/super/super-company-overview/super-company-overview.component";
import { AddCompanyComponent } from "./components/super/add-company/add-company.component";
import { CompanyAttendanceShiftComponent } from './components/company-attendance-shift/company-attendance-shift.component';
import { PayrollReportsComponent } from './components/reports/payroll-reports/payroll-reports.component';
import { AddCompanyAdminComponent } from './components/super/add-company-admin/add-company-admin.component';
import { ForgotpasswordComponent } from './components/password-configuration/forgotpassword/forgotpassword.component';
import { ResetpasswordComponent } from './components/password-configuration/resetpassword/resetpassword.component';
import { ModulePermissionsComponent } from "./components/super/module-permissions/company-module-permissions.component";
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { SettingComponent } from './components/setting/setting.component';
import { CompanyDepartmentsComponent } from './components/company-departments/company-departments.component';
import { AttendanceReportComponent } from './components/reports/attendance-report/attendance-report.component';
import { SqlReportComponent } from './components/super/sql-report/sql-report.component';
import { DynamicSqlReportsComponent } from './components/reports/dynamic-sql-reports/dynamic-sql-reports.component';
import { RegisterComponent } from './website-components/register/register.component';
import { LicenseInfoComponent } from './components/license-info/license-info.component';
import { SuccessPaymentComponent } from './components/license-info/success-payment/success-payment.component';
import { FailurePaymentComponent } from './components/license-info/failure-payment/failure-payment.component';
import { ContactUsComponent } from './components/super/contact-us/contact-us.component';
import { PaginationComponent } from "./internal-components/pagination/pagination.component";


@NgModule({
  declarations: [
    AppComponent,
    NotFoundComponent,
    SpinnerComponent,
    LoginComponent,
    DashboardComponent,
    SideMenuComponent,
    EmployeesListComponent,
    CompanyOverviewComponent,
    EmployeeInfoComponent,
    EmployeeAddressInfoComponent,
    EmployeeDocumentInfoComponent,
    EmployeeBankInfoComponent,
    EmployeeSalaryInfoComponent,
    EmployeeLeavesInfoComponent,
    EmployeeGenerateLetterInfoComponent,
    EmployeeProfileComponent,
    AddEmployeeComponent,
    PayrollStructureComponent,
    LeavesComponent,
    TransactionsComponent,
    LeaveReportsComponent,
    CompanyLettersComponent,
    TopNavComponent,
    AdminLoginComponent,
    EmployeeAttendanceComponent,
    CompanyListComponent,
    AdminDashboardComponent,
    SuperCompanyOverviewComponent,
    AddCompanyComponent,
    CompanyAttendanceShiftComponent,
    PayrollReportsComponent,
    AddCompanyAdminComponent,
    ForgotpasswordComponent,
    ResetpasswordComponent,
    ModulePermissionsComponent,
    ChangePasswordComponent,
    SettingComponent,
    CompanyDepartmentsComponent,
    AttendanceReportComponent,
    SqlReportComponent,
    DynamicSqlReportsComponent,
    RegisterComponent,
    LicenseInfoComponent,
    SuccessPaymentComponent,
    FailurePaymentComponent,
    ContactUsComponent,

  ],
  imports: [
    FormsModule,
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    NgxSpinnerModule.forRoot({ type: 'ball-elastic-dots' }),
    CalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory,
    }),
    PaginationComponent,
  ],
  providers: [
    { provide: ErrorHandler, useClass: LogHandlerService },
    DecimalPipe,
    JwtHelperService,
    { provide: JWT_OPTIONS, useValue: JWT_OPTIONS },
    DatePipe,
  ],

  bootstrap: [AppComponent],
  exports: [],
})
export class AppModule { }
