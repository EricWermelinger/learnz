import { Component } from '@angular/core';
import { TogetherAskService } from './together-ask.service';

@Component({
  selector: 'app-together-ask',
  templateUrl: './together-ask.component.html',
  styleUrls: ['./together-ask.component.scss']
})
export class TogetherAskComponent {

  constructor(
    private askService: TogetherAskService,
  ) { }

}
