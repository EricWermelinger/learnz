import { Injectable } from '@angular/core';
import { endpoints } from 'src/app/Config/endpoints';
import { LearnOpenNewSessionDTO } from 'src/app/DTOs/Learn/LearnOpenNewSessionDTO';
import { ApiService } from 'src/app/Framework/API/api.service';

@Injectable({
  providedIn: 'root'
})
export class LearnCreateDialogService {

  constructor(
    private api: ApiService,
  ) { }

  createSession$(value: LearnOpenNewSessionDTO) {
    return this.api.callApi(endpoints.LearnOpenSession, value, 'POST');
  }
}
