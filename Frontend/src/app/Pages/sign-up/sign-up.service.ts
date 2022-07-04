import { Injectable } from '@angular/core';
import { endpoints } from 'src/app/Config/endpoints';
import { TokenDTO } from 'src/app/DTOs/User/TokenDTO';
import { UserDarkThemeDTO } from 'src/app/DTOs/User/UserDarkThemeDTO';
import { UserSignUpDTO } from 'src/app/DTOs/User/UserSignUpDTO';
import { ApiService } from 'src/app/Framework/API/api.service';
import { TokenService } from 'src/app/Framework/API/token.service';
import { DarkThemeService } from 'src/app/Framework/dark-theme/dark-theme.service';

@Injectable({
  providedIn: 'root'
})
export class SignUpService {

  constructor(
    private api: ApiService,
    private tokenService: TokenService,
    private darkThemeService: DarkThemeService,
  ) { }

  save(value: UserSignUpDTO) {
    this.api.callApi<TokenDTO>(endpoints.UserSignUp, value, 'POST').subscribe(token => {
      this.tokenService.patchToken(token);
    });
  }

  setDarkTheme(value: UserDarkThemeDTO) {
    this.darkThemeService.setDarkTheme(value.darkTheme);
    this.darkThemeService.applyDarkTheme(value.darkTheme);
  }
}
