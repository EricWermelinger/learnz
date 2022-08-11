import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs/internal/Observable';
import { ChallengeOpenDTO } from 'src/app/DTOs/Challenge/ChallengeOpenDTO';
import { ChallengeCreateDialogComponent } from './challenge-create-dialog/challenge-create-dialog.component';
import { ChallengeService } from './challenge.service';

@Component({
  selector: 'app-challenge',
  templateUrl: './challenge.component.html',
  styleUrls: ['./challenge.component.scss']
})
export class ChallengeComponent {

  openChallenges$: Observable<ChallengeOpenDTO[]>;

  constructor(
    private challengeService: ChallengeService,
    private dialog: MatDialog,
  ) {
    this.openChallenges$ = this.challengeService.getOpenChallenges();
  }

  joinChallenge(challengeId: string) {
    this.challengeService.joinChallenge(challengeId);
  }

  cancelChallenge(challengeId: string) {
    this.challengeService.cancelChallenge(challengeId);
  }

  createNewChallenge() {
    this.dialog.open(ChallengeCreateDialogComponent, {
      data: {
        setEditable: true,
      }
    });
  }

  isEmpty(value: any[]) {
    return value.length === 0;
  }
}