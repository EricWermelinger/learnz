import { Component, Input } from '@angular/core';
import { LearnQuestionDTO } from 'src/app/DTOs/Learn/LearnQuestionDTO';
import { getQuestionTypes } from 'src/app/Enums/QuestionType';

@Component({
  selector: 'app-learn-result',
  templateUrl: './learn-result.component.html',
  styleUrls: ['./learn-result.component.scss']
})
export class LearnResultComponent {

  @Input() questions: LearnQuestionDTO[] = [];

  constructor() { }

  cleanupAnswer(answer: string | null, questionTypeNumber: number) {
    if (answer == null) {
      return '-';
    }
    const questionType = getQuestionTypes().filter(t => t.value === questionTypeNumber)[0].key;
    switch (questionType) {
      case 'Distribute':
        // todo;
        return answer;
      case 'MultipleChoice':
        // todo
        return answer;
      default:
        return answer;
    }
  }
}