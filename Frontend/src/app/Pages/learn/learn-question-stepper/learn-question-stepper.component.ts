import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { appRoutes } from 'src/app/Config/appRoutes';
import { LearnAnswerDTO } from 'src/app/DTOs/Learn/LearnAnswerDTO';
import { LearnMarkQuestionDTO } from 'src/app/DTOs/Learn/LearnMarkQuestionDTO';
import { LearnQuestionDTO } from 'src/app/DTOs/Learn/LearnQuestionDTO';
import { LearnQuestionSetCorrectDTO } from 'src/app/DTOs/Learn/LearnQuestionSetCorrectDTO';
import { LearnSolutionDTO } from 'src/app/DTOs/Learn/LearnSolutionDTO';
import { LearnQuestionStepperService } from './learn-question-stepper.service';

@Component({
  selector: 'app-learn-question-stepper',
  templateUrl: './learn-question-stepper.component.html',
  styleUrls: ['./learn-question-stepper.component.scss']
})
export class LearnQuestionStepperComponent {

  learnSessionId: string;
  questions$ = new BehaviorSubject<LearnQuestionDTO[]>([]);
  activeSolution$: Observable<LearnSolutionDTO> | undefined;
  
  constructor(
    private questionStepperService: LearnQuestionStepperService,
    private activatedRoute: ActivatedRoute,
  ) {
    this.learnSessionId = this.activatedRoute.snapshot.paramMap.get(appRoutes.LearnId) ?? '';
    this.questionStepperService.getQuestions$(this.learnSessionId).subscribe(questions => this.questions$.next(questions));
  }

  markQuestion(questionId: string) {
    const filtered = this.questions$.value.filter(q => q.question.questionId === questionId);
    const hard = filtered.length > 0 && !filtered[0].markedAsHard;
    const value = {
      learnSessionId: this.learnSessionId,
      questionId,
      hard,
    } as LearnMarkQuestionDTO;
    this.questionStepperService.markQuestion$(value);
  }

  cardAnswer(questionId: string) {
    this.activeSolution$ = this.questionStepperService.cardAnswe$(this.learnSessionId, questionId);
  }

  writeSolution(questionId: string) {
    this.activeSolution$ = this.questionStepperService.writeSolution$(this.learnSessionId, questionId);
  }

  writeAnswer(questionId: string, answer: string) {
    const value = {
      learnSessionId: this.learnSessionId,
      questionId,
      answer,
    } as LearnAnswerDTO;
    this.questionStepperService.writeAnswer$(value).subscribe(_ => this.writeSolution(questionId));
  }

  changeCorrectIncorrect(questionId: string, correct: boolean) {
    const value = {
      learnSessionId: this.learnSessionId,
      questionId,
      correct,
    } as LearnQuestionSetCorrectDTO;
    this.questionStepperService.changeCorrectIncorrect$(value);
  }
}