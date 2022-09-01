import { Component } from '@angular/core';
import { DrawingService } from './drawing.service';

@Component({
  selector: 'app-drawing',
  templateUrl: './drawing.component.html',
  styleUrls: ['./drawing.component.scss']
})
export class DrawingComponent {

  constructor(
    private drawingService: DrawingService,
  ) { }

}