import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { appRoutes } from 'src/app/Config/appRoutes';
import { TestResultOverviewDTO } from 'src/app/DTOs/Test/TestResultOverviewDTO';
import { TestResultOverviewService } from './test-result-overview.service';

@Component({
  selector: 'app-test-result-overview',
  templateUrl: './test-result-overview.component.html',
  styleUrls: ['./test-result-overview.component.scss']
})
export class TestResultOverviewComponent {

  resultOverview$: Observable<TestResultOverviewDTO>;
  testId: string;

  constructor(
    private resultOverviewService: TestResultOverviewService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
  ) {
    this.testId = this.activatedRoute.snapshot.paramMap.get(appRoutes.TestId) ?? '';
    this.resultOverview$ = this.resultOverviewService.getResultOverview$(this.testId);
  }

  openDetails(testOfUserId: string, userId: string) {
    this.router.navigate([appRoutes.App, appRoutes.Test, appRoutes.TestResult, testOfUserId, userId]);
  }

  calculatePercentage(value: number, total: number) {
    if (total === 0) {
      return 0;
    }
    return Math.round(value / total * 100 * 100) / 100;
  }
}