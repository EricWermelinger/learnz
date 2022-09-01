import { Injectable } from '@angular/core';
import { endpoints } from 'src/app/Config/endpoints';
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
    const queryParam = userId === null ? { testOfUserId } : { testOfUserId, userId };
    return this.api.callApi<TestResultDTO>(endpoints.TestResult, queryParam, 'GET');
  }
}
