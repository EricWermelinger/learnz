import { Component, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { appRoutes } from 'src/app/Config/appRoutes';
import { TestCreateDTO } from 'src/app/DTOs/Test/TestCreateDTO';
import { TestGroupTestCreateDTO } from 'src/app/DTOs/Test/TestGroupTestCreateDTO';
import { FormGroupTyped } from 'src/app/Material/types';
import { TestCreateDialogService } from './test-create-dialog.service';
import { v4 as guid } from 'uuid';

@Component({
  selector: 'app-test-create-dialog',
  templateUrl: './test-create-dialog.component.html',
  styleUrls: ['./test-create-dialog.component.scss']
})
export class TestCreateDialogComponent {

  setEditable: boolean;
  setName: string = '';

  isGroupTest$ = new BehaviorSubject<boolean>(false);
  formGroupTestNormal: FormGroupTyped<TestCreateDTO>;
  formGroupTestGroupTest: FormGroupTyped<TestGroupTestCreateDTO>;

  constructor(
    private createDialogService: TestCreateDialogService,
    private formBuilder: FormBuilder,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) {
    this.formGroupTestNormal = this.formBuilder.group({
      testId: [guid(), Validators.required],
      setId: ['', Validators.required],
      name: ['', Validators.required],
      maxTime: [0, Validators.required]
    }) as FormGroupTyped<TestCreateDTO>;

    this.formGroupTestGroupTest = this.formBuilder.group({
      testId: [guid(), Validators.required],
      setId: ['', Validators.required],
      name: ['', Validators.required],
      maxTime: [0, Validators.required],
      groupId: ['', Validators.required],
    }) as FormGroupTyped<TestGroupTestCreateDTO>;

    this.setEditable = data.setEditable as boolean;
    if (!this.setEditable) {
      this.setName = data.setName as string;
      this.formGroupTestNormal.controls.setId.patchValue(data.setId as string);
      this.formGroupTestGroupTest.controls.setId.patchValue(data.setId as string);
    }
  }

  changeTestType(isGroupTest: boolean) {
    this.isGroupTest$.next(isGroupTest);
  }

  testCreate() {
    if (this.isGroupTest$.value) {
      this.createDialogService.testCreate$(this.formGroupTestGroupTest.value).subscribe(_ => {
        this.router.navigate([appRoutes.App, appRoutes.Test]);
      });
    } else {
      this.createDialogService.testGrouTestCreate$(this.formGroupTestGroupTest.value).subscribe(_ => {
        this.router.navigate([appRoutes.App, appRoutes.Test]);
      });
    }
  }
}
