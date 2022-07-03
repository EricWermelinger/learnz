import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { map, Observable } from 'rxjs';
import { appRoutes } from 'src/app/Config/appRoutes';
import { UserSignUpDTO } from 'src/app/DTOs/User/UserSignUpDTO';
import { getGrades } from 'src/app/Enums/Grade';
import { getSubjects } from 'src/app/Enums/Subject';
import { ignoreUTC } from 'src/app/Framework/Helpers/DateHelpers';
import { imageTypes } from 'src/app/Framework/Helpers/FileTypesHelper';
import { LanguagesService } from 'src/app/Framework/Languages/languages.service';
import { SignUpService } from './sign-up.service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent {

  grades = getGrades();
  subjects = getSubjects();
  formGroup: FormGroup;
  maxDate = new Date();
  errorVisible$: Observable<boolean>;

  constructor(
    private signUpService: SignUpService,
    private formBuilder: FormBuilder,
    private languageService: LanguagesService,
    private router: Router,
  ) {
    this.formGroup = this.formBuilder.group({
      username: ['', Validators.required],
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      birthdate: [null, Validators.required],
      grade: [null, Validators.required],
      profileImagePath: '',
      information: ['', Validators.required],
      languageKey: ['', Validators.required],
      password: ['', Validators.required],
      goodSubject1: [null, Validators.required],
      goodSubject2: [null, Validators.required],
      goodSubject3: [null, Validators.required],
      badSubject1: [null, Validators.required],
      badSubject2: [null, Validators.required],
      badSubject3: [null, Validators.required],
    });
    this.formGroup.addValidators(() => this.validateDifferentSubjects());
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

  signUp() {
    const value = {
      ...this.formGroup.value,
      language: this.languageService.getLanguageIndex(this.formGroup.value.languageKey),
      birthdate: ignoreUTC(this.formGroup.value.birthdate),
    } as UserSignUpDTO;
    this.signUpService.save(value);
  }

  fileTypes(): string {
    return imageTypes();
  }

  loginInstead() {
    this.router.navigate([appRoutes.Login]);
  }

  allLanguages() { return this.languageService.allLanguages(); }
  selectedLanguage() { return this.languageService.getSelectedLanguage(); }
  selectLanguage(language: string) { this.languageService.selectLanguage(language); }
  isLanguageDisabled(language: string) { return this.languageService.selectableLanguages().filter(l => l.key === language).length === 0; }
}
