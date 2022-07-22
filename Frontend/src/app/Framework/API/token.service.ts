import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { appConfig } from 'src/app/Config/appConfig';
import { appRoutes } from 'src/app/Config/appRoutes';
import { TokenDTO } from 'src/app/DTOs/User/TokenDTO';
import { ErrorHandlingService } from './error-handling.service';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor(
    private router: Router,
    private errorHandler: ErrorHandlingService,
  ) { }

  patchToken(token: TokenDTO) {
    this.setToken(token.token);
    this.setRefreshToken(token.refreshToken);
    this.setExpired(token.refreshExpires);
    this.router.navigate([appRoutes.App, appRoutes.Dashboard]);
  }

  clearToken() {
    this.removeToken();
    this.removeRefreshToken();
    this.removeExpired();
    this.errorHandler.redirectToLogin();    
  }

  getToken() {
    return localStorage.getItem(appConfig.APPLICATION_TOKEN);
  }

  getRefreshToken() {
    return localStorage.getItem(appConfig.APPLICATION_REFRESH_TOKEN);
  }

  isExpired(): boolean {
    const expires = localStorage.getItem(appConfig.APPLICATION_EXPIRES);
    if (!expires) {
      return false;
    }
    const expiresDate = new Date(expires);
    const now = new Date();
    return now > expiresDate;
  }

  getSelectedLanguage() {
    return localStorage.getItem(appConfig.APPLICATION_LANGUAGE);
  }

  setSelectedLanguage(language: string) {
    localStorage.setItem(appConfig.APPLICATION_LANGUAGE, language);
  }

  setToken(token: string) {
    localStorage.setItem(appConfig.APPLICATION_TOKEN, token);
  }

  private setExpired(expires: Date) {
    localStorage.setItem(appConfig.APPLICATION_EXPIRES, expires.toString());
  }

  private setRefreshToken(refreshToken: string) {
    localStorage.setItem(appConfig.APPLICATION_REFRESH_TOKEN, refreshToken);
  }

  private removeToken() {
    localStorage.removeItem(appConfig.APPLICATION_TOKEN);
  }

  private removeRefreshToken() {
    localStorage.removeItem(appConfig.APPLICATION_REFRESH_TOKEN);
  }

  private removeExpired() {
    localStorage.removeItem(appConfig.APPLICATION_EXPIRES);
  }
}
