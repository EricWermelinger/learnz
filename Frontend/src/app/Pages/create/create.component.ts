import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { appRoutes } from 'src/app/Config/appRoutes';
import { CreateSetOverviewDTO } from 'src/app/DTOs/Create/CreateSetOverviewDTO';
import { getSubjects } from 'src/app/Enums/Subject';
import { CreateSetDialogComponent } from './create-set-dialog/create-set-dialog.component';
import { CreateService } from './create.service';
import { v4 as guid } from 'uuid';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.scss']
})
export class CreateComponent {

  own$: Observable<CreateSetOverviewDTO[]>;
  latest$: Observable<CreateSetOverviewDTO[]>;
  filtered$: Observable<CreateSetOverviewDTO[]>;
  filter: FormGroup;
  subjects = getSubjects();
  
  constructor(
    private createService: CreateService,
    private formBuilder: FormBuilder,
    private dialog: MatDialog,
    private router: Router,
  ) {
    this.filter = this.formBuilder.group({
      subjectMain: -1,
      subjectSecond: -1,
      name: '',
    });
    this.own$ = this.createService.getOwn();
    this.latest$ = this.createService.getLatest();
    this.filtered$ = this.createService.getFiltered(-1, -1, '');
  }

  filterApply() {
    this.filtered$ = this.createService.getFiltered(this.filter.value.subjectMain, this.filter.value.subjectSecond, this.filter.value.name);
  }

  newSet() {
    const setId = guid();
    const dialog = this.dialog.open(CreateSetDialogComponent, { data: {
      isNew: true,
      setId,
    } });
    dialog.afterClosed().subscribe(_ => {
      this.router.navigate([appRoutes.App, appRoutes.Create, setId])
    });
  }
}