import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { appRoutes } from 'src/app/Config/appRoutes';
import { TestAdjustUserPointDTO } from 'src/app/DTOs/Test/TestAdjustUserPointDTO';
import { TestResultDTO } from 'src/app/DTOs/Test/TestResultDTO';
import { TestResultService } from './test-result.service';

@Component({
  selector: 'app-test-result',
  templateUrl: './test-result.component.html',
  styleUrls: ['./test-result.component.scss']
})
export class TestResultComponent {

  testOfUserId: string;
  userId: string | null;
  result$: Observable<TestResultDTO>;

  constructor(
    private resultService: TestResultService,
    private activatedRoute: ActivatedRoute,
  ) {
    this.testOfUserId = this.activatedRoute.snapshot.paramMap.get(appRoutes.TestId) ?? '';
    this.userId = this.activatedRoute.snapshot.paramMap.get(appRoutes.TestUserId) ?? null;
    this.result$ = this.resultService.getResult$(this.testOfUserId, this.userId);
  }

  adjustUserPoints(questionId: string, isCorrect: boolean, pointsScored: number) {
    const value = {
      testId: this.testOfUserId,
      userId: this.userId ?? '',
      questionId,
      isCorrect,
      pointsScored,
    } as TestAdjustUserPointDTO;
    this.resultService.adjustUserPoints$(value).subscribe();
  }
}
