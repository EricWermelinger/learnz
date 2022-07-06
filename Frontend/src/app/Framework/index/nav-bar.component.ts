import { Component } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { Observable, combineLatest, filter, map, BehaviorSubject } from 'rxjs';
import { AppService } from 'src/app/app.service';
import { appRoutes } from './../../Config/appRoutes';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent {

  routes$ = new BehaviorSubject<AppRoute[]>([]);

  private routes = [
    { route: appRoutes.Login, key: 'login.login', icon: 'verified_user', navigation: [appRoutes.Login], onLoggedIn: false, layer: 1, isParent: false },
    { route: appRoutes.SignUp, key: 'signUp.signUp', icon: 'account_box', navigation: [appRoutes.SignUp], onLoggedIn: false, layer: 1, isParent: false },
    { route: appRoutes.Dashboard, key: 'dashboard.dashboard', icon: 'dashboard', navigation: [appRoutes.App, appRoutes.Dashboard], onLoggedIn: true, layer: 1, isParent: false },
    { route: appRoutes.Together, key: 'together.together', icon: 'people', navigation: [], onLoggedIn: true, layer: 1, isParent: true },
    { route: appRoutes.TogetherAsk, key: 'together.ask', icon: 'question_answer', navigation: [appRoutes.App, appRoutes.TogetherAsk], onLoggedIn: true, layer: 2, isParent: false, parent: appRoutes.Together },
    { route: appRoutes.TogetherConnect, key: 'together.connect', icon: 'person_add', navigation: [appRoutes.App, appRoutes.TogetherConnect], onLoggedIn: true, layer: 2, isParent: false, parent: appRoutes.Together },
    { route: appRoutes.TogetherSwipe, key: 'together.swipe', icon: 'swap_horiz', navigation: [appRoutes.App, appRoutes.TogetherSwipe], onLoggedIn: true, layer: 2, isParent: false, parent: appRoutes.Together },
    { route: appRoutes.Group, key: 'group.group', icon: 'public', navigation: [appRoutes.App, appRoutes.Group], onLoggedIn: true, layer: 1, isParent: false },
    { route: appRoutes.Create, key: 'create.create', icon: 'build', navigation: [appRoutes.App, appRoutes.Create], onLoggedIn: true, layer: 1, isParent: false },
    { route: appRoutes.Learn, key: 'learn.learn', icon: 'school', navigation: [appRoutes.App, appRoutes.Learn], onLoggedIn: true, layer: 1, isParent: false },
    { route: appRoutes.Challenge, key: 'challenge.challenge', icon: 'poll', navigation: [appRoutes.App, appRoutes.Challenge], onLoggedIn: true, layer: 1, isParent: false },
    { route: appRoutes.Test, key: 'test.test', icon: 'assignment_turned_in', navigation: [appRoutes.App, appRoutes.Test], onLoggedIn: true, layer: 1, isParent: false },
    { route: appRoutes.Draw, key: 'draw.draw', icon: 'edit', navigation: [appRoutes.App, appRoutes.Draw], onLoggedIn: true, layer: 1, isParent: false },
    { route: appRoutes.Settings, key: 'settings.settings', icon: 'settings', navigation: [appRoutes.App, appRoutes.Settings], onLoggedIn: true, layer: 1, isParent: false },
  ] as AppRoute[];

  constructor(
    private app: AppService,
    private router: Router,
  ) {
    const loggedIn$ = this.app.isLoggedIn$();

    combineLatest([
      loggedIn$,
      this.router.events.pipe(filter(event => event instanceof NavigationEnd)),
    ]).pipe(
      map(([isLoggedIn, _]) => this.routes.filter(r => r.onLoggedIn === null || r.onLoggedIn === isLoggedIn)),
      map(routes => routes.filter(r => r.layer === 1)),
    ).subscribe(routes => this.routes$.next(routes));
  }

  navigate(route: string[]) {
    this.router.navigate(route);
  }

  isActive(route: AppRoute) {
    return this.router.url.split('/').some(path => path === route.route);
  }

  selectParent(route: AppRoute) {
    const previous = this.routes$.value.filter(r => r.layer === 1);
    let next: AppRoute[] = [];
    for (const r of previous) {
      next.push(r);
      if (r.route === route.route) {
        next = [...next, ...this.routes.filter(r => r.parent === route.route)];
      }
    }
    this.routes$.next(next);
  }
}

interface AppRoute {
  route: string;
  key: string;
  icon: string;
  navigation: string[];
  onLoggedIn: boolean | null;
  layer: number;
  isParent: boolean;
  parent: string;
}