import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { appRoutes } from 'src/app/Config/appRoutes';
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
    private dialog: MatDialogRef<TogetherSwipeConnectedDialogComponent>,
    private swipeService: TogetherSwipeService,
    private router: Router,
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

  navigateConnections() {
    this.dialog.close();
    this.router.navigate([appRoutes.App, appRoutes.TogetherConnect]);
  }
}
