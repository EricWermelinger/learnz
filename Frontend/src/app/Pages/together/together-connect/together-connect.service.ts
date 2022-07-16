import { Injectable } from '@angular/core';
import { merge, Observable } from 'rxjs';
import { endpoints } from 'src/app/Config/endpoints';
import { TogetherOverviewUserProfileDTO } from 'src/app/DTOs/Together/TogetherOverviewUserProfileDTO';
import { ApiService } from 'src/app/Framework/API/api.service';
import { WebSocketService } from 'src/app/Framework/API/web-socket.service';

@Injectable({
  providedIn: 'root'
})
export class TogetherConnectService {

  constructor(
    private api: ApiService,
    private ws: WebSocketService,
  ) { }

  getConnections(): Observable<TogetherOverviewUserProfileDTO[]> {
    return merge(
      this.api.callApi<TogetherOverviewUserProfileDTO[]>(endpoints.TogetherConnectUser, {}, 'GET'),
      this.ws.webSocketData(endpoints.TogetherConnectUser, [] as TogetherOverviewUserProfileDTO[]),
    );
  }
}
