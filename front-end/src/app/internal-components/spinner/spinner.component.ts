import { Component } from "@angular/core";
import { CommonService } from "src/app/services/common.service";

@Component({
  selector: "app-spinner",
  templateUrl: "./spinner.component.html",
  styleUrls: ["./spinner.component.scss"],
  standalone: false,
})
export class SpinnerComponent {
  constructor(public _commonService: CommonService) {}
}