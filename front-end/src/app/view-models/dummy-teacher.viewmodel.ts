import { DummyTeacherSM } from "../service-models/app/v1/dummy-teacher-s-m";
import { BaseViewModel } from "./base.viewmodel";

export class DummyTeacherViewModel extends BaseViewModel {
    override PageTitle: string = 'Dummy Teacher';
    AddEditModalTitle: string = '';
    teachers: DummyTeacherSM[] = [];
    singleTeacher!: DummyTeacherSM;

}