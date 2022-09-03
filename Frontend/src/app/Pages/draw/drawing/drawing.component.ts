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
import { FormControl } from '@angular/forms';
import { DrawDrawingDTO } from 'src/app/DTOs/Draw/DrawDrawingDTO';

@Component({
  selector: 'app-drawing',
  templateUrl: './drawing.component.html',
  styleUrls: ['./drawing.component.scss']
})
export class DrawingComponent implements OnDestroy {

  private destroyed$ = new Subject<void>();
  collectionId: string;
  pageId: string;
  info$: Observable<DrawDrawingDTO>;
  editMode$: Observable<boolean>;
  formControlEditMode: FormControl;

  constructor(
    private drawingService: DrawingService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private dialog: MatDialog,
  ) {
    this.collectionId = this.activatedRoute.snapshot.paramMap.get(appRoutes.DrawCollectionId) ?? '';
    this.pageId = this.activatedRoute.snapshot.paramMap.get(appRoutes.DrawPageId) ?? '';
    this.info$ = this.drawingService.getPages$(this.collectionId).pipe(
      takeUntil(this.destroyed$),
    );
    this.info$.subscribe();
    this.editMode$ = this.activatedRoute.queryParamMap.pipe(
      map(params => params.has(appRoutes.Edit))
    );
    this.formControlEditMode = new FormControl(false);
    this.editMode$.subscribe(editMode => this.formControlEditMode.patchValue(editMode));
  }

  createPage() {
    const value = {
      collectionId: this.collectionId,
      pageId: guid(),
    } as DrawPageCreateDTO;
    this.drawingService.createPage$(value).subscribe(_ => {
      this.pageId = value.pageId;
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
    this.pageId = pageId;
    this.router.navigate([appRoutes.App, appRoutes.Draw, this.collectionId, pageId], { queryParams: { [appRoutes.Edit]: true }});
  }

  deletePage(pageId: string, pages: DrawPageGetDTO[]) {
    const currentPage = pages.filter(p => p.pageId === pageId)[0];
    const currentIndex = pages.indexOf(currentPage);
    const previousId = pageId === this.pageId ? pages[currentIndex === 0 ? 1 : currentIndex - 1].pageId : this.pageId;
    const dialog$ = this.dialog.open(DrawDeleteConfirmComponent, {
      data: {
        collectionId: this.collectionId,
        pageId,
      }
    });
    dialog$.afterClosed().subscribe(_ => {
      this.pageId = previousId;
      this.router.navigate([appRoutes.App, appRoutes.Draw, this.collectionId, previousId]);
    });
  }

  openPage(pageId: string) {
    this.pageId = pageId;
    if (this.formControlEditMode.value) {
      this.router.navigate([appRoutes.App, appRoutes.Draw, this.collectionId, pageId], { queryParams: { [appRoutes.Edit]: true }});
    } else {
      this.router.navigate([appRoutes.App, appRoutes.Draw, this.collectionId, pageId]);
    }
  }

  getCurrentDataUrl() {
    return '';
  }

  getActivePage(pages: DrawPageGetDTO[]) {
    const filtered = pages.filter(p => p.pageId === this.pageId);
    if (filtered.length === 0) {
      return undefined;
    }
    return filtered[0];
  }

  changeEditMode(editMode: boolean) {
    if (editMode) {
      this.router.navigate([appRoutes.App, appRoutes.Draw, this.collectionId, this.pageId], { queryParams: { [appRoutes.Edit]: true }});
    } else {
      this.router.navigate([appRoutes.App, appRoutes.Draw, this.collectionId, this.pageId]);
    }
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}