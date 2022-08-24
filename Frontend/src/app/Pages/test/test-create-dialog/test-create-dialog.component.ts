import { Component } from '@angular/core';
import { TestCreateDialogService } from './test-create-dialog.service';

@Component({
  selector: 'app-test-create-dialog',
  templateUrl: './test-create-dialog.component.html',
  styleUrls: ['./test-create-dialog.component.scss']
})
export class TestCreateDialogComponent$ {

  constructor(
    private createDialogService: TestCreateDialogService,
  ) { }

}
