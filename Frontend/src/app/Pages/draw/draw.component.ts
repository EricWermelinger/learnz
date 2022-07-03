import { Component } from '@angular/core';
import { DrawService } from './draw.service';

@Component({
  selector: 'app-draw',
  templateUrl: './draw.component.html',
  styleUrls: ['./draw.component.scss']
})
export class DrawComponent {

  constructor(
    private drawService: DrawService,
  ) { }

}
