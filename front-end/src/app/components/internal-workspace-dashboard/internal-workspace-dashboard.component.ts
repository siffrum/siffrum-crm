import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";

interface CrmAppCard {
  title: string;
  icon: string;
  route: string;
  disabled?: boolean;
}

@Component({
  selector: "app-internal-workspace-dashboard",
  templateUrl: "./internal-workspace-dashboard.component.html",
  styleUrls: ["./internal-workspace-dashboard.component.scss"],
  standalone: false,
})
export class InternalWorkspaceDashboardComponent implements OnInit {

  apps: CrmAppCard[] = [];
  loading = true;

  constructor(private router: Router) {}

  async ngOnInit() {
    await this.loadApps();
  }

  async loadApps(): Promise<void> {
    // simulate async (future: API / permission load)
    await new Promise(resolve => setTimeout(resolve, 200));

    this.apps = [
      { title: "Overview", icon: "bi bi-building", route: "/company-overview" },
      { title: "Attendance", icon: "bi bi-person-bounding-box", route: "/attendance" },
      { title: "Directory", icon: "bi bi-people", route: "/employees-list" },
      { title: "Leaves", icon: "bi bi-calendar2-check", route: "/leaves" },
      { title: "Letters", icon: "bi bi-card-text", route: "/company-letter" },
      { title: "Departments", icon: "bi bi-diagram-3", route: "/departments" },
      { title: "Shift", icon: "bi bi-clock-history", route: "/shift" },
      { title: "Payroll Structure", icon: "bi bi-wallet2", route: "/payroll-structure" },
      { title: "Transactions", icon: "bi bi-cash", route: "/transactions" },
      { title: "Pricing", icon: "bi bi-cash-coin", route: "/license" },
    ];

    this.loading = false;
  }

  async go(app: CrmAppCard) {
    if (!app.route || app.disabled) return;
    await this.router.navigateByUrl(app.route);
  }
}