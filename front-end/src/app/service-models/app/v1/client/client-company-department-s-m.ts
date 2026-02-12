import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';

export class ClientCompanyDepartmentSM extends CoinManagementServiceModelBase<number> {
    departmentName!: string;
    departmenntLocation!: string;
    departmentDescription!: string;
    departmentCode!: string;
    departmentContact!: string;
    departmentManager!: string;
    clientCompanyDetailId!: number;
}
