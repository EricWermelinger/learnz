import { Component, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable, Subject, takeUntil } from 'rxjs';
import { appRoutes } from 'src/app/Config/appRoutes';
import { GroupMessageGetDTO } from 'src/app/DTOs/Group/GroupMessageGetDTO';
import { GroupChatService } from './group-chat.service';

@Component({
  selector: 'app-group-chat',
  templateUrl: './group-chat.component.html',
  styleUrls: ['./group-chat.component.scss']
})
export class GroupChatComponent implements OnDestroy {

  private groupId: string;
  private destroyed$ = new Subject<void>();
  chat$: Observable<GroupMessageGetDTO[]>;
  
  constructor(
    private chatService: GroupChatService,
    private activatedRoute: ActivatedRoute,
  ) {
    this.groupId = this.activatedRoute.snapshot.paramMap.get(appRoutes.GroupChatId) ?? '';
    this.chat$ = this.chatService.getMessages(this.groupId).pipe(takeUntil(this.destroyed$));
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}
