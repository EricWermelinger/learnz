import { Component } from '@angular/core';
import { ChallengeService } from './challenge.service';

@Component({
  selector: 'app-challenge',
  templateUrl: './challenge.component.html',
  styleUrls: ['./challenge.component.scss']
})
export class ChallengeComponent {

  constructor(
    private challengeService: ChallengeService,
  ) { }

}
