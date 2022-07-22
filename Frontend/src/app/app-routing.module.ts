import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { appRoutes } from './Config/appRoutes';
import { AppGuard } from './Framework/API/app.guard';
import { ChallengeComponent } from './Pages/challenge/challenge.component';
import { CreateComponent } from './Pages/create/create.component';
import { DashboardComponent } from './Pages/dashboard/dashboard.component';
import { DrawComponent } from './Pages/draw/draw.component';
import { GroupChatComponent } from './Pages/group/group-chat/group-chat.component';
import { GroupFilesComponent } from './Pages/group/group-files/group-files.component';
import { GroupComponent } from './Pages/group/group.component';
import { LearnComponent } from './Pages/learn/learn.component';
import { LoginComponent } from './Pages/login/login.component';
import { SettingsComponent } from './Pages/settings/settings.component';
import { SignUpComponent } from './Pages/sign-up/sign-up.component';
import { TestComponent } from './Pages/test/test.component';
import { TogetherAskComponent } from './Pages/together/together-ask/together-ask.component';
import { TogetherChatComponent } from './Pages/together/together-chat/together-chat.component';
import { TogetherConnectComponent } from './Pages/together/together-connect/together-connect.component';
import { TogetherSwipeComponent } from './Pages/together/together-swipe/together-swipe.component';

const routes: Routes = [
  { path: '', redirectTo: `/${appRoutes.Login}`, pathMatch: 'full' },
  { path: appRoutes.Login, component: LoginComponent },
  { path: appRoutes.SignUp, component: SignUpComponent },
  {
    path: appRoutes.App,
    canActivate: [AppGuard],
    children: [
      { path: appRoutes.Dashboard, component: DashboardComponent },
      { path: appRoutes.TogetherAsk, component: TogetherAskComponent },
      { path: `${appRoutes.TogetherChat}/:${appRoutes.TogetherChatId}`, component: TogetherChatComponent },
      { path: appRoutes.TogetherConnect, component: TogetherConnectComponent },
      { path: appRoutes.TogetherSwipe, component: TogetherSwipeComponent },
      { path: appRoutes.Group, component: GroupComponent },
      { path: `${appRoutes.GroupChat}/:${appRoutes.GroupChatId}`, component: GroupChatComponent },
      { path: `${appRoutes.GroupFiles}/:${appRoutes.GroupFilesId}`, component: GroupFilesComponent },
      { path: appRoutes.Create, component: CreateComponent },
      { path: appRoutes.Learn, component: LearnComponent },
      { path: appRoutes.Challenge, component: ChallengeComponent },
      { path: appRoutes.Test, component: TestComponent },
      { path: appRoutes.Draw, component: DrawComponent },
      { path: appRoutes.Settings, component: SettingsComponent },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
