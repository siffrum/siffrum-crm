import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';

export class ClientCompanyAttendanceShiftSM extends CoinManagementServiceModelBase<number> {
    shiftFrom!: Date;
    shiftTo!: Date;
    shiftName!: string;
    shiftDescription!: string;
    primaryOfficeGeoCoordinatesLocation!: string;
    allowedRaidus!: string;
    locationBased!: string;
    allowedIps!: string;
    clientCompanyDetailId!: number;
}
