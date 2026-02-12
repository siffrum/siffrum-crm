import {
  Component,
  ElementRef,
  EventEmitter,
  OnInit,
  Output,
  ViewChild,
} from "@angular/core";
import { BaseComponent } from "../base.component";
import {
  LicenseViewModel,
  PaymentStatusInfo,
} from "src/app/view-models/license-info.viewmodel";
import { CommonService } from "src/app/services/common.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { format, parse, toDate } from "date-fns";
import { AppConstants } from "src/app-constants";
import { LicenseInfoService } from "src/app/services/license-info.service";
import { PaymentModeSM } from "src/app/service-models/app/enums/payment-mode-s-m.enum";
import { CheckoutSessionResponseSM } from "src/app/service-models/app/v1/license/checkout-session-response-s-m";
import { AccountService } from "src/app/services/account.service";
import { RoleTypeSM } from "src/app/service-models/app/enums/role-type-s-m.enum";
import { AuthGuard } from "src/app/guard/auth.guard";
declare const Stripe: any;
@Component({
    selector: "app-license-info",
    templateUrl: "./license-info.component.html",
    styleUrls: ["./license-info.component.scss"],
    standalone: false
})
export class LicenseInfoComponent
  extends BaseComponent<LicenseViewModel>
  implements OnInit
{
  constructor(
    commonService: CommonService,
    logService: LogHandlerService,
    private licenseInfoService: LicenseInfoService,
    private accountService: AccountService,
    private authGuard: AuthGuard
  ) {
    super(commonService, logService);
    this.viewModel = new LicenseViewModel();
  }
  async ngOnInit() {
    await this._commonService.presentLoading();
    await setTimeout(async () => {
      await this._commonService.dismissLoader();
      console.log("dismissed on onint");
      await this.loadPageData();
      await this.getActiveLicense();
    }, 1000);
  }

  @Output() groupId: EventEmitter<number> = new EventEmitter();

  @ViewChild("pricingCards") pricingCards!: ElementRef;

  scrollRight() {
    this.pricingCards.nativeElement.scrollTo({
      left: this.pricingCards.nativeElement.scrollLeft + 1600,
      behavior: "smooth",
    });
  }

  scrollLeft() {
    this.pricingCards.nativeElement.scrollTo({
      left: this.pricingCards.nativeElement.scrollLeft - 1600,
      behavior: "smooth",
    });
  }
  IsValidForRole(role: any): boolean {
    if (
      this._commonService.layoutVM.tokenRole == RoleTypeSM.Unknown ||
      Number(RoleTypeSM[role]) == RoleTypeSM.Unknown
    )
      return true;
    return this._commonService.layoutVM.tokenRole == Number(RoleTypeSM[role]);
  }
  /**
   * Get All Licenses
   */

  override async loadPageData(): Promise<void> {
    try {
      await this._commonService.presentLoading();
      // Access the company ID from the common service
      let resp = await this.licenseInfoService.getALLlicenseTypesExtended();
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.LicenseTypeList = resp.successData;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
      console.log("load page data");
    }
  }

  async getActiveLicense() {
    try {
      await this._commonService.presentLoading();
      // let resp: ApiResponse<UserLicenseDetailsSM>;
      this.viewModel.checkValidUser = await this.authGuard.IsTokenValid();
      if (this.viewModel.checkValidUser) {
        let resp =
          await this.licenseInfoService.getUserMineActiveLicenseInfo();
        if (resp.isError) {
          this._exceptionHandler.logObject(resp.errorData);
          this._commonService.showSweetAlertToast({
            title: resp.errorData.displayMessage,
            icon: "error",
          });
          return;
        } else {
          this.viewModel.activeLicense = resp.successData;
          if (resp.successData == null) {
            this._commonService.layoutVM.toogleWrapper = "loginWrapper";
            this._commonService.layoutVM.showLeftSideMenu=false;
           this.viewModel.activeLicense.licenseTypeId==null
            return;
          } else if (resp.successData.status == "canceled" && resp.successData.isCancelled == true) {
            this.viewModel.buyBtn;
            this.viewModel.checkActiveLicense = false;
            return;
          } else if (!resp.successData.isSuspended && resp.successData.status == "active" && resp.successData.isCancelled == false) {
            this.viewModel.buyBtn = "Upgrade";
            this.viewModel.checkActiveLicense = true;
          } else if ( !resp.successData.isSuspended && resp.successData.status === "active" &&resp.successData.isCancelled === true
          ) {
            this.viewModel.purchasedBtn = "Renew";
            this.viewModel.buyBtn = "Upgrade";
            this.viewModel.checkActiveLicense = true;
          } else if (
            !resp.successData.isSuspended &&
            resp.successData.status === "past_due" &&
            resp.successData.isCancelled === false
          ) {
            this.viewModel.purchasedBtn;
            this.viewModel.buyBtn = "Upgrade";
            this.viewModel.checkActiveLicense = true;
          } else if (
            !resp.successData.isSuspended &&
            resp.successData.status === "incomplete" &&
            resp.successData.isCancelled === false
          ) {
            this.viewModel.purchasedBtn;
            this.viewModel.buyBtn = "Upgrade";
            this.viewModel.checkActiveLicense = true;
          }
        }
      } else {
        return;
      }
    } catch (error) {
      // this._commonService.showSweetAlertToast({
      //   title: AppConstants.ErrorPrompts.Unknown_Error,
      //   icon: "error",
      // });
      throw error;
    } finally {
      await this._commonService.dismissLoader();
      console.log("active license");
    }
  }
  async payNow(stripePriceId: string, priceId: number) {
    try {
      await this._commonService.presentLoading();
      if (stripePriceId == "0000") {
        let respOftTrialLicense =
          await this.licenseInfoService.AddTrialLicense();
        if (respOftTrialLicense.isError) {
          await this._exceptionHandler.logObject(respOftTrialLicense.errorData);
          this._commonService.showSweetAlertToast({
            title: respOftTrialLicense.errorData.displayMessage,
            icon: "error",
          });
          return;
        } else {
          await this.accountService.logoutUser();
          return;
        }
      } else {
        const now = new Date(); // Get the current date and time
        const formattedDate = format(now, "yyyy-MM-dd HH:mm:ss"); // Customize the format as needed
        // Parse the formatted date string back to a Date object
        const parsedDate = parse(
          formattedDate,
          "yyyy-MM-dd HH:mm:ss",
          new Date()
        );

        let urlInfo: PaymentStatusInfo = {
          licenseId: priceId,
          paymentDate: parsedDate,
        };
        let baseUrl = window.location.href.substring(
          0,
          window.location.href.indexOf(AppConstants.WebRoutes.LICENSE)
        );
        let urlText = btoa(
          this._commonService.encrypt(JSON.stringify(urlInfo))
        );
        let resp = await this.licenseInfoService.GenerateCheckoutSessionDetails(
          {
            priceId: stripePriceId,
            failureUrl: `${baseUrl}/${AppConstants.WebRoutes.PAYMENT_FAIL_URL}/${urlText}`,
            successUrl: `${baseUrl}/${AppConstants.WebRoutes.PAYMENT_SUCCESS_URL}/${urlText}`,
            paymentMode: PaymentModeSM.CreditCard,
            productId: "prod_OqyE12m0oRgb4K",
          }
        );
        if (resp.isError) {
          await this._exceptionHandler.logObject(resp.errorData);
          this._commonService.showSweetAlertToast({
            title: resp.errorData.displayMessage,
            icon: "error",
          });
          return;
        } else {
          await this.redirectToCheckout(resp.successData);
          return;
        }
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
    // let urlText = btoa(this._commonService.encrypt(JSON.stringify(urlInfo)));
  }
  async redirectToCheckout(successData: CheckoutSessionResponseSM) {
    const stripe = Stripe(successData.publicKey);
    await stripe.redirectToCheckout({
      sessionId: successData.sessionId,
    });
  }
  /**
   * Renew License Type
   * @returns
   */
  async click_Renew() {
    if (this.viewModel.purchasedBtn !== "Renew") {
      return;
    }
    try {
      await this._commonService.presentLoading();
      let resp = await this.licenseInfoService.ManageSubscription(
        window.location.href
      );
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: resp.errorData.displayMessage,
          icon: "error",
        });
        return;
      } else {
        window.location.href = resp.successData.url;
      }
    } catch (error) {
      this._commonService.showSweetAlertToast({
        title: AppConstants.ErrorPrompts.Unknown_Error,
        icon: "error",
      });
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
}
