import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserSignUpDTO } from 'src/app/DTOs/User/UserSignUpDTO';
import { imageTypes } from 'src/app/Framework/Helpers/FileTypesHelper';
import { LanguagesService } from 'src/app/Framework/Languages/languages.service';
import { FormGroupTyped } from 'src/app/Material/types';
import { SignUpService } from './sign-up.service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent {

  formGroup: FormGroup;

  constructor(
    private signUpService: SignUpService,
    private formBuilder: FormBuilder,
    private languageService: LanguagesService,
  ) {
    this.formGroup = this.formBuilder.group({
      username: ['', Validators.required],
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      birthdate: [null, Validators.required],
      grade: [null, Validators.required],
      profileImagePath: [null, Validators.required],
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
  }

  signUp() {
    const value = {
      ...this.formGroup.value,
      language: this.languageService.getLanguageIndex(this.formGroup.value.languageKey),
    } as UserSignUpDTO;
    this.signUpService.save(value);
  }

  fileTypes(): string {
    return imageTypes();
  }
}
