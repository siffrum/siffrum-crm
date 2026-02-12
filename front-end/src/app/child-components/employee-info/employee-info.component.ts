import { DatePipe } from "@angular/common";
import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
} from "@angular/core";
import { NgForm, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { BaseComponent } from "src/app/components/base.component";
import { EmployeeStatusSM } from "src/app/service-models/app/enums/employee-status-s-m.enum";
import { LoginStatusSM } from "src/app/service-models/app/enums/login-status-s-m.enum";
import { ModuleNameSM } from "src/app/service-models/app/enums/module-name-s-m.enum";
import { RoleTypeSM } from "src/app/service-models/app/enums/role-type-s-m.enum";
import { ClientUserSM } from "src/app/service-models/app/v1/app-users/client-user-s-m";
import { PermissionSM } from "src/app/service-models/app/v1/client/permission-s-m";
import { AccountService } from "src/app/services/account.service";
import { CommonService } from "src/app/services/common.service";
import { CompanyAttendanceShiftService } from "src/app/services/company-attendance-shift.service";
import { CompanyDepartmentService } from "src/app/services/company-departments.service";
import { EmployeeService } from "src/app/services/employee.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { EmployeeInfoViewModel } from "src/app/view-models/employee-info.viewmodel";

@Component({
    selector: "app-employee-info",
    templateUrl: "./employee-info.component.html",
    styleUrls: ["./employee-info.component.scss"],
    standalone: false
})
export class EmployeeInfoComponent
  extends BaseComponent<EmployeeInfoViewModel>
  implements OnInit {
  @Input() employee: ClientUserSM = new ClientUserSM();
  @Input() isReadOnly!: boolean;
  @Input() isaddmode!: boolean;
  @Output() newItemEvent = new EventEmitter<ClientUserSM>();
  roleType!: string;
  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private employeeService: EmployeeService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private accountService: AccountService,
    private companyAttendanceShiftService: CompanyAttendanceShiftService,
    private companyDepartmentService: CompanyDepartmentService,
  ) {
    super(commonService, logService);
    this.viewModel = new EmployeeInfoViewModel();
  }
  async ngOnInit() {
    await this.getAttendanceShift();
    await this.getCompanyDeparments();
    //check role Type
    this.roleType = RoleTypeSM[this._commonService.layoutVM.tokenRole];
    let Id = Number(this.activatedRoute.snapshot.paramMap.get("Id"));
    this.viewModel.empId = Id;
    if (Id == undefined) {
      await this._commonService.ShowToastAtTopEnd(
        "Something Went Wrong",
        "error"
      );
    } else if ((!this.isaddmode && Id == 0) || Id == undefined) {
      this.isReadOnly = true;
      this.viewModel.isDisabled = true;
      await this.loadPageData();
    } else if (Id > 0) {
      await this.loadPageDataWithId(Id);
    } else {
      !this.viewModel.isDisabled;
    }
    this.getPermissions();
  }

  sendToParent(employee: ClientUserSM) {
    this.newItemEvent.emit(employee);
  }
  //check For Action permissions
  async getPermissions() {
    let ModuleName = ModuleNameSM.Employee;
    let resp: PermissionSM | any =
      await this.accountService.getMyModulePermissions(ModuleName);
    this.viewModel.Permission = resp;
  }
  /**
   * Gett All Employees of the Company
   */
  override async loadPageData() {
    try {
      await this._commonService.presentLoading();
      let resp = await this.employeeService.getEmployeeByMineEndpoint();
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.employee = resp.successData;
        this.viewModel.employee.dateOfBirth=await this.getFormattedDate(this.viewModel.employee.dateOfBirth,false)
        this.viewModel.employee.dateOfJoining=await this.getFormattedDate(this.viewModel.employee.dateOfJoining,false)
        await this.getAttendanceShift();
        await this.getCompanyDeparments();
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Get attendanceShiftList
   */
  async getAttendanceShift() {
    try {
      await this._commonService.presentLoading();
      let resp =
        await this.companyAttendanceShiftService.getAllCompanyAttendanceShiftDetails();
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.attendanceShiftList = resp.successData;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Select shifts From attendanceShiftList
   */
  selectedshift() {
    const selectedShiftName = this.viewModel.selectedShift;
    const selectedShift = this.viewModel.attendanceShiftList.find(
      (item) => item.shiftName === selectedShiftName
    );

    if (selectedShift) {
      this.viewModel.employee.clientCompanyAttendanceShiftId = selectedShift.id;
      // You can do further processing with the selectedShiftId here
    }
  }
  /**
   * GET compamy Departments
   */
  async getCompanyDeparments() {
    try {
      await this._commonService.presentLoading();
      let resp = await this.companyDepartmentService.getAllCompanyDepartments();
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.departmentList = resp.successData;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Select Department From DepartmentList
   */
  selectedCompanyDepartment() {
    const selectedDepartmentName = this.viewModel.selectedDepartment;
    const selectedDepartment = this.viewModel.departmentList.find(
      (item) => item.departmentName === selectedDepartmentName
    );

    if (selectedDepartment) {
      this.viewModel.employee.clientCompanyDepartmentId = selectedDepartment.id;
      this.viewModel.employee.department = this.viewModel.selectedDepartment;
      // You can do further processing with the selectedShiftId here
    }
  }
  /**
   * loadPage data
   * @param employeeId
   */
  async loadPageDataWithId(employeeId: number) {
    try {
      await this._commonService.presentLoading();
      let resp = await this.employeeService.getEmployeeByEmployeeId(employeeId);
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.employee = resp.successData;
        this.viewModel.isDisabled = true;
        this.viewModel.showButton = this.viewModel.showButton;
        this.viewModel.employee.dateOfJoining = await this.getFormattedDate(this.viewModel.employee.dateOfJoining, false);
        this.viewModel.employee.dateOfBirth = await this.getFormattedDate(this.viewModel.employee.dateOfBirth, false);
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }


  /**
   * Generate Unique Employee  Code
   */
  generateEmployeeCode(): void {
    // Generate a random 6-digit number
    const random6DigitNumber = Math.floor(100000 + Math.random() * 900000);

    this.viewModel.employee.employeeCode = `${random6DigitNumber}`;
  }
  /**
   * And Validate if Code Exceeds more than or less than 6 digits
   * @param event
   */
  validateEmployeeCode(event: any): void {
    // Ensure the input value is exactly 6 digits
    const inputValue = event.target.value;
    if (inputValue.length !== 6 || isNaN(Number(inputValue))) {
      // If the input is not a 6-digit number, reset it to the generated code
      this.generateEmployeeCode();
    } else {
      // Set the validated input value
      this.viewModel.employee.employeeCode = `${inputValue}`;
    }
  }
  /**
   * Add new employee to the Company
   */

  async createEmployee(employeeInfoForm: NgForm) {
    this.viewModel.formSubmitted = true;
    try {
      if (employeeInfoForm.invalid) {
        await this._commonService.showSweetAlertToast({
          title: "Form Fields Are Not valid !",
          icon: "error",
        });
        return; // Stop execution here if form is not valid
      }
      await this._commonService.presentLoading();
      this.viewModel.employee.loginId =
        this.viewModel.employee.firstName + this.viewModel.employee.lastName;
      this.viewModel.employee.passwordHash =
        this.viewModel.employee.lastName + this.viewModel.employee.firstName;
      this.viewModel.employee.loginStatus = LoginStatusSM.Enabled;
      this.viewModel.employee.employeeStatus = EmployeeStatusSM.Active;
      this.viewModel.employee.roleType = RoleTypeSM.ClientEmployee;
      this.viewModel.employee.isEmailConfirmed = true;
      this.viewModel.employee.isPhoneNumberConfirmed = true;
      let resp = await this.employeeService.addEmployee(
        this.viewModel.employee
      );
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this._commonService.showInfoOnAlertWindowPopup(
          "success",
          "Employee Added Successfully Please Note the Login Credentials for this User:",
          `Username: ${this.viewModel.employee.loginId}
          Password: ${this.viewModel.employee.passwordHash}`
        );
        this.viewModel.employee = resp.successData;
        this.sendToParent(resp.successData);
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**
   * Add  New Employee and close page
   */
  async createEmployeeAndClose(employeeInfoForm: NgForm) {
    this.viewModel.formSubmitted = true;
    try {
      if (employeeInfoForm.invalid) {
        await this._commonService.showSweetAlertToast({
          title: "Form Fields Are Not valid !",
          icon: "error",
        });
        return; // Stop execution here if form is not valid
      }
      await this._commonService.presentLoading();
      if (
        this.viewModel.employee.lastName == "" ||
        this.viewModel.employee.lastName == undefined
      ) {
        this.viewModel.employee.lastName = "";
      }
      this.viewModel.employee.loginId =
        this.viewModel.employee.firstName + this.viewModel.employee.lastName;
      this.viewModel.employee.passwordHash =
        this.viewModel.employee.lastName + this.viewModel.employee.firstName;
      this.viewModel.employee.loginStatus = LoginStatusSM.Enabled;
      this.viewModel.employee.employeeStatus = EmployeeStatusSM.Active;
      this.viewModel.employee.roleType = RoleTypeSM.ClientEmployee;
      this.viewModel.employee.isEmailConfirmed = true;
      this.viewModel.employee.isPhoneNumberConfirmed = true;
      let resp = await this.employeeService.addEmployee(
        this.viewModel.employee
      );
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this._commonService.showInfoOnAlertWindowPopup(
          "success",
          "Employee Added Successfully Please Note the Login Credentials for this User:",
          `Username: ${this.viewModel.employee.loginId}
          Password: ${this.viewModel.employee.passwordHash}`
        );
        this.viewModel.employee = resp.successData;
        this.router.navigate(["/employees-list"]);
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  /**
   * Update employee Info
   */
  async updateEmployeeInfo(employeeInfoForm: NgForm) {
    this.viewModel.formSubmitted = true;
    try {
      if (employeeInfoForm.invalid) {
        await this._commonService.showSweetAlertToast({
          title: "Form Fields Are Not valid !",
          icon: "error",
        });
        return; // Stop execution here if form is not valid
      }
      await this._commonService.presentLoading();
      let resp = await this.employeeService.updateEmployeeInfo(
        this.viewModel.employee
      );
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this._commonService.ShowToastAtTopEnd(
          "Employee Details Updated Successfully",
          "success"
        );
        this.viewModel.employee = resp.successData;
        this.viewModel.isDisabled = true;
        this.viewModel.showButton = true;
        this.viewModel.employee.dateOfBirth=await this.getFormattedDate(this.viewModel.employee.dateOfBirth,false)
        this.viewModel.employee.dateOfJoining=await this.getFormattedDate(this.viewModel.employee.dateOfJoining,false)
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }


  async checkDate(value: Date) {
    return;
  }
  /**
   * Enable Disable Edit Mode
   */
  editDetails() {
    this.isReadOnly = !this.isReadOnly;
    this.viewModel.isDisabled = !this.viewModel.isDisabled;
    this.viewModel.showButton = !this.viewModel.showButton;
  }
}
