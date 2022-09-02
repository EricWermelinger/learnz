import { Component, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { appRoutes } from 'src/app/Config/appRoutes';
import { DrawCollectionUpsertDTO } from 'src/app/DTOs/Draw/DrawCollectionUpsertDTO';
import { FormGroupTyped } from 'src/app/Material/types';
import { DrawSettingsService } from './draw-settings.service';

@Component({
  selector: 'app-draw-settings',
  templateUrl: './draw-settings.component.html',
  styleUrls: ['./draw-settings.component.scss']
})
export class DrawSettingsComponent {

  formGroup: FormGroupTyped<DrawCollectionUpsertDTO>;

  constructor(
    private drawSettingsService: DrawSettingsService,
    private dialogRef: MatDialogRef<DrawSettingsComponent>,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private formBuilder: FormBuilder,
  ) {
    this.formGroup = formBuilder.group({
      collectionId: data.collectionId,
      firstPageId: data.firstPageId,
      name: [data.name, Validators.required],
      groupId: data.groupId,
      drawGroupPolicy: data.drawGroupPolicy,
    }) as FormGroupTyped<DrawCollectionUpsertDTO>;
  }

  save() {
    this.drawSettingsService.upsertSettings$(this.formGroup.value).subscribe(_ => {
      this.dialogRef.close();
      this.router.navigate([appRoutes.App, appRoutes.Draw]);
    });
  }

  cancel() {
    this.dialogRef.close();
  }
}