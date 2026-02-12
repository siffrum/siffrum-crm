import { Component, OnInit } from "@angular/core";
import { CommonService } from "src/app/services/common.service";
import { CompanyListService } from "src/app/services/company-list.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { SuperCompanyListViewModel } from "src/app/view-models/admin-company-list.viewmodel";
import { BaseComponent } from "../../base.component";

@Component({
    selector: "app-company-list",
    templateUrl: "./company-list.component.html",
    styleUrls: ["./company-list.component.scss"],
    standalone: false
})
export class CompanyListComponent
  extends BaseComponent<SuperCompanyListViewModel>
  implements OnInit
{
  constructor(
    _commonService: CommonService,
    logService: LogHandlerService,
    private CompanylistService: CompanyListService,

  ) {
    super(_commonService, logService);
    this.viewModel = new SuperCompanyListViewModel();
  }
 async ngOnInit() {
    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;
    // await this.getTotalCountOfCompanies();
    await this.loadPageData();
  }
  /**
   * Get list Of  All Companies
   * It also implements pagination for the table
   * @param queryFilter
   * @returns successData
   */
  override async loadPageData() {
    try {
      await this._commonService.presentLoading();
      await this.getTotalCountOfCompanies();
      let resp = await this.CompanylistService.getAllCompaniesByOdata(this.viewModel
      );
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.clientCompanyDetaillist = resp.successData;
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }
  /**Get total Number of Comapanies.
   * @returns Count
   */
  async getTotalCountOfCompanies() {
    try {
      await this._commonService.presentLoading();
      let resp = await this.CompanylistService.getAllCompaniesCount();
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        this._commonService.showSweetAlertToast({
          title: 'Error!',
          text: resp.errorData.displayMessage,
          position: "top-end",
          icon: "error"
        });
      } else {
        this.viewModel.pagination.totalCount = resp.successData.intResponse;
      }
    } catch (error) {
    } finally {
      await this._commonService.dismissLoader();
    }
  }

  /**update Company Status
   * @params companyId and Boolean value
   * @return boolean
   * @developer Musaib
   */
  async updateCompanyStatus(event: any, id: number) {
    try {
      if (event.target.checked) {
        this.viewModel.isTrue = true;
      } else {
        this.viewModel.isTrue = false;
      }
      let resp = await this.CompanylistService.updateCompanyStatus(
        id,
        this.viewModel.isTrue
      );
      if (resp.isError) {
        await this._exceptionHandler.logObject(resp.errorData);
        await this._commonService.ShowToastAtTopEnd(
          resp.errorData.displayMessage,
          "error"
        );
      } else {
        await this.loadPageData();
        await this._commonService.ShowToastAtTopEnd(
          "Updated Successfully",
          "success"
        );
      }
    } catch (error) {
      throw error;
    } finally {
      await this._commonService.dismissLoader();
    }
  }

       /**this function is used to create an event for pagination */
      async loadPagedataWithPagination(pageNumber: number) {
        if (pageNumber && pageNumber > 0) {
          this.viewModel.pagination.PageNo = pageNumber;
          await this.loadPageData()

        }
      }

}
