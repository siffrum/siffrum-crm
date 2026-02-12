import { Component } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { CommonService } from 'src/app/services/common.service';
@Component({
    selector: 'app-spinner',
    templateUrl: './spinner.component.html',
    styleUrls: ['./spinner.component.scss'],
    standalone: false
})
export class SpinnerComponent {
  protected _commonService: CommonService;
  constructor(commonService: CommonService, private spinner: NgxSpinnerService) {
    this._commonService = commonService;
    this.showSpinner();
  }
  async showSpinner() {
    await this.spinner.show();
  }
}
