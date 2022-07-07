import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { endpoints } from 'src/app/Config/endpoints';
import { TogetherOverviewUserProfileDTO } from 'src/app/DTOs/Together/TogetherOverviewUserProfileDTO';
import { ApiService } from 'src/app/Framework/API/api.service';

@Injectable({
  providedIn: 'root'
})
export class TogetherConnectService {

  constructor(
    private api: ApiService,
  ) { }

  getConnections(): Observable<TogetherOverviewUserProfileDTO> {
    return this.api.callApi<TogetherOverviewUserProfileDTO>(endpoints.TogetherConnectUser, {}, 'GET');
  }
}
