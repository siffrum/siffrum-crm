import { Component, Input, OnInit } from "@angular/core";
import { CommonService } from "src/app/services/common.service";
import { LogHandlerService } from "src/app/services/log-handler.service";
import { LeavesViewmodel } from "src/app/view-models/leaves.viewmodel";
import { BaseComponent } from "../base.component";
import { AccountService } from "src/app/services/account.service";

@Component({
    selector: "app-leaves",
    templateUrl: "./leaves.component.html",
    styleUrls: ["./leaves.component.scss"],
    standalone: false
})

export class LeavesComponent extends BaseComponent<LeavesViewmodel> implements OnInit {

  @Input() isReadOnly!: boolean;
  @Input() isaddmode!: boolean;
  
  constructor(
    _commonService: CommonService,
    logService: LogHandlerService,
  ) {
    super(_commonService, logService);
    this.viewModel = new LeavesViewmodel();
  }


  ngOnInit() {
    this._commonService.layoutVM.PageTitle = this.viewModel.PageTitle;
  }
}
