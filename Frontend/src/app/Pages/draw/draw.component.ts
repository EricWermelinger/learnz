import { Component, OnDestroy } from '@angular/core';
import { Observable, Subject, takeUntil } from 'rxjs';
import { DrawCollectionGetDTO } from 'src/app/DTOs/Draw/DrawCollectionGetDTO';
import { DrawService } from './draw.service';

@Component({
  selector: 'app-draw',
  templateUrl: './draw.component.html',
  styleUrls: ['./draw.component.scss']
})
export class DrawComponent implements OnDestroy {

  private destroyed$ = new Subject<void>();
  collections$: Observable<DrawCollectionGetDTO[]>;

  constructor(
    private drawService: DrawService,
  ) {
    this.collections$ = this.drawService.getCollections$().pipe(
      takeUntil(this.destroyed$),
    );
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}