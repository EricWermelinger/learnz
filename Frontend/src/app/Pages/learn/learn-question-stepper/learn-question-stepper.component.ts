import { Component } from '@angular/core';
import { LearnQuestionStepperService } from './learn-question-stepper.service';

@Component({
  selector: 'app-learn-question-stepper',
  templateUrl: './learn-question-stepper.component.html',
  styleUrls: ['./learn-question-stepper.component.scss']
})
export class LearnQuestionStepperComponent {

  constructor(
    private questionStepperService: LearnQuestionStepperService,
  ) { }

}