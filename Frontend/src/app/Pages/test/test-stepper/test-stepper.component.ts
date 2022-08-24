import { Component } from '@angular/core';
import { TestStepperService } from './test-stepper.service';

@Component({
  selector: 'app-test-stepper',
  templateUrl: './test-stepper.component.html',
  styleUrls: ['./test-stepper.component.scss']
})
export class TestStepperComponent {

  constructor(
    private stepperService: TestStepperService,
  ) { }

}
