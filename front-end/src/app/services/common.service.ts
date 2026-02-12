import { Injectable } from "@angular/core";
import { Observable, ReplaySubject } from "rxjs";
import {
  AddCompanyWizards,
  LayoutViewModel,
  LoaderInfo,
  ToastInfo,
} from "../internal-models/common-models";
import { BaseService } from "./base.service";
import { StorageService } from "./storage.service";
import { AppConstants } from "src/app-constants";
import { SettingClient } from "../clients/setting.client";
import {
  SweetAlertIcon,
  SweetAlertOptions,
} from "../internal-models/custom-sweet-alert-options";
declare var Swal: any;

@Injectable({
  providedIn: "root",
})
export class CommonService extends BaseService {
  toasts: ToastInfo[] = [];
  public loaderInfo: LoaderInfo = { message: "", showLoader: false };
  layoutVM: LayoutViewModel = new LayoutViewModel();
  layoutViewModel: any;
  constructor(
    private storageService: StorageService,
    private settingClient: SettingClient
  ) {
    super();
  }

  // Show information in Modal popup
  // async showInfoInModalPopup(
  //   icon: SweetAlertIcon,
  //   title: string,
  //   text?: string
  // ) {
  //   Swal.fire({
  //     position: "top-end",
  //     icon: icon,
  //     title: title,
  //     text: text,
  //     showConfirmButton: false,
  //     timer: 3000,
  //   });
  // }
  // async presentForgotPasswordModal(redirectToPage: string) {
  //   this.modalService
  //     .open(ForgotpasswordComponent, {
  //       fullscreen: "lg",
  //       size: "xl",
  //       windowClass: "modal-holder",
  //       centered: true,
  //     })
  //     .result.then(
  //       (result) => {},
  //       (reason) => {
  //         if (
  //           reason === ModalDismissReasons.ESC ||
  //           reason === ModalDismissReasons.BACKDROP_CLICK
  //         ) {
  //           window.location.reload();
  //         }
  //         reason.componentInstance.redirectToPage = redirectToPage;
  //       }
  //     );
  // }

  // Convert Enum into Array
  EnumToStringArray(enumme: any) {
    const StringIsNumber = (value: any) => isNaN(Number(value)) === false;
    return Object.keys(enumme)
      .filter(StringIsNumber)
      .map((key) => enumme[key]);
  }
  /**Get Company Id From Session/Local Storage */
  async getCompanyIdFromStorage() {
    let remMe: boolean = await this.storageService.getFromStorage(
      AppConstants.DbKeys.REMEMBER_ME
    );
    if (remMe && remMe == true) {
      this.layoutVM.company.id = await this.storageService.getFromStorage(
        AppConstants.DbKeys.CLIENT_COMPANY_ID
      );
    } else {
      this.layoutVM.company.id =
        await this.storageService.getFromSessionStorage(
          AppConstants.DbKeys.CLIENT_COMPANY_ID
        );
    }
  }

  // Convert Single Enum into Array
  SingleEnumToString(enumme: any, enumVal: any) {
    const StringIsNumber = (value: any) => isNaN(Number(value)) === false;
    var x = Object.keys(enumme)
      .filter(StringIsNumber)
      .map((key) => enumme[key])[enumVal];
    return x;
  }

  /** Get Date ISO String from Month & Year String */
  getISODateFromMonthYear(selectedDate: string): any {
    return new Date(`29-${selectedDate}`).toISOString();
  }

  /** Get Date ISO String from Year String */
  getISODateFromYear(selectedDate: number): any {
    return new Date(`04-10-${selectedDate}`).toISOString();
  }

  async presentLoading(message: string = "") {
    this.loaderInfo = { message, showLoader: true };
  }

  // async presentToast(toastInfo: ToastInfo) {
  //   await this.showSweetAlertToast({
  //     title: toastInfo.body,
  //     icon: "error",

  //   });
  //   // this.toasts.push(toastInfo);
  // }

  // removeToast(toast: any) {
  //   this.toasts = this.toasts.filter((t) => t !== toast);
  // }

  // clearAllToasts() {
  //   this.toasts.splice(0, this.toasts.length);
  // }

  // async presentAlert() { }

  // async presentConfirmAlert(modalInfo: ConfirmModalInfo): Promise<boolean> {
  //   const modalRef = this.modalService.open(ConfirmModalComponent);
  //   modalRef.componentInstance.confirmModalInfo = modalInfo;
  //   return await modalRef.result;
  // }

