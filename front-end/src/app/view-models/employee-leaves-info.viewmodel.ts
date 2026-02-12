import { ClientCompanyDetailSM } from "../service-models/app/v1/client/client-company-detail-s-m";
import { ClientEmployeeLeaveExtendedUserSM } from "../service-models/app/v1/client/client-employee-leave-extended-user-s-m";
import { ClientEmployeeLeaveSM } from "../service-models/app/v1/client/client-employee-leave-s-m";
import { PermissionSM } from "../service-models/app/v1/client/permission-s-m";
import { BaseViewModel } from "./base.viewmodel";
export class EmployeeLeavesInfoViewModel extends BaseViewModel {
    override  PageTitle: string = 'Employee-Leaves-Info';
    showTooltip: boolean = false;
    employeeLeaveExtended: ClientEmployeeLeaveExtendedUserSM = new ClientEmployeeLeaveExtendedUserSM();
    showApproveBtn:boolean=true;
    isReadOnly:boolean=true;
    companyEmployeeLeavesListExtended: ClientEmployeeLeaveExtendedUserSM[] = [];
    employeeLeave: ClientEmployeeLeaveSM = new ClientEmployeeLeaveSM();
    employeeLeavesList: ClientEmployeeLeaveSM[] = [];
    company: ClientCompanyDetailSM = new ClientCompanyDetailSM()
    isAddMode: boolean = false;
    disabled: boolean = false;
    leaveTypes = new Array<string>();
    employeeLeavesCount!: number;
    totalLeavesCount!: number;
    currentDate=new Date();
    displayStyle = "none";
    formSubmitted: boolean = false;
    validations = {
        leaveType: [
            { type: 'required', message: 'Select Type' },
            { type: 'minlength', message: 'Minimum Length is 3 Characters' },
        ],
        leaveDateFromUTC: [
            { type: 'required', message: 'This field is required' },
            { type: 'minlength', message: 'Minimum Length is 3 Characters' },
        ],
        leaveDateToUTC: [
            { type: 'required', message: 'This field is required' },
            { type: 'minlength', message: 'Minimum Length is 3 Characters' },
        ],
        employeeComment: [
            { type: 'minlength', value: 5, message: 'Minimum Length is 5 Characters !' },
            { type: 'maxlength', value: 50, message: 'Maximum Length is 50 Characters !' },
            { type: "pattern", message: "Not Valid Format !" }
        ]
    }
}