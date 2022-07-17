import { Component } from '@angular/core';
import { GroupFilesService } from './group-files.service';

@Component({
  selector: 'app-group-files',
  templateUrl: './group-files.component.html',
  styleUrls: ['./group-files.component.scss']
})
export class GroupFilesComponent {

  constructor(
    private filesService: GroupFilesService,
  ) { }
}
