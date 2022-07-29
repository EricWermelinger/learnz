import { Component, Input } from '@angular/core';
import { CreateSetOverviewDTO } from 'src/app/DTOs/Create/CreateSetOverviewDTO';
import { getSubjects } from 'src/app/Enums/Subject';

@Component({
  selector: 'app-create-set-banner',
  templateUrl: './create-set-banner.component.html',
  styleUrls: ['./create-set-banner.component.scss']
})
export class CreateSetBannerComponent {

  @Input() set: CreateSetOverviewDTO | undefined;
  constructor() { }

  translateSubject(subject: number) {
    return 'Subject.' + getSubjects().filter(s => s.value === subject)[0].key;
  }
}
