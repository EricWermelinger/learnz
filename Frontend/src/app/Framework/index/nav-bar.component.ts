import { Component } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { combineLatest, distinctUntilChanged, filter, interval, map, Observable, startWith, tap } from 'rxjs'
import { TokenService } from 'src/app/Framework/API/token.service';
import { appRoutes } from './../../Config/appRoutes';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent {

  routes$: Observable<SelectableRoute[]>;

  private routes = [
    { route: appRoutes.Login, key: 'login.login', icon: 'verified_user', navigation: [appRoutes.Login], onLoggedIn: false },
    { route: appRoutes.SignUp, key: 'signUp.signUp', icon: 'account_box', navigation: [appRoutes.SignUp], onLoggedIn: false },
    { route: appRoutes.Dashboard, key: 'dashboard.dashboard', icon: 'dashboard', navigation: [appRoutes.App, appRoutes.Dashboard], onLoggedIn: true },
    { route: appRoutes.Together, key: 'together.together', icon: 'people', navigation: [appRoutes.App, appRoutes.Together], onLoggedIn: true },
    { route: appRoutes.Group, key: 'group.group', icon: 'public', navigation: [appRoutes.App, appRoutes.Group], onLoggedIn: true },
    { route: appRoutes.Create, key: 'create.create', icon: 'build', navigation: [appRoutes.App, appRoutes.Create], onLoggedIn: true },
    { route: appRoutes.Learn, key: 'learn.learn', icon: 'school', navigation: [appRoutes.App, appRoutes.Learn], onLoggedIn: true },
    { route: appRoutes.Challenge, key: 'challenge.challenge', icon: 'poll', navigation: [appRoutes.App, appRoutes.Challenge], onLoggedIn: true },
    { route: appRoutes.Test, key: 'test.test', icon: 'assignment_turned_in', navigation: [appRoutes.App, appRoutes.Test], onLoggedIn: true },
    { route: appRoutes.Draw, key: 'draw.draw', icon: 'edit', navigation: [appRoutes.App, appRoutes.Draw], onLoggedIn: true },
    { route: appRoutes.Settings, key: 'settings.settings', icon: 'settings', navigation: [appRoutes.App, appRoutes.Settings], onLoggedIn: true },
  ] as AppRoute[];

  constructor(
    private tokenService: TokenService,
    private router: Router,
  ) {
    const loggedIn$ = interval(1000).pipe(
      startWith(false),
      map(_ => !!this.tokenService.getToken()),
      distinctUntilChanged(),
    );

    this.routes$ = combineLatest([
      loggedIn$,
      this.router.events.pipe(
        filter(event => event instanceof NavigationEnd),
        map(event => (event as NavigationEnd).url),
        map(url => url.split('/')),
      )
    ]).pipe(
      map(([isLoggedIn, url]) => [isLoggedIn, this.getActiveRoute(url)]),
      map(([isLoggedIn, url]) =>
        this.routes
          .filter(r => r.onLoggedIn === null || r.onLoggedIn === isLoggedIn)
          .map(r => {
              return {
              route: r,
              selected: url === r.route
            } as SelectableRoute;
        })
      )
    );
  }

  navigate(route: string[]) {
    this.router.navigate(route);
  }

  private getActiveRoute(url: string[]){
    const urlParts = url.filter(segment => segment !== '' && segment !== appRoutes.App);
    const filteredRoutes = this.routes.filter(route => urlParts.some(part => route.route.includes(part)));
    if (filteredRoutes.length > 0) {
      return filteredRoutes[0].route;
    } else {
      return '';
    }
  }
}

interface SelectableRoute {
  route: AppRoute;
  selected: boolean;
};

interface AppRoute {
  route: string | string[];
  key: string;
  icon: string;
  navigation: string[];
  onLoggedIn: boolean | null;
}