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
}