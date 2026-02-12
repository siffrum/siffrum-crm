import { PaginationViewModel } from "../internal-models/pagination.viewmodel";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";

export class BaseViewModel {
  PageTitle: string = "";
  Permission?: PermissionSM;
  pagination: PaginationViewModel = { PageNo: 1, PageSize: 10, totalCount: 0, totalPages: [] }
}
