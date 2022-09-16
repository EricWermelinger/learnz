import { Component, EventEmitter, Output } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { combineLatest, filter, map, BehaviorSubject } from 'rxjs';
import { AppService } from 'src/app/app.service';
import { teachuRoutes } from 'src/app/Config/teachuRoutes';
import { environment } from 'src/environments/environment';
import { appRoutes } from './../../Config/appRoutes';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent {

  routes$ = new BehaviorSubject<AppRoute[]>([]);

  @Output() activeRoute = new EventEmitter<string>();

  private routes = [
    { route: teachuRoutes.Dashboard, isTeachu: true, key: 'teachuRoutes.Dashboard', icon: 'home', navigation: [teachuRoutes.App, teachuRoutes.Dashboard], onLoggedIn: true, layer: 1, isParent: false },
    { route: teachuRoutes.SchoolInfos, isTeachu: true, key: 'teachuRoutes.SchoolInfos', icon: 'info', navigation: [teachuRoutes.App, teachuRoutes.SchoolInfos], onLoggedIn: true, layer: 1, isParent: false },
    { route: teachuRoutes.Timetable, isTeachu: true, key: 'teachuRoutes.Timetable', icon: 'calendar_today', navigation: [teachuRoutes.App, teachuRoutes.Timetable], onLoggedIn: true, layer: 1, isParent: false },
    { route: teachuRoutes.Classlist, isTeachu: true, key: 'teachuRoutes.Classlist', icon: 'list', navigation: [teachuRoutes.App, teachuRoutes.Classlist], onLoggedIn: true, layer: 1, isParent: false },
    { route: teachuRoutes.Grades, isTeachu: true, key: 'teachuRoutes.Grades', icon: 'grade', navigation: [teachuRoutes.App, teachuRoutes.Grades], onLoggedIn: true, layer: 1, isParent: false },
    { route: teachuRoutes.Absences, isTeachu: true, key: 'teachuRoutes.Absences', icon: 'not_interested', navigation: [teachuRoutes.App, teachuRoutes.Absences], onLoggedIn: true, layer: 1, isParent: false },
    // { route: teachuRoutes.Chat, isTeachu: true, key: 'teachuRoutes.Chat', icon: 'chat', navigation: [teachuRoutes.App, teachuRoutes.Chat], onLoggedIn: true, layer: 1, isParent: false },
    { route: teachuRoutes.UserSettings, isTeachu: true, key: 'teachuRoutes.UserSettings', icon: 'settings', navigation: [teachuRoutes.App, teachuRoutes.UserSettings], onLoggedIn: true, layer: 1, isParent: false },

    // { route: appRoutes.Login, isTeachu: false, key: 'login.login', icon: 'verified_user', navigation: [appRoutes.Login], onLoggedIn: false, layer: 1, isParent: false },
    // { route: appRoutes.SignUp, isTeachu: false, key: 'signUp.signUp', icon: 'account_box', navigation: [appRoutes.SignUp], onLoggedIn: false, layer: 1, isParent: false },

    // { route: appRoutes.Dashboard, isTeachu: false, key: 'dashboard.dashboard', icon: 'dashboard', navigation: [appRoutes.App, appRoutes.Dashboard], onLoggedIn: true, layer: 1, isParent: false },
    
    { route: appRoutes.Together, isTeachu: false, key: 'together.together', icon: 'people', navigation: [], onLoggedIn: true, layer: 1, isParent: true },
    { route: appRoutes.TogetherAsk, isTeachu: false, key: 'together.ask', icon: 'question_answer', navigation: [appRoutes.App, appRoutes.TogetherAsk], onLoggedIn: true, layer: 2, isParent: false, parent: appRoutes.Together },
    { route: appRoutes.TogetherConnect, isTeachu: false, key: 'together.connect', icon: 'person_add', navigation: [appRoutes.App, appRoutes.TogetherConnect], onLoggedIn: true, layer: 2, isParent: false, parent: appRoutes.Together },
    { route: appRoutes.TogetherChat, isTeachu: false, key: 'together.chat', icon: '', navigation: [appRoutes.App, appRoutes.TogetherChat], onLoggedIn: null, layer: 2, isParent: false, parent: appRoutes.Together },
    { route: appRoutes.TogetherSwipe, isTeachu: false, key: 'together.swipe', icon: 'swap_horiz', navigation: [appRoutes.App, appRoutes.TogetherSwipe], onLoggedIn: true, layer: 2, isParent: false, parent: appRoutes.Together },
    
    { route: appRoutes.Group, isTeachu: false, key: 'group.group', icon: 'public', navigation: [appRoutes.App, appRoutes.Group], onLoggedIn: true, layer: 1, isParent: false },
    { route: appRoutes.GroupChat, isTeachu: false, key: 'group.chat', icon: '', navigation: [appRoutes.App, appRoutes.GroupChat], onLoggedIn: null, layer: 2, isParent: false, parent: appRoutes.Group },
    { route: appRoutes.GroupFiles, isTeachu: false, key: 'group.files', icon: '', navigation: [appRoutes.App, appRoutes.GroupFiles], onLoggedIn: null, layer: 2, isParent: false, parent: appRoutes.Group },
    
    { route: appRoutes.Create, isTeachu: false, key: 'create.create', icon: 'build', navigation: [appRoutes.App, appRoutes.Create], onLoggedIn: true, layer: 1, isParent: false },
    { route: appRoutes.Learn, isTeachu: false, key: 'learn.learn', icon: 'school', navigation: [appRoutes.App, appRoutes.Learn], onLoggedIn: true, layer: 1, isParent: false },
    { route: appRoutes.Challenge, isTeachu: false, key: 'challenge.challenge', icon: 'poll', navigation: [appRoutes.App, appRoutes.Challenge], onLoggedIn: true, layer: 1, isParent: false },
    { route: appRoutes.Test, isTeachu: false, key: 'test.test', icon: 'assignment_turned_in', navigation: [appRoutes.App, appRoutes.Test], onLoggedIn: true, layer: 1, isParent: false },
    { route: appRoutes.Draw, isTeachu: false, key: 'draw.draw', icon: 'edit', navigation: [appRoutes.App, appRoutes.Draw], onLoggedIn: true, layer: 1, isParent: false },
    { route: appRoutes.Settings, isTeachu: false, key: 'settings.settings', icon: 'settings', navigation: [appRoutes.App, appRoutes.Settings], onLoggedIn: true, layer: 1, isParent: false },
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
    ).subscribe(routes => {
      this.routes$.next(routes.filter(r => r.layer === 1 && r.onLoggedIn !== null));
      const route = routes.find(r => this.isActive(r));
      this.activeRoute.emit(route?.key);
    });
  }

  navigate(route: string[], changeFrontend: boolean) {
    if (changeFrontend) {
      window.location.href = environment.URL_TEACHU + route.join('/');
    } else {
      this.router.navigate(route);
    }
  }

  isActive(route: AppRoute) {
    return this.router.url.split('/').some(path => path === route.route);
  }

  selectParent(route: AppRoute) {
    const previous = this.routes$.value.filter(r => r.layer === 1);
    let next: AppRoute[] = [];
    const close = this.routes$.value.some(r => r.parent === route.route && r.layer === 2);
    if (close) {
      next = previous.filter(r => r.parent !== route.route);
    } else {
      for (const r of previous) {
        next.push(r);
        if (r.route === route.route) {
          next = [...next, ...this.routes.filter(r => r.parent === route.route && r.onLoggedIn !== null)];
        }
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
  isTeachu: boolean;
}