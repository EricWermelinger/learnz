import { Injectable } from '@angular/core';
import { merge, Observable } from 'rxjs';
import { endpoints } from 'src/app/Config/endpoints';
import { DrawCollectionGetDTO } from 'src/app/DTOs/Draw/DrawCollectionGetDTO';
import { ApiService } from 'src/app/Framework/API/api.service';
import { WebSocketService } from 'src/app/Framework/API/web-socket.service';

@Injectable({
  providedIn: 'root'
})
export class DrawService {

  constructor(
    private api: ApiService,
    private ws: WebSocketService,
  ) { }

  getCollections$(): Observable<DrawCollectionGetDTO[]> {
    return merge(
      this.api.callApi<DrawCollectionGetDTO[]>(endpoints.DrawCollections, {}, 'GET'),
      this.ws.webSocketData<DrawCollectionGetDTO[]>(endpoints.DrawCollections, [] as DrawCollectionGetDTO[]),
    );
  }
}
