import { Injectable } from '@angular/core';
import { endpoints } from 'src/app/Config/endpoints';
import { TestResultOverviewDTO } from 'src/app/DTOs/Test/TestResultOverviewDTO';
import { TestVisibilityDTO } from 'src/app/DTOs/Test/TestVisibilityDTO';
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
  
  changeVisibility$(testId: string, visible: boolean) {
    const value = {
      testId,
      visible
    } as TestVisibilityDTO;
    this.api.callApi(endpoints.TestVisibility, value, 'POST').subscribe();
  }
}
