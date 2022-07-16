import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { appRoutes } from 'src/app/Config/appRoutes';
import { TogetherOverviewUserProfileDTO } from 'src/app/DTOs/Together/TogetherOverviewUserProfileDTO';
import { TogetherConnectService } from './together-connect.service';

@Component({
  selector: 'app-together-connect',
  templateUrl: './together-connect.component.html',
  styleUrls: ['./together-connect.component.scss']
})
export class TogetherConnectComponent {

  overview$: Observable<TogetherOverviewUserProfileDTO[]>;

  constructor(
    private connectService: TogetherConnectService,
    private router: Router,
  ) {
    this.overview$ = this.connectService.getConnections();
    this.overview$.subscribe(x => console.log(x))
  }

  navigateToChat(userId: string) {
    this.router.navigate([appRoutes.App, appRoutes.TogetherChat, userId]);
  }

  isEmpty(data: TogetherOverviewUserProfileDTO[]) {
    return data.length === 0;
  }
}
