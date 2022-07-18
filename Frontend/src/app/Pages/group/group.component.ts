import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { GroupInfoDialogComponent } from './group-info-dialog/group-info-dialog.component';
import { GroupService } from './group.service';

@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.scss']
})
export class GroupComponent {

  constructor(
    private groupService: GroupService,
    private dialog: MatDialog,
  ) { }

  openDialog() {
    this.dialog.open(GroupInfoDialogComponent, {
      data: null,
    });
  }
}
