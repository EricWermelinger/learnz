import { Component } from '@angular/core';
import { TogetherService } from './together.service';

@Component({
  selector: 'app-together',
  templateUrl: './together.component.html',
  styleUrls: ['./together.component.scss']
})
export class TogetherComponent {

  constructor(
    private togetherService: TogetherService,
  ) { }

}
