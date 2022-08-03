import { Component, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CreateUpsertSetHeaderDTO } from 'src/app/DTOs/Create/CreateUpsertSetHeaderDTO';
import { getSetPolicies } from 'src/app/Enums/SetPolicy';
import { getSubjects } from 'src/app/Enums/Subject';
import { FormGroupTyped } from 'src/app/Material/types';
import { CreateSetDialogService } from './create-set-dialog.service';

@Component({
  selector: 'app-create-set-dialog',
  templateUrl: './create-set-dialog.component.html',
  styleUrls: ['./create-set-dialog.component.scss']
})
export class CreateSetDialogComponent {

  formGroup: FormGroupTyped<CreateUpsertSetHeaderDTO>;
  subjects = getSubjects();
  policies = getSetPolicies();
  
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private formBuilder: FormBuilder,
    private headerService: CreateSetDialogService,
    private dialogRef: MatDialogRef<CreateSetDialogComponent>,
  ) {
    this.formGroup = this.formBuilder.group({
      id: '',
      name: ['', Validators.required],
      description: ['', Validators.required],
      setPolicy: [null, Validators.required],
      subjectMain: [null, Validators.required],
      subjectSecond: null,
    }) as any as FormGroupTyped<CreateUpsertSetHeaderDTO>;
    if (this.data.isNew as boolean) {
      this.formGroup.controls.id.patchValue(this.data.setId as string);
    } else {
      this.headerService.getHeader(this.data.setId as string).subscribe(header => {
        this.formGroup.patchValue(header);
      });
    }
  }

  save() {
    this.headerService.upsertHeader(this.formGroup.value).subscribe(_ => {
      this.dialogRef.close();
    });
  }

  close() {
    this.dialogRef.close();
  }
}