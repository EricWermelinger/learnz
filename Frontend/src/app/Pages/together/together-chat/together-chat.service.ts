import { Injectable } from '@angular/core';
import { merge, Observable } from 'rxjs';
import { endpoints } from 'src/app/Config/endpoints';
import { TogetherChatDTO } from 'src/app/DTOs/Together/TogetherChatDTO';
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

  getMessages(userId: string): Observable<TogetherChatDTO> {
    return merge(
      this.api.callApi<TogetherChatDTO>(endpoints.TogetherChat, { userId }, 'GET'),
      this.ws.webSocketData<TogetherChatDTO>(endpoints.TogetherChat, {} as TogetherChatDTO),
    );
  }

  sendMessage(message: TogetherChatSendMessageDTO) {
    return this.api.callApi(endpoints.TogetherChat, message, 'POST');
  }
}