  async dismissLoader() {
    this.loaderInfo.showLoader = false;
    this.loaderInfo.message = "";
    // console.log("loader executed");
  }
  async nextTablocations(wizardLocation: AddCompanyWizards) {
    switch (wizardLocation) {
      case AddCompanyWizards.addCompanyInfo:
        break;
      case AddCompanyWizards.addCompanyAddress:
        break;
      case AddCompanyWizards.addModules:
        break;
      case AddCompanyWizards.addCompanyAdminDetails:
      default:
        break;
    }
    this.layoutVM.wizardLocation = wizardLocation;
  }

  /**Show custom sweet alert*/
  async showSweetAlert(alertOptions: SweetAlertOptions) {
    // alertOptions.toast = false;
    return (await Swal.fire(alertOptions)).isConfirmed;
  }

  // shows confirmation alert where user would select yes or no
  async showConfirmationAlert(
    title: string,
    text: string,
    showCancelButton: boolean,
    icon: SweetAlertIcon
  ) {
    let customClass = {
      container: "container-modifier",
      confirmButton: "btn btn-success m-3",
      cancelButton: "btn btn-danger m-3",
      popup: "...",
      title: "...",
      closeButton: "...",
      icon: icon,
      image: "...",
      htmlContainer: "...",
      input: "...",
      validationMessage: "...",
      actions: "...",
      denyButton: "...",
      loader: "...",
      footer: "....",
      timerProgressBar: "....",
    };
    return await this.showSweetAlert({
      customClass,
      title,
      text,
      showCancelButton,
      icon,
      buttonsStyling: false,
      confirmButtonText: "Yes, delete it!",
      cancelButtonText: "No, cancel!",
    });
    // const swalWithBootstrapButtons = Swal.mixin({
    //   customClass: {
    //     container: "container-modifier",
    //     confirmButton: "btn btn-success m-3",
    //     cancelButton: "btn btn-danger m-3",
    //     popup: "...",
    //     title: "...",
    //     closeButton: "...",
    //     icon: icon,
    //     image: "...",
    //     htmlContainer: "...",
    //     input: "...",
    //     validationMessage: "...",
    //     actions: "...",
    //     denyButton: "...",
    //     loader: "...",
    //     footer: "....",
    //     timerProgressBar: "....",
    //   },
    //   buttonsStyling: false,
    // });
    // let x = await swalWithBootstrapButtons.fire({
    //   title: title,
    //   text: text,
    //   icon: icon,
    //   showCancelButton: showCancelButton,
    //   confirmButtonText: "Yes, delete it!",
    //   cancelButtonText: "No, cancel!",
    // });
    // return x.isConfirmed;
  }
  /**Show custom sweet alert*/
  async showSweetAlertToast(alertOptions: SweetAlertOptions) {
    alertOptions.toast = true;

    if (!alertOptions.position) alertOptions.position = "bottom";
    if (!alertOptions.showConfirmButton) alertOptions.showConfirmButton = false;
    if (!alertOptions.timer) alertOptions.timer = 3000;
    if (!alertOptions.timerProgressBar) alertOptions.timerProgressBar = true;
    alertOptions.didOpen = (toast) => {
      toast.addEventListener("mouseenter", Swal.stopTimer);
      toast.addEventListener("mouseleave", Swal.resumeTimer);
    };
    return await Swal.fire(alertOptions);
  }
  // shows information on alert window
  async showInfoOnAlertWindowPopup(
    icon: SweetAlertIcon,
    title: string,
    text: string
  ) {
    let customClass = {
      container: "container-modifier",
      confirmButton: "btn btn-success m-3",
      cancelButton: "btn btn-danger m-3",
      popup: "...",
      title: "...",
      closeButton: "...",
      icon: icon,
      image: "...",
      htmlContainer: "...",
      input: "...",
      validationMessage: "...",
      actions: "...",
      denyButton: "...",
      loader: "...",
      footer: "....",
      timerProgressBar: "....",
    };
    return await this.showSweetAlert({
      customClass,
      title,
      text,
      icon,
      showClass: {
        popup: "animate__animated animate__fadeInDown",
      },
      html: `<i class="${icon}"></i>${text}`,
      hideClass: {
        popup: "animate__animated animate__fadeOutUp",
      },
    });
    // const swal = Swal.mixin({
    //   customClass: {
    //     container: "container-modifier",
    //     popup: "...",
    //     title: title,
    //     icon: icon,
    //     image: "...",
    //     htmlContainer: "...",
    //     input: "...",
    //     validationMessage: "...",
    //     actions: "...",
    //     denyButton: "...",
    //     loader: "...",
    //     footer: "....",
    //     timerProgressBar: "....",
    //   },
    // });
    // let x = swal.fire({
    //   icon: icon,
    //   title: title,
    //   text: text,
    //   showClass: {
    //     popup: "animate__animated animate__fadeInDown",
    //   },
    //   html: `<i class="${icon}"></i>${text}`,
    //   hideClass: {
    //     popup: "animate__animated animate__fadeOutUp",
    //   },
    // });
    // return x;
  }

