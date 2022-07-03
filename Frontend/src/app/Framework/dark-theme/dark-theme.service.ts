import { OverlayContainer } from '@angular/cdk/overlay';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { appConfig } from 'src/app/Config/appConfig';

@Injectable({
  providedIn: 'root'
})
export class DarkThemeService {

  isDarkTheme$ = new BehaviorSubject<boolean>(false);

  constructor(
    private overlay: OverlayContainer
  ) {}
  
  setDarkTheme(isDarkTheme: boolean) {
    localStorage.setItem(appConfig.APPLICATION_DARK_THEME, isDarkTheme.valueOf().toString());
    this.isDarkTheme$.next(isDarkTheme);
  }

  getDarkTheme(): boolean {
    const isDarkTheme = localStorage.getItem(appConfig.APPLICATION_DARK_THEME);
    if (!isDarkTheme) {
      return false;
    }
    return isDarkTheme === 'true';
  }

  applyDarkTheme(isDarkTheme: boolean) {
    document.body.classList.toggle('darkTheme', isDarkTheme);
    this.overlay.getContainerElement().classList.toggle('darkTheme', isDarkTheme);

    const styles: string[] = ['primary', 'accent', 'white', 'grey', 'grey-dark', 'black', 'warn'];
    const setTo: string = isDarkTheme ? 'dark' : 'light';
    let r = document.querySelector(':root') as HTMLElement;
    let computed = getComputedStyle(r);

    styles.forEach(style => {
      let newColor = computed.getPropertyValue(`--learnz-${setTo}-${style}`);
      r.style.setProperty(`--learnz-${style}`, newColor);
    });
  }
}
