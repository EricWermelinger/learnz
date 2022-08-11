import { Component, EventEmitter, Input, Output } from '@angular/core';
import { GeneralQuestionQuestionDTO } from 'src/app/DTOs/GeneralQuestion/GeneralQuestionQuestionDTO';
import { GeneralQuestionAnswerDTO } from 'src/app/DTOs/GeneralQuestion/GeneralQuestionAnswerDTO';
import { getSubjects } from 'src/app/Enums/Subject';

@Component({
  selector: 'app-general-question',
  templateUrl: './general-question.component.html',
  styleUrls: ['./general-question.component.scss']
})
export class GeneralQuestionComponent {

  @Input() question: GeneralQuestionQuestionDTO = { } as GeneralQuestionQuestionDTO;
  @Output() answered: EventEmitter<GeneralQuestionAnswerDTO> = new EventEmitter();

  constructor() { }

  translateSubject(subject: string) {
    return 'Subject.' + getSubjects().filter(s => s.value === subject as any as number)[0].key;
  }
}