import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { endpoints } from 'src/app/Config/endpoints';
import { TogetherSwipeDTO } from 'src/app/DTOs/Together/TogetherSwipeDTO';
import { TogetherUserProfileDTO } from 'src/app/DTOs/Together/TogetherUserProfileDTO';
import { WebSocketService } from 'src/app/Framework/API/web-socket.service';
import { TogetherSwipeConnectedDialogComponent } from './together-swipe-connected-dialog/together-swipe-connected-dialog.component';
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
    private dialog: MatDialog,
    private ws: WebSocketService,
  ) {
    this.swipe$ = this.swipeService.getNextSwipe();
    this.swipeService.connectionOccured().subscribe(user => {
      this.openConnected(user);
    });
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
    return this.swipeService.translateSubject(subject);
  }
  
  translateGrade(grade: number) {
    return this.swipeService.translateGrade(grade);
  }

  openConnected(user: TogetherUserProfileDTO) {
    this.dialog.open(TogetherSwipeConnectedDialogComponent, {
      data: user,
    });
  }
}
