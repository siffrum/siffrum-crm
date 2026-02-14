import { AfterViewInit, Component, ElementRef, OnDestroy, OnInit, Renderer2 } from "@angular/core";
import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { BaseComponent } from "src/app/components/base.component";
import { AuthGuard } from "src/app/guard/auth.guard";
import { RoleTypeSM } from "src/app/service-models/app/enums/role-type-s-m.enum";
import { ContactUsSM } from "src/app/service-models/app/v1/general/contact-us-s-m";
import { CommonService } from "src/app/services/common.service";
import { ContactUsService } from "src/app/services/contact-us.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { WebsiteViewModel } from "src/app/view-models/website.viewmodel";
import { ThemeService } from 'src/app/services/theme.service';


@Component({
  selector: "app-website",
  templateUrl: "./website.component.html",
  styleUrls: ["./website.component.scss"],
  standalone: false,
})
export class WebsiteComponent
  extends BaseComponent<WebsiteViewModel>
  implements OnInit, AfterViewInit, OnDestroy
{
  Waypoint: any;

  // ------- Landing effects state -------
  private ioReveal?: IntersectionObserver;
  private ioCount?: IntersectionObserver;
  private prefersReduced =
    window.matchMedia?.("(prefers-reduced-motion: reduce)")?.matches ?? false;

  private scrollHandler = () => this.updateScrollProgress();

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private el: ElementRef,
    private contactUsService: ContactUsService,
    private router: Router,
    private authGuard: AuthGuard,
    private renderer: Renderer2
  ) {
    super(commonService, logService);
    this.viewModel = new WebsiteViewModel();
  }

  async ngOnInit() {
    this._commonService.layoutVM.toogleWrapper = "loginWrapper";
    this._commonService.layoutVM.showLeftSideMenu = false;

    await this._commonService.presentLoading();
    setTimeout(async () => {
      await this._commonService.dismissLoader();
    }, 1000);
  }

  // After the HTML renders, setup reveal/count/pricing/progress
  ngAfterViewInit(): void {
    // If you are not using these parts in your HTML, it will safely do nothing.
    this.applyAutoStagger();
    this.setupRevealObserver();
    this.setupCountups();
    this.injectScrollProgress();
    window.addEventListener("scroll", this.scrollHandler, { passive: true });
    this.setupPricingToggle();
  }

  ngOnDestroy(): void {
    this.ioReveal?.disconnect();
    this.ioCount?.disconnect();
    window.removeEventListener("scroll", this.scrollHandler);
    document.getElementById("scroll-progress")?.remove();
  }

  // ------------------------------
  // Your original progress bar
  // ------------------------------
  progressBar() {
    const skilsContent = this.el.nativeElement.querySelector(".skills-content");
    if (skilsContent) {
      // Waypoint must be provided globally by your project if you still use this.
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

  // ------------------------------
  // Send Message (original)
  // ------------------------------
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

      const resp = await this.contactUsService.addNewcontactUsDetails(
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
        this._commonService.ShowToastAtTopEnd("Message Sent Successully", "success");
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

  // ------------------------------
  // Token check (original)
  // ------------------------------
  async checkToken() {
    if (await this.authGuard.IsTokenValid()) {
      const roleType: any = this._commonService.layoutVM.tokenRole;

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

  // ==========================================================
  // Landing page effects (from your HTML <script> converted)
  // ==========================================================

  // ----- Reveal observer -----
  private setupRevealObserver(): void {
    const revealEls = Array.from(document.querySelectorAll<HTMLElement>(".reveal"));
    if (!revealEls.length) return;

    if (this.prefersReduced || !("IntersectionObserver" in window)) {
      revealEls.forEach((el) => el.classList.add("active"));
      return;
    }

    this.ioReveal = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          if (entry.isIntersecting) {
            (entry.target as HTMLElement).classList.add("active");
            this.ioReveal?.unobserve(entry.target);
          }
        });
      },
      { threshold: 0.12, rootMargin: "0px 0px -8% 0px" }
    );

    revealEls.forEach((el) => this.ioReveal!.observe(el));
  }

  // ----- Stagger reveals inside grids -----
  private applyAutoStagger(): void {
    document.querySelectorAll<HTMLElement>(".grid").forEach((grid) => {
      const kids = Array.from(grid.children).filter(
        (el): el is HTMLElement =>
          el instanceof HTMLElement && el.classList.contains("reveal")
      );

      kids.forEach((el, i) => {
        if (!el.style.getPropertyValue("--d")) {
          el.style.setProperty("--d", `${Math.min(i * 80, 520)}ms`);
        }
      });
    });
  }

  // ----- Countups -----
  private setupCountups(): void {
    const els = Array.from(document.querySelectorAll<HTMLElement>("[data-count]"));
    if (!els.length) return;

    if (this.prefersReduced || !("IntersectionObserver" in window)) {
      els.forEach((el) =>
        this.animateCountUp(el, parseInt(el.dataset["count"] || "0", 10), 1)
      );
      return;
    }

    this.ioCount = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          if (entry.isIntersecting) {
            const el = entry.target as HTMLElement;
            const to = parseInt(el.dataset["count"] || "0", 10);
            const duration = parseInt(el.dataset["duration"] || "900", 10);
            this.animateCountUp(el, to, duration);
            this.ioCount?.unobserve(el);
          }
        });
      },
      { threshold: 0.55 }
    );

    els.forEach((el) => this.ioCount!.observe(el));
  }

  private animateCountUp(el: HTMLElement, to: number, duration = 900): void {
    const start = performance.now();
    const from = 0;
    const prefix = el.dataset["prefix"] || "";
    const suffix = el.dataset["suffix"] || "";

    const tick = (now: number) => {
      const t = Math.min(1, (now - start) / duration);
      const eased = 1 - Math.pow(1 - t, 3);
      const value = Math.round(from + (to - from) * eased);
      el.textContent = `${prefix}${value.toLocaleString()}${suffix}`;
      if (t < 1) requestAnimationFrame(tick);
    };

    requestAnimationFrame(tick);
  }

  // ----- Scroll progress bar -----
  private injectScrollProgress(): void {
    if (document.getElementById("scroll-progress")) return;

    const bar = this.renderer.createElement("div");
    bar.id = "scroll-progress";
    bar.style.cssText =
      "position:fixed;left:0;top:0;height:3px;width:0;z-index:999;" +
      "background:linear-gradient(90deg,#2563eb,#6366f1,#0ea5e9);" +
      "box-shadow:0 10px 30px rgba(37,99,235,.25);";

    this.renderer.appendChild(document.body, bar);
    this.updateScrollProgress();
  }

  private updateScrollProgress(): void {
    if (this.prefersReduced) return;
    const bar = document.getElementById("scroll-progress");
    if (!bar) return;

    const h = document.documentElement.scrollHeight - window.innerHeight;
    const p = h > 0 ? window.scrollY / h : 0;
    bar.style.width = `${Math.max(0, Math.min(1, p)) * 100}%`;
  }

  // ----- Pricing toggle -----
  private setupPricingToggle(): void {
    const monthlyBtn = document.getElementById("monthly-btn");
    const yearlyBtn = document.getElementById("yearly-btn");
    const proPrice = document.getElementById("pro-price");
    const proPeriod = document.getElementById("pro-period");
    const savingsBadge = document.getElementById("savings-badge");

    if (!monthlyBtn || !yearlyBtn || !proPrice || !proPeriod || !savingsBadge) return;

    const setActive = (btn: HTMLElement, active: boolean) => {
      btn.classList.toggle("bg-blue-600", active);
      btn.classList.toggle("text-white", active);
      btn.classList.toggle("shadow-lg", active);
      btn.classList.toggle("shadow-blue-500/30", active);
      btn.classList.toggle("text-slate-500", !active);
      btn.classList.toggle("dark:text-slate-300", !active);
    };

    const updatePricing = (isYearly: boolean) => {
      if (isYearly) {
        proPrice.textContent = "$712";
        proPeriod.textContent = "/mo";
        (savingsBadge as HTMLElement).style.opacity = "1";
        setActive(yearlyBtn as HTMLElement, true);
        setActive(monthlyBtn as HTMLElement, false);
      } else {
        proPrice.textContent = "$890";
        proPeriod.textContent = "/mo";
        (savingsBadge as HTMLElement).style.opacity = "0";
        setActive(monthlyBtn as HTMLElement, true);
        setActive(yearlyBtn as HTMLElement, false);
      }
    };

    monthlyBtn.addEventListener("click", () => updatePricing(false));
    yearlyBtn.addEventListener("click", () => updatePricing(true));
    updatePricing(true);
  }
}
