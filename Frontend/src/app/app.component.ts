import { AfterViewInit, Component } from '@angular/core';
import { filter, map, switchMap } from 'rxjs';
import { AppService } from './app.service';
import { endpoints } from './Config/endpoints';
import { UserProfileGetDTO } from './DTOs/User/UserProfileGetDTO';
import { ApiService } from './Framework/API/api.service';
import { DarkThemeService } from './Framework/dark-theme/dark-theme.service';
import { LanguagesService } from './Framework/Languages/languages.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements AfterViewInit {
  title = 'learnz';

  constructor (
    private app: AppService,
    private api: ApiService,
    private darkThemeService: DarkThemeService,
    private languageService: LanguagesService,
  ) {
    this.app.isLoggedIn$().pipe(
      filter(isLoggedIn => isLoggedIn),
      switchMap(_ => this.api.callApi<UserProfileGetDTO>(endpoints.UserProfile, {}, 'GET')),
      map(userProfile => {
        return {
          language: this.languageService.getLanguageKey(userProfile.language),
          darkTheme: userProfile.darkTheme,
        }
      })
    ).subscribe(value => {
      this.darkThemeService.setDarkTheme(value.darkTheme);
      this.darkThemeService.applyDarkTheme(value.darkTheme);
      this.languageService.selectLanguage(value.language);
    });

    this.darkThemeService.isDarkTheme$.asObservable().subscribe((isDarkTheme) => {
      this.darkThemeService.applyDarkTheme(isDarkTheme);
    });
  }

  ngAfterViewInit(): void {
    this.darkThemeService.applyDarkTheme(this.darkThemeService.getDarkTheme());
  }
}
