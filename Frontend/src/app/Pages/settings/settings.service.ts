import { Injectable } from '@angular/core';
import { UserProfileUploadDTO } from 'src/app/DTOs/User/UserProfileUploadDTO';
import { UserDarkThemeDTO } from 'src/app/DTOs/User/UserDarkThemeDTO';
import { ApiService } from 'src/app/Framework/API/api.service';
import { DarkThemeService } from 'src/app/Framework/dark-theme/dark-theme.service';
import { endpoints } from 'src/app/Config/endpoints';
import { UserProfileGetDTO } from 'src/app/DTOs/User/UserProfileGetDTO';
import { Observable } from 'rxjs';
import { UserLanguageDTO } from 'src/app/DTOs/User/UserLanguageDTO';

@Injectable({
  providedIn: 'root'
})
export class SettingsService {

  constructor(
    private api: ApiService,
    private darkThemeService: DarkThemeService,
  ) { }

  getUserProfile(): Observable<UserProfileGetDTO> {
    return this.api.callApi<UserProfileGetDTO>(endpoints.UserProfile, {}, 'GET');  
  }

  save(value: UserProfileUploadDTO) {
    this.api.callApi(endpoints.UserProfile, value, 'POST').subscribe();
  }

  setDarkTheme(value: UserDarkThemeDTO) {
    this.darkThemeService.setDarkTheme(value.darkTheme);
    this.darkThemeService.applyDarkTheme(value.darkTheme);
  }

  saveLanguage(language: number) {
    this.api.callApi(endpoints.UserLanguage, { language } as UserLanguageDTO, 'POST').subscribe();
  }
}
