import { Component } from '@angular/core';
import { LearnCreateDialogService } from './learn-create-dialog.service';

@Component({
  selector: 'app-learn-create-dialog',
  templateUrl: './learn-create-dialog.component.html',
  styleUrls: ['./learn-create-dialog.component.scss']
})
export class LearnCreateDialogComponent {

  constructor(
    createDialogService: LearnCreateDialogService,
  ) { }
 
}