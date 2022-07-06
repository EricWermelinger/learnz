import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { map, Observable } from 'rxjs';
import { UserDarkThemeDTO } from 'src/app/DTOs/User/UserDarkThemeDTO';
import { UserProfileGetDTO } from 'src/app/DTOs/User/UserProfileGetDTO';
import { UserProfileUploadDTO } from 'src/app/DTOs/User/UserProfileUploadDTO';
import { getGrades } from 'src/app/Enums/Grade';
import { getSubjects } from 'src/app/Enums/Subject';
import { ignoreUTC } from 'src/app/Framework/Helpers/DateHelpers';
import { imageTypes } from 'src/app/Framework/Helpers/FileTypesHelper';
import { LanguagesService } from 'src/app/Framework/Languages/languages.service';
import { PasswordChangeDialogComponent } from './password-change-dialog/password-change-dialog.component';
import { SettingsService } from './settings.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent {

  formGroup: FormGroup;
  grades = getGrades();
  subjects = getSubjects();
  maxDate = new Date();
  errorVisible$: Observable<boolean>;
  settings$: Observable<UserProfileGetDTO>;

  constructor(
    private settingsService: SettingsService,
    private languageService: LanguagesService,
    private formBuilder: FormBuilder,
    private dialog: MatDialog,
  ) {
    this.formGroup = this.formBuilder.group({
      username: ['', Validators.required],
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      birthdate: [null, Validators.required],
      grade: [null, Validators.required],
      profileImagePath: '',
      profileImageName: '',
      information: ['', Validators.required],
      languageKey: ['', Validators.required],
      goodSubject1: [null, Validators.required],
      goodSubject2: [null, Validators.required],
      goodSubject3: [null, Validators.required],
      badSubject1: [null, Validators.required],
      badSubject2: [null, Validators.required],
      badSubject3: [null, Validators.required],
      darkTheme: false,
    });
    this.formGroup.addValidators(() => this.validateDifferentSubjects());
    this.settings$ = this.settingsService.getUserProfile();
    this.settings$.subscribe((user: any) => {
      this.formGroup.patchValue(user);
      this.formGroup.patchValue({
        birthdate: user.birthdate,
        languageKey: this.languageService.getLanguageKey(user.language),
      });
    });
    this.errorVisible$ = this.formGroup.valueChanges.pipe(
      map(_ => this.validateDifferentSubjects() !== null),
    );
  }

  validateDifferentSubjects() {
    const subjects = [
      this.formGroup.value.goodSubject1,
      this.formGroup.value.goodSubject2,
      this.formGroup.value.goodSubject3,
      this.formGroup.value.badSubject1,
      this.formGroup.value.badSubject2,
      this.formGroup.value.badSubject3,
    ];
    if (subjects.filter(s => s === null).length > 0) {
      return null;
    }
    const distinctSubjects = subjects.filter((v, i, a) => a.indexOf(v) === i).length === 6;
    if (!distinctSubjects) {
      return { distinctSubjects: true };
    }
    return null;
  }

  save() {
    const value = {
      ...this.formGroup.value,
      language: this.languageService.getLanguageIndex(this.formGroup.value.languageKey),
      birthdate: ignoreUTC(this.formGroup.value.birthdate),
    } as UserProfileUploadDTO;
    this.settingsService.save(value);
  }

  setDarkTheme(darkTheme: boolean) {
    this.settingsService.setDarkTheme({ darkTheme } as UserDarkThemeDTO);
  }

  fileTypes(): string {
    return imageTypes();
  }

  changePassword() {
    this.dialog.open(PasswordChangeDialogComponent, {});
  }

  allLanguages() { return this.languageService.allLanguages(); }
  selectedLanguage() { return this.languageService.getSelectedLanguage(); }
  selectLanguage(language: string) {
    this.languageService.selectLanguage(language);
    this.settingsService.saveLanguage(this.languageService.getLanguageIndex(language));
  }
  isLanguageDisabled(language: string) { return this.languageService.selectableLanguages().filter(l => l.key === language).length === 0; }
}
