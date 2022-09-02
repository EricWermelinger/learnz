import { Component, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subject, takeUntil } from 'rxjs';
import { appRoutes } from 'src/app/Config/appRoutes';
import { DrawPageGetDTO } from 'src/app/DTOs/Draw/DrawPageGetDTO';
import { DrawingService } from './drawing.service';
import { v4 as guid } from 'uuid';
import { DrawPageCreateDTO } from 'src/app/DTOs/Draw/DrawPageCreateDTO';
import { DrawPageEditDTO } from 'src/app/DTOs/Draw/DrawPageEditDTO';

@Component({
  selector: 'app-drawing',
  templateUrl: './drawing.component.html',
  styleUrls: ['./drawing.component.scss']
})
export class DrawingComponent implements OnDestroy {

  private destroyed$ = new Subject<void>();
  collectionId: string;
  pageId: string;
  pages$: Observable<DrawPageGetDTO[]>;

  constructor(
    private drawingService: DrawingService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
  ) {
    this.collectionId = this.activatedRoute.snapshot.paramMap.get(appRoutes.DrawCollectionId) ?? '';
    this.pageId = this.activatedRoute.snapshot.paramMap.get(appRoutes.DrawPageId) ?? '';
    this.pages$ = this.drawingService.getPages$(this.collectionId).pipe(
      takeUntil(this.destroyed$),
    );
  }

  createPage() {
    const value = {
      collectionId: this.collectionId,
      pageId: guid(),
    } as DrawPageCreateDTO;
    this.drawingService.createPage$(value).subscribe(_ => {
      this.router.navigate([appRoutes.App, appRoutes.Draw, this.collectionId, value.pageId]);
    });
  }

  updatePage() {
    const value = {
      collectionId: this.collectionId,
      pageId: this.pageId,
      dataUrl: this.getCurrentDataUrl(),
    } as DrawPageEditDTO;
    this.drawingService.updatePage$(value).subscribe();
  }

  getCurrentDataUrl() {
    return '';
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}