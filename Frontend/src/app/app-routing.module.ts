import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { appRoutes } from './Config/appRoutes';
import { AppGuard } from './Framework/API/app.guard';
import { ChallengeComponent } from './Pages/challenge/challenge.component';
import { CreateComponent } from './Pages/create/create.component';
import { DashboardComponent } from './Pages/dashboard/dashboard.component';
import { DrawComponent } from './Pages/draw/draw.component';
import { GroupComponent } from './Pages/group/group.component';
import { LearnComponent } from './Pages/learn/learn.component';
import { LoginComponent } from './Pages/login/login.component';
import { SignUpComponent } from './Pages/sign-up/sign-up.component';
import { TestComponent } from './Pages/test/test.component';
import { TogetherComponent } from './Pages/together/together.component';

const routes: Routes = [
  { path: '', redirectTo: `/${appRoutes.Login}`, pathMatch: 'full' },
  { path: appRoutes.Login, component: LoginComponent },
  { path: appRoutes.SignUp, component: SignUpComponent },
  {
    path: appRoutes.App,
    canActivate: [AppGuard],
    children: [
      { path: appRoutes.Dashboard, component: DashboardComponent },
      { path: appRoutes.Together, component: TogetherComponent },
      { path: appRoutes.Group, component: GroupComponent },
      { path: appRoutes.Create, component: CreateComponent },
      { path: appRoutes.Learn, component: LearnComponent },
      { path: appRoutes.Challenge, component: ChallengeComponent },
      { path: appRoutes.Test, component: TestComponent },
      { path: appRoutes.Draw, component: DrawComponent },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
