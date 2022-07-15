import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { endpoints } from 'src/app/Config/endpoints';
import { TogetherSwipeDTO } from 'src/app/DTOs/Together/TogetherSwipeDTO';
import { TogetherUserProfileDTO } from 'src/app/DTOs/Together/TogetherUserProfileDTO';
import { getGrades } from 'src/app/Enums/Grade';
import { getSubjects } from 'src/app/Enums/Subject';
import { ApiService } from 'src/app/Framework/API/api.service';

@Injectable({
  providedIn: 'root'
})
export class TogetherSwipeService {

  constructor(
    private api: ApiService
  ) { }

  getNextSwipe(): Observable<TogetherUserProfileDTO> {
    return this.api.callApi<TogetherUserProfileDTO>(endpoints.TogetherSwipeUser, {}, 'GET');
  }

  swipe(swipe: TogetherSwipeDTO) {
    return this.api.callApi(endpoints.TogetherSwipeUser, swipe, 'POST');
  }

  translateSubject(subject: number) {
    return 'Subject.' + getSubjects().filter(s => s.value === subject)[0].key;
  }
  
  translateGrade(grade: number) {
    return 'Grade.' + getGrades().filter(g => g.value === grade)[0].key;
  }
}
