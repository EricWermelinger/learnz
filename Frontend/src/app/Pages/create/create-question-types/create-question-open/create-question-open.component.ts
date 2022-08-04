import { Component, Input } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { CreateQuestionOpenQuestionDTO } from 'src/app/DTOs/Create/CreateQuestionOpenQuestionDTO';
import { FormGroupTyped } from 'src/app/Material/types';

@Component({
  selector: 'app-create-question-open',
  templateUrl: './create-question-open.component.html',
  styleUrls: ['./create-question-open.component.scss']
})
export class CreateQuestionOpenComponent {

  formGroup: FormGroupTyped<CreateQuestionOpenQuestionDTO>;
  @Input() question: CreateQuestionOpenQuestionDTO = { } as CreateQuestionOpenQuestionDTO;
  @Input() editable: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
  ) {
    this.formGroup = this.formBuilder.group({
      id: this.question.id,
      answer: this.question.answer,
      question: this.question.question,
    }) as FormGroupTyped<CreateQuestionOpenQuestionDTO>;
  }
}