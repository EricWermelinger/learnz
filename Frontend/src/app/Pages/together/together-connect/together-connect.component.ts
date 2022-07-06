import { Component } from '@angular/core';
import { TogetherConnectService } from './together-connect.service';

@Component({
  selector: 'app-together-connect',
  templateUrl: './together-connect.component.html',
  styleUrls: ['./together-connect.component.scss']
})
export class TogetherConnectComponent {

  constructor(
    private connectService: TogetherConnectService,
  ) { }

}
