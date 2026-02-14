import {
  Component,
  ElementRef,
  HostListener,
  OnInit,
  Renderer2
} from "@angular/core";
import { Router } from "@angular/router";
import { BaseComponent } from "src/app/components/base.component";
import { AuthGuard } from "src/app/guard/auth.guard";
import { RoleTypeSM } from "src/app/service-models/app/enums/role-type-s-m.enum";
import { CommonService } from "src/app/services/common.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { WebsiteViewModel } from "src/app/view-models/website.viewmodel";
import { ThemeService } from "src/app/services/theme.service"; // ✅ added

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
  menuOpen = false;
  scrolled = false;

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private el: ElementRef,
    private renderer: Renderer2,
    private authGuard: AuthGuard,
    private router: Router,
    public theme: ThemeService // ✅ added
  ) {
    super(commonService, logService);
  }

  async ngOnInit() {
    await this.navigateToSection("");
    this.updateScrolledState();
  }

  @HostListener("window:scroll")
  onWindowScroll() {
    this.updateScrolledState();
  }

  private updateScrolledState() {
    this.scrolled = (window.scrollY || 0) > 40;
  }

  toggleMobileNav() {
    this.menuOpen = !this.menuOpen;
    this.syncMenuDomClasses();
  }

  closeMobileMenu() {
    if (!this.menuOpen) return;
    this.menuOpen = false;
    this.syncMenuDomClasses();
  }

  @HostListener("document:keydown.escape")
  onEsc() {
    this.closeMobileMenu();
  }

  @HostListener("document:click", ["$event"])
  onDocClick(e: MouseEvent) {
    if (!this.menuOpen) return;

    const menu = this.el.nativeElement.querySelector("#mobile-menu");
    const btn = this.el.nativeElement.querySelector("#mobile-menu-btn");
    const target = e.target as Node;

    const inside =
      (menu && menu.contains(target)) || (btn && btn.contains(target));

    if (!inside) this.closeMobileMenu();
  }

  private syncMenuDomClasses() {
    const navbar = this.el.nativeElement.querySelector("#navbar");
    if (navbar) {
      if (this.menuOpen) this.renderer.addClass(navbar, "navbar-mobile");
      else this.renderer.removeClass(navbar, "navbar-mobile");
    }

    const mobileMenu = this.el.nativeElement.querySelector("#mobile-menu");
    if (mobileMenu) {
      if (this.menuOpen) this.renderer.removeClass(mobileMenu, "hidden");
      else this.renderer.addClass(mobileMenu, "hidden");
    }

    const mobileNavToggle =
      this.el.nativeElement.querySelector(".mobile-nav-toggle");
    if (mobileNavToggle) {
      if (this.menuOpen) {
        mobileNavToggle.classList.remove("bi-list");
        mobileNavToggle.classList.add("bi-x");
      } else {
        mobileNavToggle.classList.remove("bi-x");
        mobileNavToggle.classList.add("bi-list");
      }
    }
  }

  onNavLinkClick(id: string) {
    const el = document.getElementById(id);
    if (el) el.scrollIntoView({ behavior: "smooth" });
    this.closeMobileMenu();
  }

  scrollToSection(target: string) {
    const element = document.getElementById(target);
    if (element) {
      element.scrollIntoView({ behavior: "smooth" });
    }
    this.closeMobileMenu();
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
      } else {
        this.router.navigate(["/login"]);
      }
    } else {
      this.router.navigate(["/login"]);
    }
  }
}
