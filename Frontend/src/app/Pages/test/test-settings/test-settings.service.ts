import { Injectable } from '@angular/core';
import { endpoints } from 'src/app/Config/endpoints';
import { TestSaveSettingsDTO } from 'src/app/DTOs/Test/TestSaveSettingsDTO';
import { TestSettingDTO } from 'src/app/DTOs/Test/TestSettingDTO';
import { ApiService } from 'src/app/Framework/API/api.service';

@Injectable({
  providedIn: 'root'
})
export class TestSettingsService {

  constructor(
    private api: ApiService,
  ) { }

  getSettings$(testId: string) {
    return this.api.callApi<TestSettingDTO>(endpoints.TestSettings, { testId }, 'GET');
  }

  setSettings$(value: TestSaveSettingsDTO) {
    return this.api.callApi(endpoints.TestSettings, value, 'POST');
  }
}
