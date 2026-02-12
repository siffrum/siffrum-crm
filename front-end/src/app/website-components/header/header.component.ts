import { Component, ElementRef, OnInit, Renderer2 } from "@angular/core";
import { Router } from "@angular/router";
import { BaseComponent } from "src/app/components/base.component";
import { AuthGuard } from "src/app/guard/auth.guard";
import { RoleTypeSM } from "src/app/service-models/app/enums/role-type-s-m.enum";
import { CommonService } from "src/app/services/common.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { WebsiteViewModel } from "src/app/view-models/website.viewmodel";

@Component({
    selector: "app-header",
    templateUrl: "./header.component.html",
    styleUrls: ["./header.component.scss"],
    standalone: false
})
export class HeaderComponent
  extends BaseComponent<WebsiteViewModel>
  implements OnInit
{
  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private el: ElementRef,
    private renderer: Renderer2,
    private authGuard:AuthGuard,
    private router:Router
  ) {
    super(commonService, logService);
  }

  async ngOnInit() {
    await this.navigateToSection("");
  }

  toggleMobileNav() {
    const navbar = this.el.nativeElement.querySelector("#navbar");
    const mobileNavToggle =
      this.el.nativeElement.querySelector(".mobile-nav-toggle");

    if (navbar) {
      if (mobileNavToggle.classList.contains("bi-list")) {
        this.renderer.addClass(navbar, "navbar-mobile");
        mobileNavToggle.classList.remove("bi-list");
        mobileNavToggle.classList.add("bi-x");
      } else {
        this.renderer.removeClass(navbar, "navbar-mobile");
        mobileNavToggle.classList.remove("bi-x");
        mobileNavToggle.classList.add("bi-list");
      }
    }
  }

  scrollToSection(target: string) {
    const element = document.getElementById(target);
    if (element) {
      element.scrollIntoView({ behavior: "smooth" });
    }
  }

  public navigateToSection(hashValue: string) {
    switch (hashValue) {
      case "#about":
      case "#contact":
      case "#pricing":
      case "#home":
        window.location.assign("/website" + hashValue);
        break;
      default:
    }
  }
  async checkToken() {
    if (await this.authGuard.IsTokenValid()) {
      let roleType: any = RoleTypeSM[this._commonService.layoutVM.tokenRole];
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
