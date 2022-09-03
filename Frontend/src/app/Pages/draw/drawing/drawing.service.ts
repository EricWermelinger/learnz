import { Injectable } from '@angular/core';
import { merge, Observable } from 'rxjs';
import { endpoints } from 'src/app/Config/endpoints';
import { DrawDrawingDTO } from 'src/app/DTOs/Draw/DrawDrawingDTO';
import { DrawPageCreateDTO } from 'src/app/DTOs/Draw/DrawPageCreateDTO';
import { DrawPageEditDTO } from 'src/app/DTOs/Draw/DrawPageEditDTO';
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

  getPages$(collectionId: string): Observable<DrawDrawingDTO> {
    return merge(
      this.api.callApi<DrawDrawingDTO>(endpoints.DrawPages, { collectionId }, 'GET'),
      this.ws.webSocketData<DrawDrawingDTO>(endpoints.DrawPages, {} as DrawDrawingDTO, collectionId)
    );
  }

  createPage$(value: DrawPageCreateDTO) {
    return this.api.callApi(endpoints.DrawPages, value, 'POST');
  }

  updatePage$(value: DrawPageEditDTO) {
    return this.api.callApi(endpoints.DrawPages, value, 'PUT');
  }
}