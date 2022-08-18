import { Injectable } from '@angular/core';
import { endpoints } from 'src/app/Config/endpoints';
import { LearnSessionDTO } from 'src/app/DTOs/Learn/LearnSessionDTO';
import { ApiService } from 'src/app/Framework/API/api.service';

@Injectable({
  providedIn: 'root'
})
export class LearnService {

  constructor(
    private api: ApiService,
  ) { }

  getClosedSessions$() {
    return this.api.callApi<LearnSessionDTO[]>(endpoints.LearnClosedSession, { }, 'GET');
  }

  getOpenSessions$() {
    return this.api.callApi<LearnSessionDTO[]>(endpoints.LearnOpenSession, { }, 'GET');
  }
}
