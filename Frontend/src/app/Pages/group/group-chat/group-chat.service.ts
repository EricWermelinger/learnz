import { Injectable } from '@angular/core';
import { merge, Observable } from 'rxjs';
import { endpoints } from 'src/app/Config/endpoints';
import { GroupMessageGetDTO } from 'src/app/DTOs/Group/GroupMessageGetDTO';
import { GroupMessageSendDTO } from 'src/app/DTOs/Group/GroupMessageSendDTO';
import { ApiService } from 'src/app/Framework/API/api.service';
import { WebSocketService } from 'src/app/Framework/API/web-socket.service';

@Injectable({
  providedIn: 'root'
})
export class GroupChatService {

  constructor(
    private api: ApiService,
    private ws: WebSocketService,
  ) { }

  getMessages(groupId: string): Observable<GroupMessageGetDTO[]> {
    return merge(
      this.api.callApi<GroupMessageGetDTO[]>(endpoints.GroupMessage, { groupId }, 'GET'),
      this.ws.webSocketData<GroupMessageGetDTO[]>(endpoints.GroupMessage, [] as GroupMessageGetDTO[], groupId),
    );
  }

  sendMessage(message: GroupMessageSendDTO) {
    this.api.callApi(endpoints.GroupMessage, message, 'POST').subscribe();
  }
}
