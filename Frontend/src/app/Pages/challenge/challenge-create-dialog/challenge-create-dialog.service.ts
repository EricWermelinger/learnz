import { Injectable } from '@angular/core';
import { endpoints } from 'src/app/Config/endpoints';
import { ChallengeCreateDTO } from 'src/app/DTOs/Challenge/ChallengeCreateDTO';
import { ApiService } from 'src/app/Framework/API/api.service';

@Injectable({
  providedIn: 'root'
})
export class ChallengeCreateDialogService {

  constructor(
    private api: ApiService,
  ) { }

  createChallenge(challenge: ChallengeCreateDTO) {
    return this.api.callApi(endpoints.ChallengeOpen, { challenge }, 'POST');
  }
}