import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { TogetherSwipeDTO } from 'src/app/DTOs/Together/TogetherSwipeDTO';
import { TogetherUserProfileDTO } from 'src/app/DTOs/Together/TogetherUserProfileDTO';
import { TogetherSwipeService } from './together-swipe.service';

@Component({
  selector: 'app-together-swipe',
  templateUrl: './together-swipe.component.html',
  styleUrls: ['./together-swipe.component.scss']
})
export class TogetherSwipeComponent {

  swipe$: Observable<TogetherUserProfileDTO>;

  constructor(
    private swipeService: TogetherSwipeService,
  ) {
    this.swipe$ = this.swipeService.getNextSwipe();
  }

  swipe(userId: string, choice: boolean) {
    const value = {
      userId,
      choice
    } as TogetherSwipeDTO;
    this.swipeService.swipe(value).subscribe(_ => {
      this.swipe$ = this.swipeService.getNextSwipe();
    });
  }
}
