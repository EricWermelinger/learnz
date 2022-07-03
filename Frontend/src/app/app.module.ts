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
import { TogetherComponent } from './Pages/together/together.component';
import { GroupComponent } from './Pages/group/group.component';
import { CreateComponent } from './Pages/create/create.component';
import { LearnComponent } from './Pages/learn/learn.component';
import { ChallengeComponent } from './Pages/challenge/challenge.component';
import { TestComponent } from './Pages/test/test.component';
import { DrawComponent } from './Pages/draw/draw.component';

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
    TogetherComponent,
    GroupComponent,
    CreateComponent,
    LearnComponent,
    ChallengeComponent,
    TestComponent,
    DrawComponent,
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
