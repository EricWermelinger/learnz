import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { appRoutes } from 'src/app/Config/appRoutes';
import { TestAdjustUserPointDTO } from 'src/app/DTOs/Test/TestAdjustUserPointDTO';
import { TestResultDTO } from 'src/app/DTOs/Test/TestResultDTO';
import { getQuestionTypes } from 'src/app/Enums/QuestionType';
import { getSubjects } from 'src/app/Enums/Subject';
import { TestEditPointsComponent } from '../test-edit-points/test-edit-points.component';
import { TestResultService } from './test-result.service';

@Component({
  selector: 'app-test-result',
  templateUrl: './test-result.component.html',
  styleUrls: ['./test-result.component.scss']
})
export class TestResultComponent {

  testOfUserId: string;
  userId: string | null;
  pointsAdjustable: boolean;
  result$: Observable<TestResultDTO>;

  constructor(
    private resultService: TestResultService,
    private activatedRoute: ActivatedRoute,
    private dialog: MatDialog,
  ) {
    this.testOfUserId = this.activatedRoute.snapshot.paramMap.get(appRoutes.TestId) ?? '';
    this.userId = this.activatedRoute.snapshot.paramMap.get(appRoutes.TestUserId) ?? null;
    this.pointsAdjustable = !!this.userId;
    this.result$ = this.resultService.getResult$(this.testOfUserId, this.userId);
  }

  translateSubject(subject: number) {
    return 'Subject.' + getSubjects().filter(s => s.value === subject)[0].key;
  }

  calculatePercentage(scored: number, possible: number) {
    if (possible === 0) {
      return 0;
    }
    return Math.round(scored / possible * 100 * 100) / 100;
  }

  translateSubjectFromString(subject: string) {
    const filtered = getSubjects().filter(s => s.value === parseInt(subject));
    if (filtered.length === 0) {
      return '';
    }
    return 'Subject.' + filtered[0].key;
  }

  getQuestionType(type: number) {
    return getQuestionTypes().filter(t => t.value === type)[0].key;
  }

  editPoints(questionId: string, isRight: boolean, pointsScored: number) {
    this.dialog.open(TestEditPointsComponent, {
      data: {
        testOfUserId: this.testOfUserId,
        userId: this.userId,
        questionId,
        isCorrect: isRight,
        pointsScored,
      }
    });
  }
}