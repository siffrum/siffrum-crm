import { CoinManagementServiceModelBase } from '../../base/coin-management-service-model-base';
import { ClientUserAddressTypeSM } from '../../enums/client-user-address-type-s-m.enum';

export class ClientUserAddressSM extends CoinManagementServiceModelBase<number> {
    country!: string;
    state!: string;
    city!: string;
    address1!: string;
    address2!: string;
    pinCode!: string;
    clientUserAddressType!: ClientUserAddressTypeSM;
    clientUserId!: number;
    clientCompanyDetailId!: number;
}
