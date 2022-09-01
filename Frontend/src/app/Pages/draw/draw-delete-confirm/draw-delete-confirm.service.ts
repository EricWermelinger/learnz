import { Injectable } from '@angular/core';
import { ApiService } from 'src/app/Framework/API/api.service';

@Injectable({
  providedIn: 'root'
})
export class DrawDeleteConfirmService {

  constructor(
    private api: ApiService,
  ) { }
}