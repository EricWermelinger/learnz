import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/internal/Observable';
import { appRoutes } from 'src/app/Config/appRoutes';
import { TestSettingDTO } from 'src/app/DTOs/Test/TestSettingDTO';
import { TestSettingsService } from './test-settings.service';

@Component({
  selector: 'app-test-settings',
  templateUrl: './test-settings.component.html',
  styleUrls: ['./test-settings.component.scss']
})
export class TestSettingsComponent {

  settings$: Observable<TestSettingDTO>;
  testId: string;

  constructor(
    private settingsService: TestSettingsService,
    private activatedRoute: ActivatedRoute,
  ) {
    this.testId = this.activatedRoute.snapshot.paramMap.get(appRoutes.TestId) ?? '';
    this.settings$ = this.settingsService.getSettings$(this.testId);
  }

  saveSettings(settings: TestSettingDTO) {
    this.settingsService.setSettings$(settings);
  }
}
