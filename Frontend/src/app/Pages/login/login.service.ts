import { Injectable } from '@angular/core';
import { endpoints } from 'src/app/Config/endpoints';
import { TokenDTO } from 'src/app/DTOs/User/TokenDTO';
import { UserLoginDTO } from 'src/app/DTOs/User/UserLoginDTO';
import { UserRefreshTokenDTO } from 'src/app/DTOs/User/UserRefreshTokenDTO';
import { ApiService } from 'src/app/Framework/API/api.service';
import { TokenService } from 'src/app/Framework/API/token.service';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(
    private api: ApiService,
    private tokenService: TokenService
  ) { }

  login(value: UserLoginDTO) {
    this.api.callApi<TokenDTO>(endpoints.UserLogin, value, 'POST').subscribe(token => this.tokenService.patchToken(token));
  }

  loginByRefreshToken(refreshToken: string) {
    this.api.callApi<TokenDTO>(endpoints.UserRefreshToken, {
      refreshToken
    } as UserRefreshTokenDTO, 'POST').subscribe(token => this.tokenService.patchToken(token));
  }
}
