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
import { MAT_DATE_LOCALE } from '@angular/material/core';
import { LanguageLookupComponent } from './Framework/Languages/language-lookup.component';
import { TextEditorComponent } from './Framework/text-editor/text-editor.component';
import { TextEditorActionBarComponent } from './Framework/text-editor/text-editor-action-bar.component';
import { ErrorHandlingDialogComponent } from './Framework/error-handling-dialog/error-handling-dialog.component';
import { ErrorHandlerInterceptor } from './Framework/API/error-handler.interceptor';
import { LoginComponent } from './Pages/login/login.component';
import { ToastrModule } from 'ngx-toastr';
import { CustomToastyComponent } from './Framework/custom-toasty/custom-toasty.component';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

@NgModule({
  declarations: [
    AppComponent,
    CustomToastyComponent,
    ErrorHandlingDialogComponent,
    LanguageLookupComponent,
    TextEditorComponent,
    TextEditorActionBarComponent,
    LoginComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
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
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorHandlerInterceptor,
      multi: true
    },
    {
        provide: MAT_DATE_LOCALE,
        useValue: 'de-DE'
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
