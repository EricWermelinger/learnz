import { Injectable } from '@angular/core';
import { endpoints } from 'src/app/Config/endpoints';
import { TestCreateDTO } from 'src/app/DTOs/Test/TestCreateDTO';
import { TestGroupTestCreateDTO } from 'src/app/DTOs/Test/TestGroupTestCreateDTO';
import { ApiService } from 'src/app/Framework/API/api.service';

@Injectable({
  providedIn: 'root'
})
export class TestCreateDialogService {

  constructor(
    private api: ApiService,
  ) { }

  testCreate$(value: TestCreateDTO) {
    return this.api.callApi(endpoints.TestCreate, value, 'POST');
  }

  testGrouTestCreate$(value: TestGroupTestCreateDTO) {
    return this.api.callApi(endpoints.TestGroupTestCreate, value, 'POST');
  }
}
