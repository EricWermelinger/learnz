import { Injectable } from '@angular/core';
import { endpoints } from 'src/app/Config/endpoints';
import { TestResultOverviewDTO } from 'src/app/DTOs/Test/TestResultOverviewDTO';
import { ApiService } from 'src/app/Framework/API/api.service';

@Injectable({
  providedIn: 'root'
})
export class TestResultOverviewService {

  constructor(
    private api: ApiService,
  ) { }

  getResultOverview$(testId: string) {
    return this.api.callApi<TestResultOverviewDTO>(endpoints.TestResultOverview, { testId }, 'GET');
  }
}
