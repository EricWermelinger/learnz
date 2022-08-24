import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { appRoutes } from 'src/app/Config/appRoutes';
import { TestAnswerDTO } from 'src/app/DTOs/Test/TestAnswerDTO';
import { TestQuestionDTO } from 'src/app/DTOs/Test/TestQuestionDTO';
import { TestStepperService } from './test-stepper.service';

@Component({
  selector: 'app-test-stepper',
  templateUrl: './test-stepper.component.html',
  styleUrls: ['./test-stepper.component.scss']
})
export class TestStepperComponent {

  testOfUserId: string;
  questions$: Observable<TestQuestionDTO[]>;

  constructor(
    private stepperService: TestStepperService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
  ) {
    this.testOfUserId = this.activatedRoute.snapshot.paramMap.get(appRoutes.TestId) ?? '';
    this.questions$ = this.stepperService.getQuestions$(this.testOfUserId);
  }

  questionAnswer(answer: string, questionOfUserId: string) {
    const value = {
      answer,
      questionOfUserId,
    } as TestAnswerDTO;
    this.stepperService.questionAnswer$(value);
  }

  endTest() {
    this.stepperService.endTest$(this.testOfUserId).subscribe(_ => {
      this.router.navigate([appRoutes.App, appRoutes.Test]);
    });
  }
}
