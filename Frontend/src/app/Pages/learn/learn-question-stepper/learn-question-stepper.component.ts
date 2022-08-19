import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject, map, Observable, filter } from 'rxjs';
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
  activeQuestion$: Observable<LearnQuestionDTO>;
  activeSolution$: Observable<LearnSolutionDTO> | undefined;
  progress$: Observable<string>;
  showSolution$ = new BehaviorSubject<boolean>(false);
  showResult$: Observable<boolean>;

  constructor(
    private questionStepperService: LearnQuestionStepperService,
    private activatedRoute: ActivatedRoute,
  ) {
    this.learnSessionId = this.activatedRoute.snapshot.paramMap.get(appRoutes.LearnId) ?? '';
    this.questionStepperService.getQuestions$(this.learnSessionId).subscribe(questions => this.questions$.next(questions));
    this.showResult$ = this.questions$.asObservable().pipe(
      map(questions => questions.some(q => !q.answered))
    );
    this.activeQuestion$ = this.questions$.asObservable().pipe(
      map(questions => questions.filter(q => !q.answered)),
      filter(questions => questions.length > 0),
      map(questions => questions[0]),
    );
    this.progress$ = this.questions$.asObservable().pipe(
      map(questions => {
        return {
          answered: questions.filter(q => q.answered).length + 1,
          total: questions.length,
        }
      }),
      map(progress => `${progress.answered}/${progress.total}`),
    );
  }

  markQuestion(questionId: string) {
    const filtered = this.questions$.value.filter(q => q.question.questionId === questionId);
    const hard = filtered.length > 0 && !filtered[0].markedAsHard;
    const value = {
      learnSessionId: this.learnSessionId,
      questionId,
      hard,
    } as LearnMarkQuestionDTO;
    this.questionStepperService.markQuestion$(value).subscribe(_ => {
      const current = this.questions$.value;
      const next = current.map(q => {
        if (q.question.questionId !== questionId) {
          return q;
        }
        return {
          ...q,
          markedAsHard: hard,
        };
      });
      this.questions$.next(next);
    });
  }

  cardAnswer(questionId: string) {
    this.activeSolution$ = this.questionStepperService.cardAnswer$(this.learnSessionId, questionId);
    this.activeSolution$.subscribe(_ => this.showSolution$.next(true));
  }

  writeSolution(questionId: string) {
    this.activeSolution$ = this.questionStepperService.writeSolution$(this.learnSessionId, questionId);
    this.activeSolution$.subscribe(_ => this.showSolution$.next(true));
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

  nextQuestion(currentQuestionId: string) {
    const current = this.questions$.value;
    const next = current.map(q => {
      if (q.question.questionId !== currentQuestionId) {
        return q;
      }
      return {
        ...q,
        answered: true,
      };
    });
    this.questions$.next(next);
    this.showSolution$.next(false);
  }
}