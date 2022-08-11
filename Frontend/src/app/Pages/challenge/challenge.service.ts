import { Injectable } from '@angular/core';
import { endpoints } from 'src/app/Config/endpoints';
import { ChallengeOpenDTO } from 'src/app/DTOs/Challenge/ChallengeOpenDTO';
import { ApiService } from 'src/app/Framework/API/api.service';

@Injectable({
  providedIn: 'root'
})
export class ChallengeService {

  constructor(
    private api: ApiService,
  ) { }

  getOpenChallenges() {
    return this.api.callApi<ChallengeOpenDTO[]>(endpoints.ChallengeOpen, { }, 'GET');
  }

  joinChallenge(challengeId: string) {
    this.api.callApi(endpoints.ChallengeOpen, { challengeId }, 'PUT').subscribe();
  }

  cancelChallenge(challengeId: string) {
    this.api.callApi(endpoints.ChallengeOpen, { challengeId }, 'DELETE').subscribe();
  }
}