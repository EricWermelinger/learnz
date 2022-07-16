import { Injectable } from '@angular/core';
import { interval, startWith, map, distinctUntilChanged } from 'rxjs';
import { TokenService } from './Framework/API/token.service';

@Injectable({
  providedIn: 'root'
})
export class AppService {

  constructor(
    private tokenService: TokenService,
  ) { }

  isLoggedIn$() {
    const loggedIn$ = interval(1000).pipe(
      startWith(false),
      map(_ => !!this.tokenService.getToken()),
      distinctUntilChanged(),
    );
    return loggedIn$;
  }  
}
