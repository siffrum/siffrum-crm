import { RoleTypeSM } from "../service-models/app/enums/role-type-s-m.enum";
import { ClientCompanyDetailSM } from "../service-models/app/v1/client/client-company-detail-s-m";
import { PermissionAdminEmployeeSM } from "../service-models/app/v1/client/permission-admin-employee-s-m";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class ModulePermissionsViewModel extends BaseViewModel {
  override PageTitle: string = "Module-Permissions";
  company: ClientCompanyDetailSM = new ClientCompanyDetailSM();
  initialAddModePermissionCompanyDetailId: number = 0;
  listRoles = new Array<string>();
  selectedRole!: RoleTypeSM;
  adminModulePermissionslist: PermissionSM[] = [];
  EmployeeModulePermissionslist: PermissionSM[] = [];
  modulePermissionList: PermissionSM[] = [];
  modulePermissions: PermissionSM[] = [];
  adminPermission: boolean = false;
  employeePermission: boolean = false;
  adminCheckboxChecked: boolean = false;
  employeeCheckboxChecked: boolean = false;
  tableContent: boolean = false;
  selectAll: boolean = false;
  isChecked: boolean = false;
  saveSelected: boolean = false;
  switchToAddAdminBtn: boolean = true;
}
