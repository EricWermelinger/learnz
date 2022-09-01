import { Injectable } from '@angular/core';
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
}
