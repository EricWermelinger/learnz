import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { appRoutes } from 'src/app/Config/appRoutes';
import { UserLoginDTO } from 'src/app/DTOs/User/UserLoginDTO';
import { TokenService } from 'src/app/Framework/API/token.service';
import { DarkThemeService } from 'src/app/Framework/dark-theme/dark-theme.service';
import { FormGroupTyped } from 'src/app/Material/types';
import { LoginService } from './login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  form: FormGroupTyped<UserLoginDTO>;
  loginWrong: boolean = false;

  constructor(
    private loginService: LoginService,
    private formBuilder: FormBuilder,
    private tokenService: TokenService,
    private darkThemeService: DarkThemeService,
    private router: Router,
  ) {
    this.form = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    }) as FormGroupTyped<UserLoginDTO>;

    if (this.tokenService.isExpired()) {
      this.tokenService.clearToken();
    }
    const refreshToken = this.tokenService.getRefreshToken();
    this.darkThemeService.setDarkTheme(this.darkThemeService.getDarkTheme());
    if (refreshToken) {
     this.loginService.loginByRefreshToken(refreshToken);
    }
  }

  login() {
    this.loginService.login(this.form.value);
  }

  signUpInstead() {
    this.router.navigate([appRoutes.SignUp]);
  }
}
