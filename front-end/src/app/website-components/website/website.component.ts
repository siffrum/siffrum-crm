import { Component, ElementRef, OnInit } from "@angular/core";
import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { BaseComponent } from "src/app/components/base.component";
import { AuthGuard } from "src/app/guard/auth.guard";
import { RoleTypeSM } from "src/app/service-models/app/enums/role-type-s-m.enum";
import { ContactUsSM } from "src/app/service-models/app/v1/general/contact-us-s-m";
import { CommonService } from "src/app/services/common.service";
import { ContactUsService } from "src/app/services/contact-us.service";
import { LicenseInfoService } from "src/app/services/license-info.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { WebsiteViewModel } from "src/app/view-models/website.viewmodel";
@Component({
    selector: "app-website",
    templateUrl: "./website.component.html",
    styleUrls: ["./website.component.scss"],
    standalone: false
})
export class WebsiteComponent
  extends BaseComponent<WebsiteViewModel>
  implements OnInit
{
  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private el: ElementRef,
    private contactUsService: ContactUsService,
    private router: Router,
    private authGuard: AuthGuard,
  ) {
    super(commonService, logService);

    this.viewModel = new WebsiteViewModel();
  }
  async ngOnInit() {
    // this.removePreloader();
    this._commonService.layoutVM.toogleWrapper = "loginWrapper";
    this._commonService.layoutVM.showLeftSideMenu = false;

    await this._commonService.presentLoading();
    await setTimeout(async () => {
      await this._commonService.dismissLoader();
    }, 1000);
  }
  Waypoint: any;

  progressBar() {
    const skilsContent = this.el.nativeElement.querySelector(".skills-content");
    if (skilsContent) {
      new this.Waypoint({
        element: skilsContent,
        offset: "80%",
        handler: function () {
          const progressBars = skilsContent.querySelectorAll(".progress-bar");

          progressBars.forEach((el: any) => {
            el.style.width = el.getAttribute("aria-valuenow") + "%";
          });
        },
      });
    }
  }
  /**
   * Send Mesaage
   * @param contactUsForm
   * @returns
   */
  async sendMessage(contactUsForm: NgForm) {
    this.viewModel.formSubmitted = true;
    try {
      if (contactUsForm.invalid) {
        await this._commonService.showSweetAlertToast({
          title: "Form Fields Are Not valid !",
          icon: "error",
        });
        return;
      }
      await this._commonService.presentLoading();
      let resp = await this.contactUsService.addNewcontactUsDetails(
        this.viewModel.contactUsObj
      );
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        this._commonService.showSweetAlertToast({
          title: "Error!",
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error",
        });
      } else {
        this._commonService.ShowToastAtTopEnd(
          "Message Sent Successully",
          "success"
        );
        this.viewModel.displayStyle = "none";
        this.viewModel.contactUsObj = new ContactUsSM();
        this.router.navigate(["/website"]);
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  async checkToken() {
    if (await this.authGuard.IsTokenValid()) {
      let roleType: any = this._commonService.layoutVM.tokenRole;
      if (
        roleType === RoleTypeSM.ClientAdmin ||
        roleType === RoleTypeSM.ClientEmployee
      ) {
        await this.router.navigate(["/dashboard"]);
      }
      else{
        this.router.navigate(['/login'])
      }
    }
    else{
      this.router.navigate(['/login'])
    }
  }
}
