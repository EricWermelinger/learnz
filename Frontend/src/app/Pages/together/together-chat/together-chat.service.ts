import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { endpoints } from 'src/app/Config/endpoints';
import { TogetherChatMessageDTO } from 'src/app/DTOs/Together/TogetherChatMessageDTO';
import { TogetherChatSendMessageDTO } from 'src/app/DTOs/Together/TogetherChatSendMessageDTO';
import { ApiService } from 'src/app/Framework/API/api.service';

@Injectable({
  providedIn: 'root'
})
export class TogetherChatService {

  constructor(
    private api: ApiService,
  ) { }

  getMessages(userId: string): Observable<TogetherChatMessageDTO> {
    return this.api.callApi<TogetherChatMessageDTO>(endpoints.TogetherChat, { userId }, 'GET');
  }

  sendMessage(message: TogetherChatSendMessageDTO) {
    return this.api.callApi(endpoints.TogetherChat, message, 'POST');
  }
}
