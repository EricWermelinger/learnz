import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { appRoutes } from 'src/app/Config/appRoutes';
import { CreateSetOverviewDTO } from 'src/app/DTOs/Create/CreateSetOverviewDTO';
import { getSubjects } from 'src/app/Enums/Subject';

@Component({
  selector: 'app-create-set-banner',
  templateUrl: './create-set-banner.component.html',
  styleUrls: ['./create-set-banner.component.scss']
})
export class CreateSetBannerComponent {

  @Input() set: CreateSetOverviewDTO | undefined;
  constructor(
    private router: Router,
  ) { }

  translateSubject(subject: number) {
    return 'Subject.' + getSubjects().filter(s => s.value === subject)[0].key;
  }

  viewSet(setId: string) {
    this.router.navigate([appRoutes.App, appRoutes.Create, setId]);
  }

  editSet(setId: string, policyEditable: boolean) {
    if (policyEditable) {
      this.router.navigate([appRoutes.App, appRoutes.Create, setId], { queryParams: { [appRoutes.Edit]: true, [appRoutes.EditPolicy]: true }});
    } else {
      this.router.navigate([appRoutes.App, appRoutes.Create, setId], { queryParams: { [appRoutes.Edit]: true }});
    }
  }
}
