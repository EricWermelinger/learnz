import { Component } from '@angular/core';
import { DrawSettingsService } from './draw-settings.service';

@Component({
  selector: 'app-draw-settings',
  templateUrl: './draw-settings.component.html',
  styleUrls: ['./draw-settings.component.scss']
})
export class DrawSettingsComponent {

  constructor(
    private drawSettingsService: DrawSettingsService,
  ) { }

}