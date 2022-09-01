import { Injectable, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { ApiService } from 'src/app/Framework/API/api.service';
import { WebSocketService } from 'src/app/Framework/API/web-socket.service';

@Injectable({
  providedIn: 'root'
})
export class DrawingService implements OnDestroy {

  private destroyed$ = new Subject<void>();

  constructor(
    private api: ApiService,
    private ws: WebSocketService,
  ) { }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}