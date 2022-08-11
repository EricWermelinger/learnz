import { Component, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable, Subject, takeUntil } from 'rxjs';
import { appRoutes } from 'src/app/Config/appRoutes';
import { ChallengeActiveDTO } from 'src/app/DTOs/Challenge/ChallengeActiveDTO';
import { ChallengeActiveService } from './challenge-active.service';

@Component({
  selector: 'app-challenge-active',
  templateUrl: './challenge-active.component.html',
  styleUrls: ['./challenge-active.component.scss']
})
export class ChallengeActiveComponent implements OnDestroy {

  challengeId: string;
  challenge$: Observable<ChallengeActiveDTO>;
  private destroyed$ = new Subject<void>();

  constructor(
    private challengeActiveService: ChallengeActiveService,
    private activatedRoute: ActivatedRoute,
  ) {
    this.challengeId = this.activatedRoute.snapshot.paramMap.get(appRoutes.ChallengeId) ?? '';
    this.challenge$ = this.challengeActiveService.getActiveChallenge(this.challengeId).pipe(takeUntil(this.destroyed$));
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}