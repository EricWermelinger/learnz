import { Component } from '@angular/core';
import { GroupService } from './group.service';

@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.scss']
})
export class GroupComponent {

  constructor(
    private groupService: GroupService,
  ) { }

}
