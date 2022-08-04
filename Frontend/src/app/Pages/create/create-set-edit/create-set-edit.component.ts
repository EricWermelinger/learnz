import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject, combineLatest, distinctUntilChanged, map, Observable, startWith } from 'rxjs';
import { appRoutes } from 'src/app/Config/appRoutes';
import { CreateQuestionOpenQuestionDTO } from 'src/app/DTOs/Create/CreateQuestionOpenQuestionDTO';
import { CreateUpsertSetHeaderDTO } from 'src/app/DTOs/Create/CreateUpsertSetHeaderDTO';
import { CreateUpsertSetQuestionsDTO } from 'src/app/DTOs/Create/CreateUpsertSetQuestionsDTO';
import { getQuestionTypes } from 'src/app/Enums/QuestionType';
import { getSetPolicies } from 'src/app/Enums/SetPolicy';
import { getSubjects } from 'src/app/Enums/Subject';
import { CreateSetDialogComponent } from '../create-set-dialog/create-set-dialog.component';
import { CreateSetEditService } from './create-set-edit.service';
import { v4 as guid } from 'uuid';

@Component({
  selector: 'app-create-set-edit',
  templateUrl: './create-set-edit.component.html',
  styleUrls: ['./create-set-edit.component.scss']
})
export class CreateSetEditComponent {

  setId: string;
  setHeader$: Observable<CreateUpsertSetHeaderDTO>;
  _setQuestions$ = new BehaviorSubject<CreateUpsertSetQuestionsDTO>({} as CreateUpsertSetQuestionsDTO);
  setQuestions$: Observable<{ questions: CreateUpsertSetQuestionsDTO, editable: boolean }>;
  isEditMode$ = new BehaviorSubject<boolean>(false);
  formGroupAddQuestion: FormGroup;
  questionTypes = getQuestionTypes();

  constructor(
    private editSetService: CreateSetEditService,
    private activatedRoute: ActivatedRoute,
    private dialog: MatDialog,
    private formBuilder: FormBuilder,
  ) {
    this.setId = this.activatedRoute.snapshot.paramMap.get(appRoutes.CreateSetEditId) ?? '';
    this.setHeader$ = this.editSetService.getHeader$(this.setId);
    this.editSetService.getQuestions$(this.setId).subscribe(x => this._setQuestions$.next(x));
    this.formGroupAddQuestion = this.formBuilder.group({
      questionType: [null, Validators.required],
    });
    this.setQuestions$ = combineLatest([
      this._setQuestions$.asObservable(),
      this.isEditMode$.asObservable().pipe(startWith(false), distinctUntilChanged()),
    ]).pipe(
      map(([setQuestions, editable]) => {
        return {
          questions: setQuestions,
          editable
        }
      }),
    );
    const isEdit = this.activatedRoute.snapshot.queryParamMap.get(appRoutes.Edit) === 'true';
    if (isEdit) {
      this.editHeader(this.setId);
    }
  }

  editHeader(setId: string) {
    const dialog = this.dialog.open(CreateSetDialogComponent, { data: {
      isNew: false,
      setId,
    }});
    dialog.afterClosed().subscribe(_ => this.setHeader$ = this.editSetService.getHeader$(this.setId));
  }

  addQuestion() {
    if (!this.isEditMode$.value) {
      this.editQuestions();
    }
    const questionTypeFilter = getQuestionTypes().filter(x => x.value === this.formGroupAddQuestion.value.questionType);
    if (questionTypeFilter.length === 0) {
      return;
    }
    const questionType = questionTypeFilter[0].key;
    let nextValue = {
      ...this._setQuestions$.value,
    };
    switch (questionType) {
      case 'OpenQuestion':
        nextValue.questionsOpenQuestion = [...nextValue.questionsOpenQuestion, { 
          id: guid(),
          question: '',
          answer: '',
        } as CreateQuestionOpenQuestionDTO];
        break;
      default:
        break;
    }
    this._setQuestions$.next(nextValue);
  }

  editQuestions() {
    this.isEditMode$.next(true);
  }

  save(openQuestions: CreateQuestionOpenQuestionDTO[]) {
    console.log(openQuestions);
  }

  translateSetPolicy(setPolicy: number) {
    return getSetPolicies().filter(x => x.value === setPolicy)[0].key;
  }

  translateSubject(subject: number) {
    return getSubjects().filter(x => x.value === subject)[0].key;
  }

  isEmpty(value: any[]) {
    return (value ?? []).length === 0;
  }
}
