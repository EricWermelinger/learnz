import { Component } from '@angular/core';
import { TogetherSwipeService } from './together-swipe.service';

@Component({
  selector: 'app-together-swipe',
  templateUrl: './together-swipe.component.html',
  styleUrls: ['./together-swipe.component.scss']
})
export class TogetherSwipeComponent {

  constructor(
    private swipeService: TogetherSwipeService,
  ) { }

}
