import { Component } from '@angular/core';
import { LearnResultService } from './learn-result.service';

@Component({
  selector: 'app-learn-result',
  templateUrl: './learn-result.component.html',
  styleUrls: ['./learn-result.component.scss']
})
export class LearnResultComponent {

  constructor(
    private resultService: LearnResultService,
  ) { }

}