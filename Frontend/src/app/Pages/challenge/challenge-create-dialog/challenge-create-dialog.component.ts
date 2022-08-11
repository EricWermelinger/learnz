import { Component } from '@angular/core';
import { ChallengeCreateDialogService } from './challenge-create-dialog.service';

@Component({
  selector: 'app-challenge-create-dialog',
  templateUrl: './challenge-create-dialog.component.html',
  styleUrls: ['./challenge-create-dialog.component.scss']
})
export class ChallengeCreateDialogComponent {

  constructor(
    private challengeCreateService: ChallengeCreateDialogService,
  ) { }

}