import { Injectable } from '@angular/core';
import { endpoints } from 'src/app/Config/endpoints';
import { LearnQuestionDTO } from 'src/app/DTOs/Learn/LearnQuestionDTO';
import { ApiService } from 'src/app/Framework/API/api.service';

@Injectable({
  providedIn: 'root'
})
export class LearnResultService {

  constructor(
    private api: ApiService,
  ) { }

  getQuestions$(learnSessionId: string) {
    return this.api.callApi<LearnQuestionDTO>(endpoints.LearnSessionQuestions, { learnSessionId }, 'GET');
  }
}
