import { CoinManagementServiceModelBase } from '../base/coin-management-service-model-base';

export class DummySubjectSM extends CoinManagementServiceModelBase<number> {
    subjectName!: string;
    subjectCode!: string;
    dummyTeacherID?: number;
}
