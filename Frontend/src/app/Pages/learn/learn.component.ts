import { Component } from '@angular/core';
import { LearnService } from './learn.service';

@Component({
  selector: 'app-learn',
  templateUrl: './learn.component.html',
  styleUrls: ['./learn.component.scss']
})
export class LearnComponent {

  constructor(
    private learnService: LearnService,
  ) { }

}
