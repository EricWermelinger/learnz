import { Component, OnDestroy } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { appRoutes } from 'src/app/Config/appRoutes';
import { TogetherChatMessageDTO } from 'src/app/DTOs/Together/TogetherChatMessageDTO';
import { TogetherChatSendMessageDTO } from 'src/app/DTOs/Together/TogetherChatSendMessageDTO';
import { isToday } from 'src/app/Framework/Helpers/DateHelpers';
import { TogetherChatService } from './together-chat.service';

@Component({
  selector: 'app-together-chat',
  templateUrl: './together-chat.component.html',
  styleUrls: ['./together-chat.component.scss']
})
export class TogetherChatComponent implements OnDestroy {

  chat$: Observable<TogetherChatMessageDTO[]>;
  private destroyed$ = new Subject<void>();
  newMessageControl = new FormControl('', Validators.required);
  chatId: string;

  constructor(
    private chatService: TogetherChatService,
    private activatedRoute: ActivatedRoute,
  ) {
    this.chatId = this.activatedRoute.snapshot.paramMap.get(appRoutes.TogetherChatId) ?? '';
    this.chat$ = this.chatService.getMessages(this.chatId);
  }

  isToday(date: Date) {
    return isToday(date);
  }

  sendMessage() {
    const value = {
      userId: this.chatId,
      message: this.newMessageControl.value,
    } as TogetherChatSendMessageDTO;
    this.chatService.sendMessage(value).subscribe(_ => {
      this.newMessageControl = new FormControl('', Validators.required);
    });
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}
