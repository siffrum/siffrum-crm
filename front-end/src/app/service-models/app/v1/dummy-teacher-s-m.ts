import { CoinManagementServiceModelBase } from '../base/coin-management-service-model-base';

export class DummyTeacherSM extends CoinManagementServiceModelBase<number> {
    firstName!: string;
    lastName!: string;
    emailAddress!: string;
    profilePictureFileId?: number;
}
