import { Component, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { distinctUntilChanged, filter, map, Observable, Subject, takeUntil } from 'rxjs';
import { appRoutes } from 'src/app/Config/appRoutes';
import { ChallengeIdDTO } from 'src/app/DTOs/Challenge/ChallengeIdDTO';
import { ChallengePlayerResultDTO } from 'src/app/DTOs/Challenge/ChallengePlayerResultDTO';
import { GeneralQuestionAnswerDTO } from 'src/app/DTOs/GeneralQuestion/GeneralQuestionAnswerDTO';
import { GeneralQuestionQuestionDTO } from 'src/app/DTOs/GeneralQuestion/GeneralQuestionQuestionDTO';
import { ChallengeState, getChallengeStates } from 'src/app/Enums/ChallengeState';
import { ChallengeActiveService } from './challenge-active.service';

@Component({
  selector: 'app-challenge-active',
  templateUrl: './challenge-active.component.html',
  styleUrls: ['./challenge-active.component.scss']
})
export class ChallengeActiveComponent implements OnDestroy {

  challengeId: string;
  challengeName$: Observable<string>;
  result$: Observable<ChallengePlayerResultDTO[]>;
  isOwner$: Observable<boolean>;
  question$: Observable<GeneralQuestionQuestionDTO>;
  lastQuestionPoint$: Observable<number>;
  state$: Observable<ChallengeState>;
  private destroyed$ = new Subject<void>();

  constructor(
    private challengeActiveService: ChallengeActiveService,
    private activatedRoute: ActivatedRoute,
  ) {
    this.challengeId = this.activatedRoute.snapshot.paramMap.get(appRoutes.ChallengeId) ?? '';
    const challenge$ = this.challengeActiveService.getActiveChallenge(this.challengeId).pipe(takeUntil(this.destroyed$));
    this.challengeName$ = challenge$.pipe(map(chl => chl.name), distinctUntilChanged());
    this.result$ = challenge$.pipe(map(chl => chl.result), distinctUntilChanged());
    this.isOwner$ = challenge$.pipe(map(chl => chl.isOwner), distinctUntilChanged());
    this.question$ = challenge$.pipe(map(chl => chl.question), filter(qst => !!qst), map(qst => qst as GeneralQuestionQuestionDTO), distinctUntilChanged());
    this.lastQuestionPoint$ = challenge$.pipe(map(chl => chl.lastQuestionPoint), filter(pts => !!pts), map(pts => pts as number), distinctUntilChanged());
    this.state$ = challenge$.pipe(map(chl => this.getState(chl.state)), distinctUntilChanged());
    
    const cancelled$ = challenge$.pipe(map(chl => chl.cancelled), distinctUntilChanged(), filter(cnc => cnc === true));
    cancelled$.subscribe(_ => this.showCancelDialog());
  }

  challengeNextFlow() {
    this.challengeActiveService.challengeNextFlow({ challengeId: this.challengeId } as ChallengeIdDTO);
  }

  challengeAnswer(value: GeneralQuestionAnswerDTO) {
    this.challengeActiveService.challengeAnswer(value);
  }

  showCancelDialog() {
    // todo showCancelDialog
  }

  getState(state: number) {
    return getChallengeStates().filter(s => s.value === state)[0].key;
  }

  getLength(arr: any[]) {
    return arr.length;
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}