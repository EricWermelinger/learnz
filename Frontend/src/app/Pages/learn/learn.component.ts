import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { LearnSessionDTO } from 'src/app/DTOs/Learn/LearnSessionDTO';
import { LearnCreateDialogComponent } from './learn-create-dialog/learn-create-dialog.component';
import { LearnService } from './learn.service';

@Component({
  selector: 'app-learn',
  templateUrl: './learn.component.html',
  styleUrls: ['./learn.component.scss']
})
export class LearnComponent {

  openSessions$: Observable<LearnSessionDTO[]>;
  closedSessions$: Observable<LearnSessionDTO[]>;

  constructor(
    private learnService: LearnService,
    private dialog: MatDialog,
  ) {
    this.openSessions$ = this.learnService.getOpenSessions$();
    this.closedSessions$ = this.learnService.getClosedSessions$();
  }

  createNew() {
    this.dialog.open(LearnCreateDialogComponent, {
      data: {
        setEditable: true,
      }
    });
  }
}
