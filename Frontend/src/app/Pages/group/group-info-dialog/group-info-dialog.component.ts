import { Component, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { GroupInfoCreateDTO } from 'src/app/DTOs/Group/GroupInfoCreateDTO';
import { GroupInfoDTO } from 'src/app/DTOs/Group/GroupInfoDTO';
import { FormGroupTyped } from 'src/app/Material/types';
import { GroupInfoDialogService } from './group-info-dialog.service';
import { v4 as guid } from 'uuid';

@Component({
  selector: 'app-group-info-dialog',
  templateUrl: './group-info-dialog.component.html',
  styleUrls: ['./group-info-dialog.component.scss']
})
export class GroupInfoDialogComponent {

  isEditMode = false;
  groupInfo$: Observable<GroupInfoDTO> | undefined;
  formGroup: FormGroupTyped<GroupInfoCreateDTO>;

  constructor(
    private infoDialogService: GroupInfoDialogService,
    private formBuilder: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: string | null,
  ) {
    this.formGroup = this.formBuilder.group({
      groupId: null,
      description: ['', Validators.required],
      members: [[], Validators.required],
      name: ['', Validators.required],
      profileImagePath: ['', Validators.required],
    }) as any as FormGroupTyped<GroupInfoCreateDTO>;
    if (data === null) {
      this.isEditMode = true;
      this.formGroup.controls.groupId.patchValue(guid());
    } else {
      this.groupInfo$ = this.infoDialogService.getGroupInfo(data);
    }
  }

  edit(groupInfo: GroupInfoDTO) {
    this.formGroup.patchValue({
      groupId: groupInfo.groupId,
      description: groupInfo.description,
      members: groupInfo.members.map(m => m.userId),
      name: groupInfo.name,
      profileImagePath: groupInfo.profileImagePath
    });
  }
}
