import { KeyValue } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Observable, startWith, switchMap } from 'rxjs';
import { appRoutes } from 'src/app/Config/appRoutes';
import { DrawCollectionUpsertDTO } from 'src/app/DTOs/Draw/DrawCollectionUpsertDTO';
import { getDrawGroupPolicies } from 'src/app/Enums/DrawGroupPolicy';
import { FormGroupTyped } from 'src/app/Material/types';
import { TestCreateDialogService } from '../../test/test-create-dialog/test-create-dialog.service';
import { DrawSettingsService } from './draw-settings.service';

@Component({
  selector: 'app-draw-settings',
  templateUrl: './draw-settings.component.html',
  styleUrls: ['./draw-settings.component.scss']
})
export class DrawSettingsComponent {

  formGroup: FormGroupTyped<DrawCollectionUpsertDTO>;
  formControlIsGroup: FormControl;
  formControlGroupFilter = new FormControl('');
  filteredOptions$: Observable<KeyValue<string, string>[]>;
  policies = getDrawGroupPolicies();

  constructor(
    private drawSettingsService: DrawSettingsService,
    private dialogRef: MatDialogRef<DrawSettingsComponent>,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private formBuilder: FormBuilder,
    private testCreateDialogService: TestCreateDialogService,
  ) {
    this.formGroup = this.formBuilder.group({
      collectionId: data.collectionId,
      firstPageId: data.firstPageId,
      name: [data.name, Validators.required],
      groupId: data.groupId,
      drawGroupPolicy: data.drawGroupPolicy,
    }) as FormGroupTyped<DrawCollectionUpsertDTO>;
    this.formControlIsGroup = new FormControl(!!data.groupId);

    this.filteredOptions$ = this.formControlGroupFilter.valueChanges.pipe(
      startWith(''),
      switchMap(filter => this.testCreateDialogService.getFilteredGroups$(filter ?? '')),
    );
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

  selectGroup(event: MatAutocompleteSelectedEvent): void {
    const value = event.option.value;
    this.formControlGroupFilter.patchValue(value.key);
    this.formGroup.controls.groupId.patchValue(value.value);
  }
}