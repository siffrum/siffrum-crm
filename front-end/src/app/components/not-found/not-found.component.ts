import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { RoleTypeSM } from "src/app/service-models/app/enums/role-type-s-m.enum";
import { CommonService } from "src/app/services/common.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { StorageService } from "src/app/services/storage.service";
import { BaseComponent } from "../base.component";
import { DummyTeacherViewModel } from "src/app/view-models/dummy-teacher.viewmodel";
import { AuthGuard } from "src/app/guard/auth.guard";
declare var Swal: any;
@Component({
    selector: "app-not-found",
    templateUrl: "./not-found.component.html",
    styleUrls: ["./not-found.component.scss"],
    standalone: false
})
export class NotFoundComponent extends BaseComponent<DummyTeacherViewModel> implements OnInit {
  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private router: Router,
    private authguard: AuthGuard
  ) {
    super(commonService, logService)
  }

  async ngOnInit() {
    let x = await this.authguard.GetRoleFromToken();




    Swal.fire({
      icon: "warning",
      title: "This Page does Not Exist",
      showCancelButton: false,
      confirmButtonText: "OK",
    }).then((result: any) => {

      if (result.isConfirmed) {
        if (x === RoleTypeSM.SuperAdmin) {

          this.router.navigate(["/admin/dashboard"]);
        }
        else if (x === RoleTypeSM.ClientAdmin || RoleTypeSM.ClientEmployee) {
          this.router.navigate(["/"]);
        }
      } // Replace '/` with your default route path
    });
  }
}
