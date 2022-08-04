import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { appRoutes } from 'src/app/Config/appRoutes';
import { CreateUpsertSetHeaderDTO } from 'src/app/DTOs/Create/CreateUpsertSetHeaderDTO';
import { CreateUpsertSetQuestionsDTO } from 'src/app/DTOs/Create/CreateUpsertSetQuestionsDTO';
import { getSetPolicies } from 'src/app/Enums/SetPolicy';
import { getSubjects } from 'src/app/Enums/Subject';
import { CreateSetDialogComponent } from '../create-set-dialog/create-set-dialog.component';
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
    private dialog: MatDialog,
  ) {
    this.setId = this.activatedRoute.snapshot.paramMap.get(appRoutes.CreateSetEditId) ?? '';
    this.setHeader$ = this.editSetService.getHeader$(this.setId);
    this.setQuestions$ = this.editSetService.getQuestions$(this.setId);
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

  translateSetPolicy(setPolicy: number) {
    return getSetPolicies().filter(x => x.value === setPolicy)[0].key;
  }

  translateSubject(subject: number) {
    return getSubjects().filter(x => x.value === subject)[0].key;
  }
}
