import { Component } from '@angular/core';
import { GroupInfoDialogService } from './group-info-dialog.service';

@Component({
  selector: 'app-group-info-dialog',
  templateUrl: './group-info-dialog.component.html',
  styleUrls: ['./group-info-dialog.component.scss']
})
export class GroupInfoDialogComponent {

  constructor(
    private infoDialogService: GroupInfoDialogService,
  ) { }
}
