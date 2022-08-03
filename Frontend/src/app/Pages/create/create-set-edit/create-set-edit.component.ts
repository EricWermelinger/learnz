import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { appRoutes } from 'src/app/Config/appRoutes';
import { CreateUpsertSetHeaderDTO } from 'src/app/DTOs/Create/CreateUpsertSetHeaderDTO';
import { CreateUpsertSetQuestionsDTO } from 'src/app/DTOs/Create/CreateUpsertSetQuestionsDTO';
import { CreateSetEditService } from './create-set-edit.service';

@Component({
  selector: 'app-create-set-edit',
  templateUrl: './create-set-edit.component.html',
  styleUrls: ['./create-set-edit.component.scss']
})
export class CreateSetEditComponent {

  setId: string;
  setHeader$: Observable<CreateUpsertSetHeaderDTO>;
  setQuestions$: Observable<CreateUpsertSetQuestionsDTO[]>;

  constructor(
    private editSetService: CreateSetEditService,
    private activatedRoute: ActivatedRoute,
  ) {
    this.setId = this.activatedRoute.snapshot.paramMap.get(appRoutes.CreateSetEditId) ?? '';
    this.setHeader$ = this.editSetService.getHeader$(this.setId);
    this.setQuestions$ = this.editSetService.getQuestions$(this.setId);
  }

}
