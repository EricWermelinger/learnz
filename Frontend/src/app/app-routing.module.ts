import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { appRoutes } from './Config/appRoutes';
import { AppGuard } from './Framework/API/app.guard';
import { LoginComponent } from './Pages/login/login.component';
import { SignUpComponent } from './Pages/sign-up/sign-up.component';

const routes: Routes = [
  { path: '', redirectTo: `/${appRoutes.Login}`, pathMatch: 'full' },
  { path: appRoutes.Login, component: LoginComponent },
  { path: appRoutes.SignUp, component: SignUpComponent },
  {
    path: appRoutes.App,
    canActivate: [AppGuard],
    children: [],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
