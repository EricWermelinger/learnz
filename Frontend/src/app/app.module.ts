import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from './Material/material.module';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { TextEditorComponent } from './Framework/text-editor/text-editor.component';
import { TextEditorActionBarComponent } from './Framework/text-editor/text-editor-action-bar.component';
import { ErrorHandlingDialogComponent } from './Framework/error-handling-dialog/error-handling-dialog.component';
import { ErrorHandlerInterceptor } from './Framework/API/error-handler.interceptor';
import { LoginComponent } from './Pages/login/login.component';
import { ToastrModule } from 'ngx-toastr';
import { CustomToastyComponent } from './Framework/custom-toasty/custom-toasty.component';
import { SignUpComponent } from './Pages/sign-up/sign-up.component';
import { FileUploadComponent } from './Framework/file-upload/file-upload.component';
import { MatMomentDateModule, MAT_MOMENT_DATE_FORMATS } from '@angular/material-moment-adapter';
import { NavBarComponent } from './Framework/index/nav-bar.component';
import { DashboardComponent } from './Pages/dashboard/dashboard.component';
import { GroupComponent } from './Pages/group/group.component';
import { CreateComponent } from './Pages/create/create.component';
import { LearnComponent } from './Pages/learn/learn.component';
import { ChallengeComponent } from './Pages/challenge/challenge.component';
import { TestComponent } from './Pages/test/test.component';
import { DrawComponent } from './Pages/draw/draw.component';
import { SettingsComponent } from './Pages/settings/settings.component';
import { PasswordChangeDialogComponent } from './Pages/settings/password-change-dialog/password-change-dialog.component';
import { TogetherChatComponent } from './Pages/together/together-chat/together-chat.component';
import { TogetherSwipeComponent } from './Pages/together/together-swipe/together-swipe.component';
import { TogetherAskComponent } from './Pages/together/together-ask/together-ask.component';
import { TogetherConnectComponent } from './Pages/together/together-connect/together-connect.component';
import { TogetherDetailDialogComponent } from './Pages/together/together-detail-dialog/together-detail-dialog.component';
import { GroupInfoDialogComponent } from './Pages/group/group-info-dialog/group-info-dialog.component';
import { GroupChatComponent } from './Pages/group/group-chat/group-chat.component';
import { GroupFilesComponent } from './Pages/group/group-files/group-files.component';
import { FileHistoryDialogComponent } from './Framework/file-upload/file-history-dialog/file-history-dialog.component';
import { CreateSetBannerComponent } from './Pages/create/create-set-banner/create-set-banner.component';
import { CreateSetDialogComponent } from './Pages/create/create-set-dialog/create-set-dialog.component';
import { CreateSetEditComponent } from './Pages/create/create-set-edit/create-set-edit.component';
import { CreateQuestionOpenComponent } from './Pages/create/create-question-types/create-question-open/create-question-open.component';
import { CreateQuestionReadonlyFieldComponent } from './Pages/create/create-question-readonly-field/create-question-readonly-field.component';
import { CreateQuestionMathematicComponent } from './Pages/create/create-question-types/create-question-mathematic/create-question-mathematic.component';
import { CreateQuestionTrueFalseComponent } from './Pages/create/create-question-types/create-question-true-false/create-question-true-false.component';
import { CreateQuestionTextFieldComponent } from './Pages/create/create-question-types/create-question-text-field/create-question-text-field.component';
import { CreateQuestionWordComponent } from './Pages/create/create-question-types/create-question-word/create-question-word.component';
import { CreateQuestionMultipleChoiceComponent } from './Pages/create/create-question-types/create-question-multiple-choice/create-question-multiple-choice.component';
import { CreateQuestionDistributeComponent } from './Pages/create/create-question-types/create-question-distribute/create-question-distribute.component';
import { CreateQuestionDistributeAnswerComponent } from './Pages/create/create-question-types/create-question-distribute/create-question-distribute-answer/create-question-distribute-answer.component';
import { CreateQuestionMathematicVariableComponent } from './Pages/create/create-question-types/create-question-mathematic/create-question-mathematic-variable/create-question-mathematic-variable.component';
import { CreateQuestionMultipleChoiceAnswerComponent } from './Pages/create/create-question-types/create-question-multiple-choice/create-question-multiple-choice-answer/create-question-multiple-choice-answer.component';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

@NgModule({
  declarations: [
    AppComponent,
    CustomToastyComponent,
    ErrorHandlingDialogComponent,
    TextEditorComponent,
    TextEditorActionBarComponent,
    LoginComponent,
    SignUpComponent,
    FileUploadComponent,
    NavBarComponent,
    DashboardComponent,
    GroupComponent,
    CreateComponent,
    LearnComponent,
    ChallengeComponent,
    TestComponent,
    DrawComponent,
    SettingsComponent,
    PasswordChangeDialogComponent,
    TogetherChatComponent,
    TogetherSwipeComponent,
    TogetherAskComponent,
    TogetherConnectComponent,
    TogetherDetailDialogComponent,
    GroupInfoDialogComponent,
    GroupChatComponent,
    GroupFilesComponent,
    FileHistoryDialogComponent,
    CreateSetBannerComponent,
    CreateSetDialogComponent,
    CreateSetEditComponent,
    CreateQuestionOpenComponent,
    CreateQuestionReadonlyFieldComponent,
    CreateQuestionMathematicComponent,
    CreateQuestionTrueFalseComponent,
    CreateQuestionTextFieldComponent,
    CreateQuestionWordComponent,
    CreateQuestionMultipleChoiceComponent,
    CreateQuestionDistributeComponent,
    CreateQuestionDistributeAnswerComponent,
    CreateQuestionMathematicVariableComponent,
    CreateQuestionMultipleChoiceAnswerComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatMomentDateModule,
    TranslateModule.forRoot({
      defaultLanguage: 'en-GB',
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    }),
    ToastrModule.forRoot({
      toastComponent: CustomToastyComponent,
      timeOut: 3000,
      maxOpened: 5,
      newestOnTop: true,
      preventDuplicates: true,
      positionClass: 'toast-bottom-right',
  }),
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ErrorHandlerInterceptor, multi: true },
    { provide: MAT_DATE_LOCALE, useValue: 'de-CH' },
    { provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
