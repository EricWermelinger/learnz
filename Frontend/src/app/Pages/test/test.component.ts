import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { appRoutes } from 'src/app/Config/appRoutes';
import { TestDTO } from 'src/app/DTOs/Test/TestDTO';
import { TestService } from './test.service';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.scss']
})
export class TestComponent {

  openTests$: Observable<TestDTO[]>;
  closedTests$: Observable<TestDTO[]>;

  constructor(
    private testService: TestService,
    private router: Router,
  ) {
    this.openTests$ = this.testService.openTests$();
    this.closedTests$ = this.testService.closedTests$();
  }

  testStart(testId: string) {
    this.testService.testStart$(testId).subscribe(_ => {
      this.router.navigate([appRoutes.App, appRoutes.Test, appRoutes.TestStepper, testId]);
    });
  }
  
  testStartVisibility(testId: string, visible: boolean) {
    this.testService.testVisibility$(testId, visible);
  }
}
