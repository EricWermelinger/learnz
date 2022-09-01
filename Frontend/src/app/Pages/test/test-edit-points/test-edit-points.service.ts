import { Injectable } from '@angular/core';
import { endpoints } from 'src/app/Config/endpoints';
import { TestAdjustUserPointDTO } from 'src/app/DTOs/Test/TestAdjustUserPointDTO';
import { ApiService } from 'src/app/Framework/API/api.service';

@Injectable({
  providedIn: 'root'
})
export class TestEditPointsService {

  constructor(
    private api: ApiService,
  ) { }

  adjustUserPoints$(value: TestAdjustUserPointDTO) {
    return this.api.callApi(endpoints.TestGroupAdjustUserPoints, value, 'POST');
  }
}
