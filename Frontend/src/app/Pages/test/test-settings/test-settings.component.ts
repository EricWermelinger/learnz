import { Component } from '@angular/core';
import { TestSettingsService } from './test-settings.service';

@Component({
  selector: 'app-test-settings',
  templateUrl: './test-settings.component.html',
  styleUrls: ['./test-settings.component.scss']
})
export class TestSettingsComponent {

  constructor(
    private settingsService: TestSettingsService,
  ) { }

}