  // shows Toast Information at Top End
  // this function is not required, all the option for sweet alert toast can be passed to showSweetAlertToast
  async ShowToastAtTopEnd(title: string, icon: SweetAlertIcon) {
    this.showSweetAlertToast({ title, icon });
    // const Toast = Swal.mixin({
    //   toast: true,
    //   position: "bottom",
    //   showConfirmButton: false,
    //   timer: 2500,
    //   timerProgressBar: true,
    //   // icon: icon,
    //   didOpen: (toast: any) => {
    //     toast.addEventListener("mouseenter", Swal.stopTimer);
    //     toast.addEventListener("mouseleave", Swal.resumeTimer);
    //   },
    // });
    // Toast.fire({
    //   title: title,
    //   icon: icon,
    // });
  }

  // shows information on alert window
  async showDeleteInfo(icon: SweetAlertIcon, text: string) {
    const swal = Swal.mixin({
      customClass: {
        container: "container-modifier",
        popup: "...",
        // title: title,
        icon: icon,
        image: "...",
        htmlContainer: "...",
        input: "...",
        validationMessage: "...",
        actions: "...",
        denyButton: "...",
        loader: "...",
        footer: "....",
        timerProgressBar: "....",
      },
    });
    let x = swal.fire({
      icon: icon,
      // title: title,
      text: text,
      showClass: {
        popup: "animate__animated animate__fadeInDown",
      },
      hideClass: {
        popup: "animate__animated animate__fadeOutUp",
      },
    });
    return x;
  }

  // Convert From Base64 to File Extension
  public convertFromBase64ToPDF(b64Data: string, contentType: any) {
    contentType = contentType || "";
    let sliceSize = 512;
    var byteCharacters = atob(b64Data);
    var byteArrays = [];
    for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
      var slice = byteCharacters.slice(offset, offset + sliceSize);
      var byteNumbers = new Array(slice.length);
      for (var i = 0; i < slice.length; i++) {
        byteNumbers[i] = slice.charCodeAt(i);
      }
      var byteArray = new Uint8Array(byteNumbers);
      byteArrays.push(byteArray);
    }
    var blob = new Blob(byteArrays, { type: contentType });
    return blob;
  }

  // Convert Doc to Base64
  convertFileToBase64(file: File): Observable<string> {
    const result = new ReplaySubject<string>(1);
    const reader = new FileReader();
    reader.readAsBinaryString(file);
    reader.onload = (event: any) =>
      result.next(btoa(event.target.result.toString()));
    return result;
  }

  downloadDocument(
    base64data: string,
    letterName: string,
    fileExtension: string,
    employeeUserName?: string
  ) {
    var blob = this.convertFromBase64ToPDF(base64data, "application/docx");
    let a = document.createElement("a");
    document.body.appendChild(a);
    var url = window.URL.createObjectURL(blob);
    a.href = url;
    a.download = String(letterName + employeeUserName + "." + fileExtension);
    a.click();
    window.URL.revokeObjectURL(url);
    a.remove();
  }
  /**
   *Apply default Theme
   */
  async loadDefaultTheme() {
    try {
      await this.presentLoading();
      let resp = await this.settingClient.GetClientDefaultTheme();
      if (resp.isError) {
        this.showSweetAlertToast({
          title: "Error!",
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error",
        });
      } else {
        // Apply the theme using dynamically injected style
        const style = document.createElement("style");
        if (style) {
          style.remove();
        }

        style.id = "theme-style";
        style.innerHTML = `:root  ${resp.successData.css}`;
        document.head.appendChild(style);
      }
    } catch (error) {
      throw error;
    } finally {
      await this.dismissLoader();
    }
  }
  /**
   * Apply mine/Client Theme Gloabally
   */
  async applyThemeGlobally() {
    try {
      let resp = await this.settingClient.GetClientTheme();
      if (resp.isError) {
        // Handle the error case if needed
        this.showSweetAlertToast({
          title: "Error!",
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error",
        });
      } else {
        // Check if storedThemeResponse is usable before proceeding
        let style = document.getElementById("theme-style");
        // Remove existing theme style if it exists
        if (style) {
          style.remove();
        }
        // Create and inject the new theme style
        let newStyle = document.createElement("style");
        newStyle.id = "theme-style";
        newStyle.innerHTML = `:root ${resp.successData.css}`; // Assuming 'css' property holds the theme style
        document.head.appendChild(newStyle);
      }
    } catch (error) {
      throw error;
    } finally {
      await this.dismissLoader();
    }
  }
}
