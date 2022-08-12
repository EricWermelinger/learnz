import { Component, EventEmitter, Input, Output } from '@angular/core';
import { GeneralQuestionQuestionDTO } from 'src/app/DTOs/GeneralQuestion/GeneralQuestionQuestionDTO';
import { GeneralQuestionAnswerDTO } from 'src/app/DTOs/GeneralQuestion/GeneralQuestionAnswerDTO';
import { getSubjects } from 'src/app/Enums/Subject';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { getQuestionTypes } from 'src/app/Enums/QuestionType';
import { ChallengeQuestionAnswerDTO } from 'src/app/DTOs/Challenge/ChallengeQuestionAnswerDTO';
import { KeyValue } from '@angular/common';

@Component({
  selector: 'app-general-question',
  templateUrl: './general-question.component.html',
  styleUrls: ['./general-question.component.scss']
})
export class GeneralQuestionComponent {

  formGroup: FormGroup;
  multipleChoiceValues: string[] = [];
  distributeAnswers: KeyValue<string, string>[] = [];
  @Input() question: GeneralQuestionQuestionDTO = { } as GeneralQuestionQuestionDTO;
  @Input() challengeId: string = '';
  @Input() disabled: boolean = false;
  @Output() answered: EventEmitter<GeneralQuestionAnswerDTO> = new EventEmitter();

  constructor(
    private formBuilder: FormBuilder,
  ) {
    this.formGroup = this.formBuilder.group({
      answer: ['', Validators.required],
    });
  }

  saveForm() {
    const answer = this.formGroup.value.answer;
    this.answered.emit({
      challengeId: this.challengeId,
      answer,
      questionId: this.question.questionId,
    } as GeneralQuestionAnswerDTO);
  }

  saveDistribute() {
    this.answered.emit({
      challengeId: this.challengeId,
      answer: this.distributeAnswers.map(a => a.key + '|' + a.value).join('||'),
      questionId: this.question.questionId,
    })
  }

  saveTrueFalse(value: boolean) {
    this.answered.emit({
      challengeId: this.challengeId,
      answer: value ? 'true' : 'false',
      questionId: this.question.questionId,
    } as GeneralQuestionAnswerDTO);
  }

  saveMultipleChoice() {
    this.answered.emit({
      challengeId: this.challengeId,
      answer: this.multipleChoiceValues.join('|'),
      questionId: this.question.questionId,
    } as GeneralQuestionAnswerDTO);
  }

  translateSubject(subject: string) {
    const filtered = getSubjects().filter(s => s.value === subject as any as number);
    if (filtered.length === 0) {
      return '';
    }
    return 'Subject.' + filtered[0].key;
  }

  getQuestionType(type: number) {
    return getQuestionTypes().filter(t => t.value === type)[0].key;
  }

  toggleMultipleChoice(value: string) {
    if (this.multipleChoiceValues.includes(value)) {
      this.multipleChoiceValues = this.multipleChoiceValues.filter(v => v !== value);
    } else {
      this.multipleChoiceValues.push(value);
    }
  }

  distributeConnect(leftSideId: string, rightSideAnswer: string, rightSide: ChallengeQuestionAnswerDTO[]) {
    var answer = {
      key: leftSideId,
      value: rightSide.filter(rs => rs.answer === rightSideAnswer)[0].answerId,
    } as KeyValue<string, string>;
    if (this.distributeAnswers.includes(answer)) {
      this.distributeAnswers = this.distributeAnswers.filter(a => a.key !== leftSideId);
    }
    this.distributeAnswers.push(answer);
  }

  showForm(type: number) {
    const questionType = this.getQuestionType(type);
    return questionType === 'Mathematic' || questionType === 'OpenQuestion' || questionType === 'TextField' || questionType === 'Word';
  }

  showText(type: number) {
    const questionType = this.getQuestionType(type);
    return questionType === 'OpenQuestion' || questionType === 'TextField' || questionType === 'Word';  
  }

  showTrueFalse(type: number) {
    return this.getQuestionType(type) === 'TrueFalse';
  }

  showMultipleChoice(type: number) {
    return this.getQuestionType(type) === 'MultipleChoice';
  }

  showDistribute(type: number) {
    return this.getQuestionType(type) === 'Distribute';
  }
}