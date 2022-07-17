import { Injectable } from '@angular/core';
import { merge, Observable } from 'rxjs';
import { endpoints } from 'src/app/Config/endpoints';
import { GroupOverviewDTO } from 'src/app/DTOs/Group/GroupOverviewDTO';
import { ApiService } from 'src/app/Framework/API/api.service';
import { WebSocketService } from 'src/app/Framework/API/web-socket.service';

@Injectable({
  providedIn: 'root'
})
export class GroupService {

  constructor(
    private api: ApiService,
    private ws: WebSocketService,
  ) { }

  getOverview(): Observable<GroupOverviewDTO[]> {
    return merge(
      this.api.callApi<GroupOverviewDTO[]>(endpoints.GroupOverview, {}, 'GET'),
      this.ws.webSocketData<GroupOverviewDTO[]>(endpoints.GroupOverview, [] as GroupOverviewDTO[]),
    );
  }
}
