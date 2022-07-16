import { Injectable } from '@angular/core';
import { merge, Observable } from 'rxjs';
import { endpoints } from 'src/app/Config/endpoints';
import { TogetherChatMessageDTO } from 'src/app/DTOs/Together/TogetherChatMessageDTO';
import { TogetherChatSendMessageDTO } from 'src/app/DTOs/Together/TogetherChatSendMessageDTO';
import { ApiService } from 'src/app/Framework/API/api.service';
import { WebSocketService } from 'src/app/Framework/API/web-socket.service';

@Injectable({
  providedIn: 'root'
})
export class TogetherChatService {

  constructor(
    private api: ApiService,
    private ws: WebSocketService,
  ) { }

  getMessages(userId: string): Observable<TogetherChatMessageDTO[]> {    
    return merge(
      this.api.callApi<TogetherChatMessageDTO[]>(endpoints.TogetherChat, { userId }, 'GET'),
      this.ws.webSocketData<TogetherChatMessageDTO[]>(endpoints.TogetherChat, [] as TogetherChatMessageDTO[]),
    );
  }

  sendMessage(message: TogetherChatSendMessageDTO) {
    return this.api.callApi(endpoints.TogetherChat, message, 'POST');
  }
}
