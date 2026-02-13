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
  // ✅ Added for modern navbar
  menuOpen = false;
  scrolled = false;

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private el: ElementRef,
    private renderer: Renderer2,
    private authGuard: AuthGuard,
    private router: Router
  ) {
    super(commonService, logService);
  }

  async ngOnInit() {
    // keep your original behavior
    await this.navigateToSection("");
    // set initial scroll state
    this.updateScrolledState();
  }

  /* =========================
     Scroll morph (adds .scrolled)
     ========================= */
  @HostListener("window:scroll")
  onWindowScroll() {
    this.updateScrolledState();
  }

  private updateScrolledState() {
    this.scrolled = (window.scrollY || 0) > 40;
  }

  /* =========================
     Mobile menu open/close
     (keeps your method name)
     ========================= */
  toggleMobileNav() {
    this.menuOpen = !this.menuOpen;
    this.syncMenuDomClasses();
  }

  closeMobileMenu() {
    if (!this.menuOpen) return;
    this.menuOpen = false;
    this.syncMenuDomClasses();
  }

  // ESC closes mobile menu
  @HostListener("document:keydown.escape")
  onEsc() {
    this.closeMobileMenu();
  }

  // click outside closes menu
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

  // ✅ keeps compatibility with old markup (#navbar + navbar-mobile, .mobile-nav-toggle icons)
  private syncMenuDomClasses() {
    const navbar = this.el.nativeElement.querySelector("#navbar");
    if (navbar) {
      if (this.menuOpen) this.renderer.addClass(navbar, "navbar-mobile");
      else this.renderer.removeClass(navbar, "navbar-mobile");
    }

    const mobileMenu = this.el.nativeElement.querySelector("#mobile-menu");
    if (mobileMenu) {
      // If you're using Tailwind "hidden"
      if (this.menuOpen) this.renderer.removeClass(mobileMenu, "hidden");
      else this.renderer.addClass(mobileMenu, "hidden");

      // If you're using our SCSS dropdown wrapper, it relies on [class.open] in HTML
      // so this is just extra safety; it won’t break anything.
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

  /* =========================
     Link click helper (optional)
     call this from HTML: (click)="onNavLinkClick('pricing')"
     ========================= */
  onNavLinkClick(id: string) {
    // smooth scroll if section exists
    const el = document.getElementById(id);
    if (el) el.scrollIntoView({ behavior: "smooth" });

    // close mobile menu after clicking a link
    this.closeMobileMenu();
  }

  /* =========================
     Your original functions (unchanged)
     ========================= */
  scrollToSection(target: string) {
    const element = document.getElementById(target);
    if (element) {
      element.scrollIntoView({ behavior: "smooth" });
    }
    // ✅ close after click (doesn't affect desktop)
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
