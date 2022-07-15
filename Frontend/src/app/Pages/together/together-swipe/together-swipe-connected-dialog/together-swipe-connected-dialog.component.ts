import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TogetherUserProfileDTO } from 'src/app/DTOs/Together/TogetherUserProfileDTO';
import { TogetherSwipeService } from '../together-swipe.service';

@Component({
  selector: 'app-together-swipe-connected-dialog',
  templateUrl: './together-swipe-connected-dialog.component.html',
  styleUrls: ['./together-swipe-connected-dialog.component.scss']
})
export class TogetherSwipeConnectedDialogComponent {

  constructor(
    @Inject(MAT_DIALOG_DATA) private data: TogetherUserProfileDTO,
    private swipeService: TogetherSwipeService,
  ) { }

  getData() {
    return this.data;
  }

  translateSubject(subject: number) {
    return this.swipeService.translateSubject(subject);
  }
  
  translateGrade(grade: number) {
    return this.swipeService.translateGrade(grade);
  }

}
