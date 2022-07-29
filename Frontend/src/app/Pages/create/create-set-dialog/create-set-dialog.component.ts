import { Component, Inject } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CreateUpsertSetHeaderDTO } from 'src/app/DTOs/Create/CreateUpsertSetHeaderDTO';
import { FormGroupTyped } from 'src/app/Material/types';
import { v4 as guid } from 'uuid';
import { CreateSetDialogService } from './create-set-dialog.service';

@Component({
  selector: 'app-create-set-dialog',
  templateUrl: './create-set-dialog.component.html',
  styleUrls: ['./create-set-dialog.component.scss']
})
export class CreateSetDialogComponent {

  formGroup: FormGroupTyped<CreateUpsertSetHeaderDTO>;
  
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: string | null,
    private formBuilder: FormBuilder,
    private headerService: CreateSetDialogService,
    private dialogRef: MatDialogRef<CreateSetDialogComponent>,
  ) {
    this.formGroup = this.formBuilder.group({
      id: '',
      name: '',
      description: '',
      setPolicy: -1,
      subjectMain: -1,
      subjectSecond: -1,
    }) as any as FormGroupTyped<CreateUpsertSetHeaderDTO>;
    if (data) {
      this.headerService.getHeader(data).subscribe(header => {
        this.formGroup.patchValue(header);
      });
    } else {
      this.formGroup.controls.id.patchValue(guid());
    }
  }

  save() {
    this.headerService.upsertHeader(this.formGroup.value).subscribe(_ => {
      this.dialogRef.close();
    });
  }

  cancel() {
    this.dialogRef.close();
  }
}