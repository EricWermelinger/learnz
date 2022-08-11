import { Component, Inject } from '@angular/core';
import { ChallengeCreateDialogService } from './challenge-create-dialog.service';
import { v4 as guid } from 'uuid';
import { FormGroupTyped } from 'src/app/Material/types';
import { ChallengeCreateDTO } from 'src/app/DTOs/Challenge/ChallengeCreateDTO';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { appRoutes } from 'src/app/Config/appRoutes';

@Component({
  selector: 'app-challenge-create-dialog',
  templateUrl: './challenge-create-dialog.component.html',
  styleUrls: ['./challenge-create-dialog.component.scss']
})
export class ChallengeCreateDialogComponent {

  formGroup: FormGroupTyped<ChallengeCreateDTO>;
  setEditable: boolean;
  setName: string = '';
  challengeId: string;

  constructor(
    private challengeCreateService: ChallengeCreateDialogService,
    private formBuilder: FormBuilder,
    private dialogRef: MatDialogRef<ChallengeCreateDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private router: Router,
  ) {
    this.challengeId = guid();
    this.formGroup = this.formBuilder.group({
      challengeId: this.challengeId,
      name: ['', Validators.required],
      createSetId: ['', Validators.required],
    }) as FormGroupTyped<ChallengeCreateDTO>;
    this.setEditable = data.setEditable as boolean;
    if (!this.setEditable) {
      this.setName = data.setName as string;
      this.formGroup.controls.createSetId.patchValue(data.setId as string);
    }
  }

  save() {
    this.challengeCreateService.createChallenge(this.formGroup.value).subscribe(_ => {
      this.dialogRef.close();
      this.router.navigate([appRoutes.App, appRoutes.Challenge, this.challengeId]);
    });
  }

  cancel() {
    this.dialogRef.close();
  }
}