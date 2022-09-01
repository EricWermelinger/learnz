import { Component, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { DrawService } from './draw.service';

@Component({
  selector: 'app-draw',
  templateUrl: './draw.component.html',
  styleUrls: ['./draw.component.scss']
})
export class DrawComponent implements OnDestroy {

  private destroyed$ = new Subject<void>();

  constructor(
    private drawService: DrawService,
  ) { }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}