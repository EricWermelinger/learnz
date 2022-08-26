import { KeyValue } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject, first, map, Observable } from 'rxjs';
import { appRoutes } from 'src/app/Config/appRoutes';
import { GeneralQuestionAnswerDTO } from 'src/app/DTOs/GeneralQuestion/GeneralQuestionAnswerDTO';
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
  questions$ = new BehaviorSubject<TestQuestionDTO[]>([]);
  questionsChoice$: Observable<KeyValue<string, boolean>[]>;
  activeQuestion: TestQuestionDTO | undefined;

  constructor(
    private stepperService: TestStepperService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
  ) {
    this.testOfUserId = this.activatedRoute.snapshot.paramMap.get(appRoutes.TestId) ?? '';
    this.stepperService.getQuestions$(this.testOfUserId).subscribe(questions => this.questions$.next(questions));
    this.questionsChoice$ = this.questions$.pipe(
      map(questions => questions.map(question => {
        return {
          key: question.question.questionId,
          value: !!question.answer
        }
      }))
    );
    this.questions$.pipe(
      first(),
      map(questions => questions[0]),
    ).subscribe(question => this.activeQuestion = question);
  }

  answerQuestion(value: GeneralQuestionAnswerDTO) {
    this.questionAnswer(value.answer, this.activeQuestion!.question.questionId);
  }

  questionAnswer(answer: string, questionOfUserId: string) {
    const value = {
      answer,
      questionOfUserId,
    } as TestAnswerDTO;
    this.stepperService.questionAnswer$(value).subscribe(_ => {
      this.stepperService.getQuestions$(this.testOfUserId).subscribe(questions => {
        this.questions$.next(questions);
        const lastQuestionIndex = questions.findIndex(q => q.question.questionId == questionOfUserId);
        if (lastQuestionIndex === questions.length - 1) {
          this.activeQuestion = questions[0];
        } else {
          this.activeQuestion = questions[lastQuestionIndex + 1];
        }
      });
    });
  }

  endTest() {
    this.stepperService.endTest$(this.testOfUserId).subscribe(_ => {
      this.router.navigate([appRoutes.App, appRoutes.Test]);
    });
  }

  setQuestionActive(questionId: string) {
    const question = this.questions$.value.find(question => question.question.questionId === questionId);
    this.activeQuestion = question;
  }

  getQuestionNumber(questionId: string, choices: KeyValue<string, boolean>[]) {
    return choices.findIndex(choice => choice.key === questionId) + 1;
  }
}
