import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { catchError, Observable, switchMap, tap } from 'rxjs';
import { appConfig } from 'src/app/Config/appConfig';
import { endpoints } from 'src/app/Config/endpoints';
import { ApiService } from './api.service';
import { ErrorHandlingService } from './error-handling.service';
import { TokenService } from './token.service';
import { UserRefreshTokenDTO } from 'src/app/DTOs/User/UserRefreshTokenDTO';
import { UserTokenDTO } from 'src/app/DTOs/User/UserTokenDTO';

@Injectable()
export class ErrorHandlerInterceptor implements HttpInterceptor {

  constructor(
    private api: ApiService,
    private errorHandler: ErrorHandlingService,
    private tokenService: TokenService,
  ) {}

  private cloneRequest(request: HttpRequest<unknown>, token: string): HttpRequest<unknown> {
    return request.clone({
      setHeaders: {
        [appConfig.API_HEADER_AUTHORIZATION]: `${appConfig.API_HEADER_BEARER} ${token}`
      }
    });
  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const token = this.tokenService.getToken();
    if (!!token) {
      request = this.cloneRequest(request, token);
    }

    return next.handle(request).pipe(
      catchError(error => {
        if (this.tokenService.isExpired()) {
          this.tokenService.clearToken();
        }
        if (error.status !== 401 && error.url.split('/').some((u: any) => u.includes(endpoints.UserRefreshToken))) {
          this.tokenService.clearToken();
        }
        if (error.status === 401) {
          return this.api.callApi<UserTokenDTO>(endpoints.UserRefreshToken,  {
            refreshToken: this.tokenService.getRefreshToken(),
          } as UserRefreshTokenDTO, 'POST').pipe(
            tap(token => {
              this.tokenService.setToken(token.token);
            }),
            switchMap(token => next.handle(this.cloneRequest(request, token.token))),
          );
        } else {
          return this.errorHandler.handleError({
            error,
            request
          });
        }
      }),
    )
  }
}
