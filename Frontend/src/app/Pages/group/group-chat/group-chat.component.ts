import { Component } from '@angular/core';
import { GroupChatService } from './group-chat.service';

@Component({
  selector: 'app-group-chat',
  templateUrl: './group-chat.component.html',
  styleUrls: ['./group-chat.component.scss']
})
export class GroupChatComponent {

  constructor(
    private chatService: GroupChatService,
  ) { }
}
