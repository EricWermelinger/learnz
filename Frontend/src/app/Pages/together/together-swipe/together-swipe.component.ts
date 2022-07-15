import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { TogetherSwipeDTO } from 'src/app/DTOs/Together/TogetherSwipeDTO';
import { TogetherUserProfileDTO } from 'src/app/DTOs/Together/TogetherUserProfileDTO';
import { getGrades } from 'src/app/Enums/Grade';
import { getSubjects } from 'src/app/Enums/Subject';
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

  translateSubject(subject: number) {
    return 'Subject.' + getSubjects().filter(s => s.value === subject)[0].key;
  }
  
  translateGrade(grade: number) {
    return 'Grade.' + getGrades().filter(g => g.value === grade)[0].key;
  }
}
