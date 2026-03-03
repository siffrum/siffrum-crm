import {
  AfterViewInit,
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  Renderer2,
} from "@angular/core";
import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import Chart from "chart.js/auto";

import { BaseComponent } from "src/app/components/base.component";
import { AuthGuard } from "src/app/guard/auth.guard";
import { RoleTypeSM } from "src/app/service-models/app/enums/role-type-s-m.enum";
import { ContactUsSM } from "src/app/service-models/app/v1/general/contact-us-s-m";
import { CommonService } from "src/app/services/common.service";
import { ContactUsService } from "src/app/services/contact-us.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { WebsiteViewModel } from "src/app/view-models/website.viewmodel";

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

  private ioReveal?: IntersectionObserver;
  private ioCount?: IntersectionObserver;

  private prefersReduced =
    window.matchMedia?.("(prefers-reduced-motion: reduce)")?.matches ?? false;

  private scrollHandler = () => this.updateScrollProgress();

  // ✅ Use a local form model (avoids template type-check errors)
  contactForm = {
    name: "",
    email: "",
    message: "",
  };

  // Charts
  private charts: Chart[] = [];
  private themeObserver?: MutationObserver;

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private el: ElementRef<HTMLElement>,
    private contactUsService: ContactUsService,
    private router: Router,
    private authGuard: AuthGuard,
    private renderer: Renderer2
  ) {
    super(commonService, logService);
    this.viewModel = new WebsiteViewModel();

    // ✅ keep this for API call compatibility
    if (!this.viewModel.contactUsObj) {
      this.viewModel.contactUsObj = new ContactUsSM();
    }
  }

  async ngOnInit() {
    this._commonService.layoutVM.toogleWrapper = "loginWrapper";
    this._commonService.layoutVM.showLeftSideMenu = false;

    await this._commonService.presentLoading();
    setTimeout(async () => {
      await this._commonService.dismissLoader();
    }, 1000);
  }

  ngAfterViewInit(): void {
    this.applyAutoStagger();
    this.setupRevealObserver();
    this.setupCountups();
    this.injectScrollProgress();
    window.addEventListener("scroll", this.scrollHandler, { passive: true });
    this.setupPricingToggle();

    // Charts
    this.initCharts();
    this.watchThemeChanges();
  }

  ngOnDestroy(): void {
    this.ioReveal?.disconnect();
    this.ioCount?.disconnect();
    window.removeEventListener("scroll", this.scrollHandler);
    document.getElementById("scroll-progress")?.remove();

    this.destroyCharts();
    this.themeObserver?.disconnect();
  }

  // ------------------------------
  // Send Message (fixed + safe)
  // ------------------------------
  async sendMessage(contactUsForm: NgForm) {
    try {
      if (contactUsForm.invalid) {
        await this._commonService.showSweetAlertToast({
          title: "Please fill all fields correctly!",
          icon: "error",
        });
        return;
      }

      await this._commonService.presentLoading();

      // ✅ Map local form -> ContactUsSM without template typing problems
      const payload = new ContactUsSM() as any;

      // Put values in common keys (works even if backend ignores unknown keys)
      payload.name = this.contactForm.name;
      payload.fullName = this.contactForm.name;
      payload.email = this.contactForm.email;
      payload.emailId = this.contactForm.email;
      payload.message = this.contactForm.message;
      payload.query = this.contactForm.message;

      const resp = await this.contactUsService.addNewcontactUsDetails(payload);

      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        await this._commonService.showSweetAlertToast({
          title: "Error!",
          text: resp.errorData?.displayMessage ?? "Something went wrong",
          position: "top-end",
          icon: "error",
        });
        return;
      }

      this._commonService.ShowToastAtTopEnd("Message sent successfully", "success");

      // reset form
      this.contactForm = { name: "", email: "", message: "" };
      contactUsForm.resetForm();

      this.router.navigate(["/website"]);
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  // ------------------------------
  // Token check (same)
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
  // Reveal
  // ==========================================================
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

  private applyAutoStagger(): void {
    document.querySelectorAll<HTMLElement>(".grid").forEach((grid) => {
      const kids = Array.from(grid.children).filter(
        (node): node is HTMLElement =>
          node instanceof HTMLElement && node.classList.contains("reveal")
      );

      kids.forEach((el, i) => {
        if (!el.style.getPropertyValue("--d")) {
          el.style.setProperty("--d", `${Math.min(i * 80, 520)}ms`);
        }
      });
    });
  }

  // ==========================================================
  // Countups
  // ==========================================================
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
          if (!entry.isIntersecting) return;

          const el = entry.target as HTMLElement;
          const to = parseInt(el.dataset["count"] || "0", 10);
          const duration = parseInt(el.dataset["duration"] || "900", 10);

          this.animateCountUp(el, to, duration);
          this.ioCount?.unobserve(el);
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

  // ==========================================================
  // Scroll progress
  // ==========================================================
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
    const bar = document.getElementById("scroll-progress") as HTMLElement | null;
    if (!bar) return;

    const h = document.documentElement.scrollHeight - window.innerHeight;
    const p = h > 0 ? window.scrollY / h : 0;

    bar.style.width = `${Math.max(0, Math.min(1, p)) * 100}%`;
  }

  // ==========================================================
  // Pricing toggle
  // ==========================================================
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

  // ==========================================================
  // Charts
  // ==========================================================
  private isDark(): boolean {
    return document.documentElement.classList.contains("dark") || document.body.classList.contains("dark");
  }

  private getChartColors() {
    const dark = this.isDark();
    return {
      grid: dark ? "rgba(255,255,255,0.08)" : "rgba(15,23,42,0.08)",
      ticks: dark ? "rgba(226,232,240,0.75)" : "rgba(51,65,85,0.75)",
      title: dark ? "rgba(226,232,240,0.92)" : "rgba(15,23,42,0.92)",
      blue: "#2563eb",
      indigo: "#6366f1",
      emerald: "#34d399",
      rose: "#fb7185",
    };
  }

  private destroyCharts(): void {
    this.charts.forEach((c) => c.destroy());
    this.charts = [];
  }

  private initCharts(): void {
    if (this.prefersReduced) return;

    this.destroyCharts();
    const c = this.getChartColors();

    const revenueEl = document.getElementById("chartRevenue") as HTMLCanvasElement | null;
    if (revenueEl) {
      this.charts.push(
        new Chart(revenueEl, {
          type: "line",
          data: {
            labels: ["W1", "W2", "W3", "W4"],
            datasets: [
              {
                label: "Closed-won",
                data: [48, 62, 55, 74],
                borderColor: c.blue,
                backgroundColor: "rgba(37,99,235,0.18)",
                fill: true,
                tension: 0.35,
                pointRadius: 3,
              },
              {
                label: "Pipeline",
                data: [66, 58, 72, 68],
                borderColor: c.indigo,
                backgroundColor: "rgba(99,102,241,0.12)",
                fill: true,
                tension: 0.35,
                pointRadius: 3,
              },
            ],
          },
          options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: { legend: { display: false } },
            scales: {
              x: { grid: { color: c.grid }, ticks: { color: c.ticks } },
              y: { grid: { color: c.grid }, ticks: { color: c.ticks } },
            },
          },
        })
      );
    }

    const slaEl = document.getElementById("chartSla") as HTMLCanvasElement | null;
    if (slaEl) {
      this.charts.push(
        new Chart(slaEl, {
          type: "doughnut",
          data: {
            labels: ["Within SLA", "Breached"],
            datasets: [{ data: [94, 6], backgroundColor: [c.emerald, c.rose], borderWidth: 0 }],
          },
          options: {
            responsive: true,
            maintainAspectRatio: false,
            cutout: "72%",
            plugins: { legend: { display: false } },
          },
        })
      );
    }

    const pipeEl = document.getElementById("chartPipeline") as HTMLCanvasElement | null;
    if (pipeEl) {
      this.charts.push(
        new Chart(pipeEl, {
          type: "bar",
          data: {
            labels: ["Lead", "Qualified", "Proposal", "Negotiation", "Won"],
            datasets: [{ data: [28, 22, 17, 11, 9], backgroundColor: "rgba(37,99,235,0.55)", borderRadius: 10 }],
          },
          options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: { legend: { display: false } },
            scales: {
              x: { grid: { color: c.grid }, ticks: { color: c.ticks } },
              y: { grid: { color: c.grid }, ticks: { color: c.ticks } },
            },
          },
        })
      );
    }
  }

  private watchThemeChanges(): void {
    this.themeObserver = new MutationObserver(() => this.initCharts());
    this.themeObserver.observe(document.documentElement, { attributes: true, attributeFilter: ["class"] });
  }
}