import { Injectable } from '@angular/core';
import { endpoints } from 'src/app/Config/endpoints';
import { TestIdDTO } from 'src/app/DTOs/Test/TestIdDTO';
import { ApiService } from 'src/app/Framework/API/api.service';

@Injectable({
  providedIn: 'root'
})
export class TestEndConfirmService {

  constructor(
    private api: ApiService,
  ) { }

  endTest$(testOfUserId: string) {
    const value = {
      testId: testOfUserId,
    } as TestIdDTO;
    return this.api.callApi(endpoints.TestEnd, value, 'POST');
  }
}
