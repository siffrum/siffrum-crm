import { Injectable } from '@angular/core';
import { AppConstants } from 'src/app-constants';
import { DummyTeacherClient } from '../clients/dummy-teacher.client';
import { DummyTeacherSM } from '../service-models/app/v1/dummy-teacher-s-m';
import { ApiResponse } from '../service-models/foundation/api-contracts/base/api-response';
import { DeleteResponseRoot } from '../service-models/foundation/common-response/delete-response-root';
import { BaseService } from './base.service';

@Injectable({
  providedIn: 'root'
})
export class TeacherService extends BaseService {



  constructor(private teacherClient: DummyTeacherClient) {
    super();

  }

  async getAllTeachers(): Promise<ApiResponse<DummyTeacherSM[]>> {
    return await this.teacherClient.GetAllTeachers();
  }

  async getTeacherById(id: number): Promise<ApiResponse<DummyTeacherSM>> {
    if (id <= 0) {
      throw new Error(AppConstants.ErrorPrompts.Delete_Data_Error)
    }
    return await this.teacherClient.GetTeacherById(id);
  }
  async deleteTeacher(id: number): Promise<ApiResponse<DeleteResponseRoot>> {
    if (id <= 0) {
      throw new Error(AppConstants.ErrorPrompts.Delete_Data_Error)
    }
    return await this.teacherClient.DeleteTeacherById(id);
  }

}
