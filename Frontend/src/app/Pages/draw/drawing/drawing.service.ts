import { Injectable } from '@angular/core';
import { merge, Observable } from 'rxjs';
import { endpoints } from 'src/app/Config/endpoints';
import { DrawPageCreateDTO } from 'src/app/DTOs/Draw/DrawPageCreateDTO';
import { DrawPageEditDTO } from 'src/app/DTOs/Draw/DrawPageEditDTO';
import { DrawPageGetDTO } from 'src/app/DTOs/Draw/DrawPageGetDTO';
import { ApiService } from 'src/app/Framework/API/api.service';
import { WebSocketService } from 'src/app/Framework/API/web-socket.service';

@Injectable({
  providedIn: 'root'
})
export class DrawingService {

  constructor(
    private api: ApiService,
    private ws: WebSocketService,
  ) { }

  getPages$(collectionId: string): Observable<DrawPageGetDTO[]> {
    return merge(
      this.api.callApi<DrawPageGetDTO[]>(endpoints.DrawPages, { collectionId }, 'GET'),
      this.ws.webSocketData<DrawPageGetDTO[]>(endpoints.DrawPages, [] as DrawPageGetDTO[], collectionId)
    );
  }

  createPage$(value: DrawPageCreateDTO) {
    return this.api.callApi(endpoints.DrawPages, value, 'POST');
  }

  updatePage$(value: DrawPageEditDTO) {
    return this.api.callApi(endpoints.DrawPages, value, 'PUT');
  }
}