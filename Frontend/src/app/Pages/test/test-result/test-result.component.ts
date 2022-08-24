import { Component } from '@angular/core';
import { TestResultService } from './test-result.service';

@Component({
  selector: 'app-test-result',
  templateUrl: './test-result.component.html',
  styleUrls: ['./test-result.component.scss']
})
export class TestResultComponent {

  constructor(
    private resultService: TestResultService,
  ) { }

}
