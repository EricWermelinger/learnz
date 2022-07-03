import { Component } from '@angular/core';
import { CreateService } from './create.service';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.scss']
})
export class CreateComponent {

  constructor(
    private createService: CreateService,
  ) { }

}