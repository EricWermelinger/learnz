import { Component, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { map, Observable, Subject, takeUntil } from 'rxjs';
import { appRoutes } from 'src/app/Config/appRoutes';
import { DrawPageGetDTO } from 'src/app/DTOs/Draw/DrawPageGetDTO';
import { DrawingService } from './drawing.service';
import { v4 as guid } from 'uuid';
import { DrawPageCreateDTO } from 'src/app/DTOs/Draw/DrawPageCreateDTO';
import { DrawPageEditDTO } from 'src/app/DTOs/Draw/DrawPageEditDTO';
import { MatDialog } from '@angular/material/dialog';
import { DrawDeleteConfirmComponent } from '../draw-delete-confirm/draw-delete-confirm.component';

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
  activePage$: Observable<DrawPageGetDTO | undefined>;

  constructor(
    private drawingService: DrawingService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private dialog: MatDialog,
  ) {
    this.collectionId = this.activatedRoute.snapshot.paramMap.get(appRoutes.DrawCollectionId) ?? '';
    this.pageId = this.activatedRoute.snapshot.paramMap.get(appRoutes.DrawPageId) ?? '';
    this.pages$ = this.drawingService.getPages$(this.collectionId).pipe(
      takeUntil(this.destroyed$),
    );
    this.pages$.subscribe();
    this.activePage$ = this.pages$.pipe(
      map(pages => {
        const filtered = pages.filter(p => p.pageId === this.pageId);
        if (filtered.length === 0) {
          return undefined;
        }
        return filtered[0];
      })
    )
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

  editPage(pageId: string) {
    this.router.navigate([appRoutes.App, appRoutes.Draw, this.collectionId, pageId], { queryParams: { [appRoutes.Edit]: true }});
  }

  deletePage(pageId: string, pages: DrawPageGetDTO[]) {
    const currentPage = pages.filter(p => p.pageId === pageId)[0];
    const currentIndex = pages.indexOf(currentPage);
    const previousId = pages[currentIndex === 0 ? 1 : currentIndex - 1].pageId;
    const dialog$ = this.dialog.open(DrawDeleteConfirmComponent, {
      data: {
        collectionId: this.collectionId,
        pageId,
      }
    });
    dialog$.afterClosed().subscribe(_ => {
      this.router.navigate([appRoutes.App, appRoutes.Draw, this.collectionId, previousId]);
    });
  }

  openPage(pageId: string) {
    this.router.navigate([appRoutes.App, appRoutes.Draw, this.collectionId, pageId], { queryParams: { [appRoutes.Edit]: true }});
  }

  getCurrentDataUrl() {
    return '';
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}