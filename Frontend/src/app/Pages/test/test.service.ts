import { Injectable } from '@angular/core';
import { endpoints } from 'src/app/Config/endpoints';
import { TestDTO } from 'src/app/DTOs/Test/TestDTO';
import { TestIdDTO } from 'src/app/DTOs/Test/TestIdDTO';
import { TestVisibilityDTO } from 'src/app/DTOs/Test/TestVisibilityDTO';
import { ApiService } from 'src/app/Framework/API/api.service';

@Injectable({
  providedIn: 'root'
})
export class TestService {

  constructor(
    private api: ApiService,
  ) { }

  openTests$() {
    return this.api.callApi<TestDTO[]>(endpoints.TestOpen, { }, 'GET');
  }

  closedTests$() {
    return this.api.callApi<TestDTO[]>(endpoints.TestClosed, { }, 'GET');
  }

  testStart$(testId: string) {
    const value = {
      testId
    } as TestIdDTO;
    return this.api.callApi<TestIdDTO>(endpoints.TestStart, value, 'POST');
  }

  testVisibility$(testId: string, visible: boolean) {
    const value = {
      testId,
      visible
    } as TestVisibilityDTO;
    this.api.callApi(endpoints.TestGroupTestVisibility, value, 'POST').subscribe();
  }
}
