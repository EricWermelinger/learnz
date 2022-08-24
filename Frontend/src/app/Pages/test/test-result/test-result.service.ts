import { Injectable } from '@angular/core';
import { endpoints } from 'src/app/Config/endpoints';
import { TestAdjustUserPointDTO } from 'src/app/DTOs/Test/TestAdjustUserPointDTO';
import { TestResultDTO } from 'src/app/DTOs/Test/TestResultDTO';
import { ApiService } from 'src/app/Framework/API/api.service';

@Injectable({
  providedIn: 'root'
})
export class TestResultService {

  constructor(
    private api: ApiService,
  ) { }

  getResult$(testOfUserId: string, userId: string | null = null) {
    return this.api.callApi<TestResultDTO>(endpoints.TestQuestions, { testOfUserId, userId }, 'GET');
  }

  adjustUserPoints$(value: TestAdjustUserPointDTO) {
    return this.api.callApi(endpoints.TestGroupAdjustUserPoints, value, 'POST');
  }
}
