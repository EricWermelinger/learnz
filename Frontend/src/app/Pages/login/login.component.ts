import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { appRoutes } from 'src/app/Config/appRoutes';
import { endpoints } from 'src/app/Config/endpoints';
import { TokenDTO } from 'src/app/DTOs/User/TokenDTO';
import { UserLoginDTO } from 'src/app/DTOs/User/UserLoginDTO';
import { UserRefreshTokenDTO } from 'src/app/DTOs/User/UserRefreshTokenDTO';
import { ApiService } from 'src/app/Framework/API/api.service';
import { ErrorHandlingService } from 'src/app/Framework/API/error-handling.service';
import { TokenService } from 'src/app/Framework/API/token.service';
import { DarkThemeService } from 'src/app/Framework/dark-theme/dark-theme.service';
import { FormGroupTyped } from 'src/app/Material/types';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  form: FormGroupTyped<UserLoginDTO>;
  loginWrong: boolean = false;

  constructor(
    private api: ApiService,
    private formBuilder: FormBuilder,
    private tokenService: TokenService,
    private router: Router,
    private errorHandler: ErrorHandlingService,
    private darkThemeService: DarkThemeService,
  ) {
    this.form = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    }) as FormGroupTyped<UserLoginDTO>;

    if (this.tokenService.isExpired()) {
      this.tokenService.removeToken();
      this.tokenService.removeRefreshToken();
      this.errorHandler.redirectToLogin();
    }
    const refreshToken = this.tokenService.getRefreshToken();
    this.darkThemeService.setDarkTheme(this.darkThemeService.getDarkTheme());
    if (refreshToken) {
      this.api.callApi<TokenDTO>(endpoints.UserRefreshToken, {
        refreshToken
      } as UserRefreshTokenDTO, 'POST').subscribe(token => this.setToken(token));
    }
  }

  login() {
    this.api.callApi<TokenDTO>(endpoints.UserLogin, {
      ...this.form.value
    }, 'POST').subscribe(token => this.setToken(token));
  }

  setToken(token: TokenDTO) {
    this.tokenService.setToken(token.token);
    this.tokenService.setRefreshToken(token.refreshToken);
    this.tokenService.setExpired(token.refreshExpires);
    this.router.navigate([appRoutes.App, appRoutes.Dashboard]);
  }
}
