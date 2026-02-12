import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Router } from "@angular/router";
import { JwtHelperService } from "@auth0/angular-jwt";
import { AppConstants } from "src/app-constants";
import decode from "jwt-decode";
import { AccountService } from "../services/account.service";
import { CommonService } from "../services/common.service";
import { RoleTypeSM } from "../service-models/app/enums/role-type-s-m.enum";
import { PermissionType } from "../internal-models/common-models";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
const jwtHelper = new JwtHelperService();

@Injectable({
  providedIn: "root",
})
export class AuthGuard {
  constructor(
    private accountService: AccountService,
    private commonService: CommonService,
    public router: Router
  ) { }

  async canActivate(route: ActivatedRouteSnapshot): Promise<boolean> {
    if (!(await this.IsTokenValid())) {
      await this.accountService.logoutUser();
      await this.router.navigate([AppConstants.WebRoutes.LOGIN]);
      return false;
    }
    let permissionAllowed = false;
    const expectedRole: RoleTypeSM[] = route.data["allowedRole"];
    const permissionTypes: PermissionType[] = route.data["permissionType"];
    const moduleName = route.data["moduleName"];
    let tokenRole: RoleTypeSM = await this.GetRoleFromToken();

    if (!expectedRole.includes(tokenRole)) {
      this.commonService.layoutVM.showLeftSideMenu = false;
      this.router.navigate([AppConstants.WebRoutes.UNAUTHORIZED]);
      return false;
    }
    if (tokenRole == RoleTypeSM.SuperAdmin || tokenRole == RoleTypeSM.SystemAdmin)
      return true;
    // get Module Names
    let per: PermissionSM | any =
      await this.accountService.getMyModulePermissions(moduleName);
    if (per) {
      if (per.isStandAlone) {
        permissionAllowed = per.isEnabledForClient;
      } else {
        permissionTypes.forEach((type) => {
          switch (type) {
            case PermissionType.view:
              permissionAllowed = per.view;
              break;
            default:
              permissionAllowed = per.isEnabledForClient;
              break;
          }
        });
      }
    }
    // let per: PermissionSM | any =
    //   await this.accountService.getMyModulePermissions(moduleName);
    // expectedRole.forEach((role: number) => {
    //   if (role == RoleTypeSM.ClientAdmin || role == RoleTypeSM.ClientEmployee) {
    //     if (per) {
    //       if (per.isStandAlone) {
    //         permissionAllowed = per.isEnabledForClient;
    //       } else {
    //         permissionTypes.forEach((type) => {
    //           switch (type) {
    //             case PermissionType.view:
    //               permissionAllowed = per.view;
    //               break;
    //             default:
    //               permissionAllowed = per.isEnabledForClient;
    //               break;
    //           }
    //         });
    //       }
    //     }
    //     // else {
    //     //   // Handle the case where 'per' is undefined (module permission not available).
    //     //   permissionAllowed = false;
    //     // }
    //   } else {
    //     permissionAllowed = true;
    //   }
    // });
    if (!permissionAllowed) {
      this.router.navigate([AppConstants.WebRoutes.UNAUTHORIZED]);
      return false;
    }
    return true;
  }

  // async canActivateChild(
  //   childRoute: ActivatedRouteSnapshot,
  //   state: RouterStateSnapshot): Promise<boolean> {
  //   const token: string = await this.storageService.getFromStorage(AppConstants.DbKeys.ACCESS_TOKEN);
  //   return !this.jwtHelper.isTokenExpired(token);
  // }

  async IsTokenValid(): Promise<boolean> {
    const token: string = await this.accountService.getTokenFromStorage();
    return token != null && token != "" && !jwtHelper.isTokenExpired(token);
  }

  async IsRoleValidForRoute(expectedRole: any): Promise<boolean> {
    let tokenRole = await this.GetRoleFromToken();
    if (tokenRole != RoleTypeSM.Unknown) {
      this.commonService.layoutVM.showLeftSideMenu = true;
    }

    if (tokenRole === expectedRole) return true;
    return false;
  }

  async GetRoleFromToken(): Promise<RoleTypeSM> {
    let resp: any = RoleTypeSM.Unknown;
    const token: string = await this.accountService.getTokenFromStorage();
    const tokenPayload: any = await decode(token);
    resp = RoleTypeSM[tokenPayload[AppConstants.Token_Info_Keys.Role]];
    this.commonService.layoutVM.tokenRole = resp;
    return resp;
  }
}
