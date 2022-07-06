import { Injectable } from '@angular/core';
import { endpoints } from 'src/app/Config/endpoints';
import { UserChangePasswordDTO } from 'src/app/DTOs/User/UserChangePasswordDTO';
import { ApiService } from 'src/app/Framework/API/api.service';

@Injectable({
  providedIn: 'root'
})
export class PasswordChangeDialogService {

  constructor(
    private api: ApiService,
  ) { }

  save(value: UserChangePasswordDTO) {
    return this.api.callApi(endpoints.UserProfile, value, 'PUT');
  }
}
