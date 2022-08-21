import { KeyValue } from '@angular/common';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { endpoints } from 'src/app/Config/endpoints';
import { CreateSetOverviewDTO } from 'src/app/DTOs/Create/CreateSetOverviewDTO';
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

  getFilteredSets$(setNameFilter: string): Observable<KeyValue<string, string>[]> {
    const filter = {
      subjectMain: -1,
      subjectSecond: -1,
      name: setNameFilter
    };
    return this.api.callApi<CreateSetOverviewDTO[]>(endpoints.CreateFilterSets, filter, 'GET').pipe(
      map(sets => sets.map(set => ({ key: set.name, value: set.setId } as KeyValue<string, string>)))
    );
  }
}
