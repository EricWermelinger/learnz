import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-draw-notify',
  templateUrl: './draw-notify.component.html',
  styleUrls: ['./draw-notify.component.scss']
})
export class DrawNotifyComponent {

  currentUserMakingChangesName: string | null;
  newUserMakingChangesName: string | null;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) {
    this.currentUserMakingChangesName = data.currentUserMakingChangesName;
    this.newUserMakingChangesName = data.newUserMakingChangesName;
  }
}