import { Component } from '@angular/core';
import { TogetherChatService } from './together-chat.service';

@Component({
  selector: 'app-together-chat',
  templateUrl: './together-chat.component.html',
  styleUrls: ['./together-chat.component.scss']
})
export class TogetherChatComponent {

  constructor(
    private chatService: TogetherChatService,
  ) { }

}
