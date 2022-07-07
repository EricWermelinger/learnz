import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { endpoints } from 'src/app/Config/endpoints';
import { TogetherAskAnswerDTO } from 'src/app/DTOs/Together/TogetherAskAnswerDTO';
import { TogetherAskUserDTO } from 'src/app/DTOs/Together/TogetherAskUserDTO';
import { TogetherUserProfileDTO } from 'src/app/DTOs/Together/TogetherUserProfileDTO';
import { ApiService } from 'src/app/Framework/API/api.service';

@Injectable({
  providedIn: 'root'
})
export class TogetherAskService {

  constructor(
    private api: ApiService,
  ) { }

  getOpenAsks(): Observable<TogetherUserProfileDTO> {
    return this.api.callApi<TogetherUserProfileDTO>(endpoints.TogetherAskUser, {}, 'GET');
  }

  askUser(ask: TogetherAskUserDTO) {
    return this.api.callApi(endpoints.TogetherAskUser, ask, 'POST');
  }

  answerUser(answer: TogetherAskAnswerDTO) {
    return this.api.callApi(endpoints.TogetherAskUser, answer, 'PUT');
  }

  getAllUsers(): Observable<TogetherUserProfileDTO> {
    return this.api.callApi<TogetherUserProfileDTO>(endpoints.TogetherAskUser, {}, 'GET');
  }
}
