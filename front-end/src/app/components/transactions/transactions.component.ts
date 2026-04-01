import { Component, OnInit, TemplateRef, ViewChild, HostListener } from "@angular/core";
import { DatePipe } from "@angular/common";
import { format } from "date-fns";

import { CommonService } from "src/app/services/common.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { TransactionViewModel } from "src/app/view-models/payroll-transaction.viewmodel";
import { BaseComponent } from "../base.component";
import { TransactionService } from "src/app/services/transaction.service";
import { GeneratePayrollTransactionSM } from "src/app/service-models/app/v1/client/generate-payroll-transaction-s-m";
import { ModuleNameSM } from "src/app/service-models/app/enums/module-name-s-m.enum";
import { PermissionSM } from "src/app/service-models/app/v1/client/permission-s-m";
import { AccountService } from "src/app/services/account.service";

type HistoryItem = { title: string; sub: string; time: string };
type PayslipItem = {
  month: string;
  file: string;
  employeeName: string;
  designation: string;
  amount: number;
  paymentType: string;
  paymentFor: string;
  status: string;
};

@Component({
  selector: "app-transactions",
  templateUrl: "./transactions.component.html",
  styleUrls: ["./transactions.component.scss"],
  standalone: false,
})
export class TransactionsComponent
  extends BaseComponent<TransactionViewModel>
  implements OnInit
{
  @ViewChild("content") content: TemplateRef<any> | undefined;

  hideAmount = false;

  // Drawer
  drawerOpen = false;
  drawerTab: "history" | "payslips" = "history";

  // Search
  searchText = "";
  filteredList: any[] = [];

  // Stats
  paidCount = 0;
  notPaidCount = 0;
  resignedCount = 0;

  // Drawer data
  historyList: HistoryItem[] = [];
  payslipList: PayslipItem[] = [];

  // ✅ FIX: Template-safe total count getter (prevents totalCount/totalcount warnings)
  get totalCountSafe(): number {
    const pg: any = this.viewModel?.pagination;
    // support both totalCount and TotalCount just in case model uses PascalCase
    return Number(pg?.totalCount ?? pg?.TotalCount ?? 0);
  }

  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    public datepipe: DatePipe,
    private transactionService: TransactionService,
    private accountService: AccountService
  ) {
    super(commonService, logService);
    this.viewModel = new TransactionViewModel();
  }

  async ngOnInit() {
    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;
    await this.getMonths();
    this.viewModel.selectedMonth = this.viewModel.months[0] || "";
    await this.getPermissions();
  }

  // ===== Drawer controls =====
  openPayrollDrawer() {
    this.drawerOpen = true;
    this.drawerTab = "history";
  }

  closePayrollDrawer() {
    this.drawerOpen = false;
  }

  @HostListener("document:keydown.escape")
  onEsc() {
    if (this.drawerOpen) this.closePayrollDrawer();
  }

  toggleAmount() {
    this.hideAmount = !this.hideAmount;
  }

  openHistoryFromRow(item: any) {
    this.drawerOpen = true;
    this.drawerTab = item?.paymentPaid === true ? "payslips" : "history";
    this.historyList.unshift({
      title: `Payslip opened • ${item?.employeeName || "Employee"}`,
      sub: `Payment For: ${item?.paymentFor || "-"} • Type: ${item?.paymentType || "-"}`,
      time: new Date().toLocaleString(),
    });
  }

  downloadPayslip(p: PayslipItem) {
    const html = this.buildPayslipHtml(p);
    const blob = new Blob([html], { type: "text/html;charset=utf-8" });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement("a");
    link.href = url;
    link.download = p.file.replace(/\.pdf$/i, ".html");
    document.body.appendChild(link);
    link.click();
    window.URL.revokeObjectURL(url);
    link.remove();

    this.historyList.unshift({
      title: `Payslip download requested`,
      sub: `${p.employeeName} • ${p.file}`,
      time: new Date().toLocaleString(),
    });
    this.drawerOpen = true;
    this.drawerTab = "history";
  }

  // ===== Safe status helpers =====
  isActiveStatus(item: any): boolean {
    const s = item?.employeeStatus;
    return s !== null && s !== undefined && String(s) === "Active";
  }

  isResignedStatus(item: any): boolean {
    const s = item?.employeeStatus;
    return s !== null && s !== undefined && String(s) === "Resigned";
  }

  // ===== Search =====
  onSearch() {
    this.applyFilter();
  }

  private applyFilter() {
    const q = (this.searchText || "").trim().toLowerCase();
    const list = this.viewModel.payrollTransactionList || [];

    if (!q) {
      this.filteredList = [...list];
      return;
    }

    this.filteredList = list.filter((x: any) => {
      const a = String(x?.employeeName || "").toLowerCase();
      const b = String(x?.designation || "").toLowerCase();
      const c = String(x?.paymentType || "").toLowerCase();
      const d = String(x?.paymentFor || "").toLowerCase();
      const e = String(x?.employeeStatus || "").toLowerCase();
      return a.includes(q) || b.includes(q) || c.includes(q) || d.includes(q) || e.includes(q);
    });
  }

  private computeStats() {
    const list = this.viewModel.payrollTransactionList || [];
    this.paidCount = list.filter((x: any) => x?.paymentPaid === true).length;
    this.notPaidCount = list.filter((x: any) => this.isActiveStatus(x) && x?.paymentPaid === false).length;
    this.resignedCount = list.filter((x: any) => this.isResignedStatus(x)).length;
  }

  private refreshPayslips() {
    const list = this.viewModel.payrollTransactionList || [];
    this.payslipList = list
      .filter((x: any) => Number(x?.paymentAmount || 0) > 0)
      .map((x: any) => ({
        month: x?.paymentFor || this.viewModel.selectedMonth || "-",
        file: `Payslip_${String(x?.employeeName || "Employee").replace(/\s+/g, "_")}_${String(x?.paymentFor || this.viewModel.selectedMonth || "Payroll").replace(/\s+/g, "_")}.html`,
        employeeName: x?.employeeName || "Employee",
        designation: x?.designation || "-",
        amount: Number(x?.paymentAmount || 0),
        paymentType: x?.paymentType || "-",
        paymentFor: x?.paymentFor || "-",
        status: x?.paymentPaid === true ? "Paid" : "Generated",
      }));
  }

  private buildPayslipHtml(p: PayslipItem): string {
    const amount = new Intl.NumberFormat("en-IN", {
      style: "currency",
      currency: "INR",
      maximumFractionDigits: 2,
    }).format(Number(p.amount || 0));

    return `<!doctype html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <title>${p.file}</title>
  <style>
    body { font-family: Arial, sans-serif; background:#f8fafc; color:#0f172a; margin:0; padding:32px; }
    .sheet { max-width:760px; margin:0 auto; background:#fff; border:1px solid #e2e8f0; border-radius:20px; padding:28px; }
    .eyebrow { display:inline-block; padding:6px 10px; border-radius:999px; background:#dbeafe; color:#1d4ed8; font-size:12px; font-weight:700; text-transform:uppercase; letter-spacing:.08em; }
    h1 { margin:14px 0 8px; font-size:28px; }
    p { color:#475569; line-height:1.6; }
    .grid { display:grid; grid-template-columns:1fr 1fr; gap:14px; margin-top:22px; }
    .card { border:1px solid #e2e8f0; border-radius:16px; padding:16px; background:#f8fafc; }
    .label { font-size:12px; text-transform:uppercase; letter-spacing:.08em; color:#64748b; font-weight:700; margin-bottom:6px; }
    .value { font-size:16px; font-weight:700; color:#0f172a; }
    .amount { margin-top:20px; padding:20px; border-radius:18px; background:linear-gradient(135deg,#2563eb,#1d4ed8); color:#fff; }
    .amount .label { color:rgba(255,255,255,.75); }
    .amount .value { color:#fff; font-size:30px; }
  </style>
</head>
<body>
  <div class="sheet">
    <span class="eyebrow">Payroll Payslip</span>
    <h1>${p.employeeName}</h1>
    <p>Generated from the payroll transactions page using the current paid transaction data.</p>
    <div class="grid">
      <div class="card"><div class="label">Designation</div><div class="value">${p.designation}</div></div>
      <div class="card"><div class="label">Status</div><div class="value">${p.status}</div></div>
      <div class="card"><div class="label">Payment For</div><div class="value">${p.paymentFor}</div></div>
      <div class="card"><div class="label">Payment Type</div><div class="value">${p.paymentType}</div></div>
    </div>
    <div class="amount">
      <div class="label">Paid Amount</div>
      <div class="value">${amount}</div>
    </div>
  </div>
</body>
</html>`;
  }

  // ===== APIs =====
  async getPayrolltransactionCountOfCompany() {
    try {
      await this._commonService.presentLoading();

      const UTCDate = this._commonService.getISODateFromMonthYear(this.viewModel.selectedMonth);
      const resp = await this.transactionService.getPayrollTransactionCountOfCompany(UTCDate);

      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: "Error!",
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error",
        });
      } else {
        // keep your existing contract
        (this.viewModel.pagination as any).totalCount = resp.successData?.intResponse ?? 0;
      }
    } catch (error) {
      await this._exceptionHandler.logObject(error);
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  async getTransactionMonthlyBased() {
    try {
      await this._commonService.presentLoading();

      const UTCDate = this._commonService.getISODateFromMonthYear(this.viewModel.selectedMonth);
      const resp = await this.transactionService.getPayrollTransactionMonthlyBased(this.viewModel, UTCDate);

      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        await this._commonService.ShowToastAtTopEnd(resp.errorData.displayMessage, "error");
        return;
      }

      this.viewModel.showTable = true;
      this.viewModel.payrollTransactionList = resp.successData || [];

      await this.getPayrolltransactionCountOfCompany();

      const hasErrors = this.viewModel.payrollTransactionList.some((x: any) => x?.errorInGeneration === true);

      if (this.viewModel.payrollTransactionList.length === 0) {
        this.viewModel.showGenerateAllBtn = false;
        this.viewModel.showPayAllBtn = false;
        await this._commonService.ShowToastAtTopEnd("No records Found", "info");
      } else if (hasErrors) {
        this.viewModel.showGenerateAllBtn = true;
        this.viewModel.showPayAllBtn = true;
      } else {
        this.viewModel.showGenerateAllBtn = false;
        this.viewModel.showPayAllBtn = true;
        await this._commonService.ShowToastAtTopEnd("Data Successfully Retrieved", "success");
      }

      this.computeStats();
      this.applyFilter();
      this.refreshPayslips();
    } catch (error) {
      await this._exceptionHandler.logObject(error);
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  async loadPagedataWithPagination(pageNumber: number) {
    if (pageNumber && pageNumber > 0) {
      (this.viewModel.pagination as any).PageNo = pageNumber;
      await this.getTransactionMonthlyBased();
    }
  }

  async getPermissions() {
    const moduleName = ModuleNameSM.CompanyAccountTransaction;
    const resp: PermissionSM | any = await this.accountService.getMyModulePermissions(moduleName);
    this.viewModel.Permission = resp;
  }

  async generateEmployeePayrollByClientUserId(generatePayroll: GeneratePayrollTransactionSM) {
    try {
      await this._commonService.presentLoading();

      const monthLabel = format(new Date(this.viewModel.selectedMonth), "MMMM-yyyy");
      generatePayroll.paymentFor = monthLabel;

      const resp = await this.transactionService.generatePayrollByClientUserId(generatePayroll);

      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        await this._commonService.ShowToastAtTopEnd(resp.errorData.displayMessage, "error");
      } else {
        this.historyList.unshift({
          title: `Payroll generated • ${generatePayroll?.employeeName || "Employee"}`,
          sub: `For: ${monthLabel}`,
          time: new Date().toLocaleString(),
        });
        await this.getTransactionMonthlyBased();
      }
    } catch (error) {
      await this._exceptionHandler.logObject(error);
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  async generateAllEmployeePayroll() {
    try {
      await this._commonService.presentLoading();

      const filteredList = (this.viewModel.payrollTransactionList || []).filter(
        (x: any) => Number(x?.paymentAmount || 0) === 0 && this.isActiveStatus(x)
      );

      const resp = await this.transactionService.GenerateAllEmployeePayroll(filteredList);

      if (resp.isError) {
        await this._exceptionHandler.logObject(resp);
        await this._commonService.showInfoOnAlertWindowPopup("error", resp.errorData.displayMessage, "error");
      } else {
        this.viewModel.payrollTransactionList = resp.successData || [];
        this.historyList.unshift({
          title: "Generate All payroll clicked",
          sub: `Employees queued: ${filteredList.length}`,
          time: new Date().toLocaleString(),
        });
        await this.getTransactionMonthlyBased();
      }

      this.computeStats();
      this.applyFilter();
      this.refreshPayslips();
    } catch (error) {
      await this._exceptionHandler.logObject(error);
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  async getMonths() {
    const date = new Date();
    const months: string[] = [];
    const monthNames = [
      "January","February","March","April","May","June",
      "July","August","September","October","November","December",
    ];

    for (let i = 0; i < 12; i++) {
      months.push(monthNames[date.getMonth()] + " " + date.getFullYear());
      date.setMonth(date.getMonth() - 1);
    }

    this.viewModel.months = months;
  }
}
