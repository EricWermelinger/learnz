import { Component, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable, of } from 'rxjs';
import { GroupInfoCreateDTO } from 'src/app/DTOs/Group/GroupInfoCreateDTO';
import { GroupInfoDTO } from 'src/app/DTOs/Group/GroupInfoDTO';
import { FormGroupTyped } from 'src/app/Material/types';
import { GroupInfoDialogService } from './group-info-dialog.service';
import { v4 as guid } from 'uuid';
import { GroupPossibleUserDTO } from 'src/app/DTOs/Group/GroupPossibleUserDTO';

@Component({
  selector: 'app-group-info-dialog',
  templateUrl: './group-info-dialog.component.html',
  styleUrls: ['./group-info-dialog.component.scss']
})
export class GroupInfoDialogComponent {

  isEditMode = false;
  groupInfo$: Observable<GroupInfoDTO> | undefined;
  possibleUsers$: Observable<GroupPossibleUserDTO[]>;
  formGroup: FormGroupTyped<GroupInfoCreateDTO>;

  constructor(
    private infoDialogService: GroupInfoDialogService,
    private formBuilder: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: string | null,
    private dialogRef: MatDialogRef<GroupInfoDialogComponent>,
  ) {
    this.possibleUsers$ = this.infoDialogService.getPossibleUsers();
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

  leaveGroup(groupId: string) {
    this.infoDialogService.leaveGroup({ groupId }).subscribe(_ => this.dialogRef.close());
  }

  upsertGroup() {
    this.infoDialogService.upsertGroup(this.formGroup.value).subscribe(_ => this.dialogRef.close());
  }

  closeDialog() {
    this.dialogRef.close();
  }
}
