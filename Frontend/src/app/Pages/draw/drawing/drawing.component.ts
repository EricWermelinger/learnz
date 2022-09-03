import { Component, ElementRef, OnDestroy, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject, combineLatest, debounceTime, distinctUntilChanged, filter, first, fromEvent, map, Observable, pairwise, Subject, switchMap, takeUntil, tap } from 'rxjs';
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
  private CANVAS_SIZE = 600;
  collectionId: string;
  pageId$ = new BehaviorSubject<string | undefined>(undefined);
  info$: Observable<DrawDrawingDTO>;
  editMode$: Observable<boolean>;
  formControlEditMode: FormControl;
  @ViewChild('canvas') canvas: ElementRef<HTMLCanvasElement> | undefined;
  canvasContext: CanvasRenderingContext2D | null = null;
  previousPage: string | undefined = undefined;
  previousEditmode: boolean | null = null;

  constructor(
    private drawingService: DrawingService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private dialog: MatDialog,
  ) {
    this.collectionId = this.activatedRoute.snapshot.paramMap.get(appRoutes.DrawCollectionId) ?? '';
    this.pageId$.next(this.activatedRoute.snapshot.paramMap.get(appRoutes.DrawPageId) ?? '');

    this.info$ = this.drawingService.getPages$(this.collectionId).pipe(
      takeUntil(this.destroyed$),
    );
    this.info$.subscribe();
    
    this.editMode$ = this.activatedRoute.queryParamMap.pipe(
      map(params => params.has(appRoutes.Edit))
    );
    this.formControlEditMode = new FormControl(false);
    this.editMode$.subscribe(editMode => this.formControlEditMode.patchValue(editMode));
    
    this.info$.pipe(
      first(),
    ).subscribe(_ => this.canvasSetup());

    combineLatest([
      this.info$,
      this.editMode$,
      this.pageId$.asObservable(),
    ]).pipe(
      tap(_ => {
        if (!this.canvasContext || !this.canvas?.nativeElement) {
          this.canvasSetup();
        }
      }),
      filter(_ => !!this.canvasContext && !!this.canvas?.nativeElement),
      filter(([_, editMode, pageId]) => !editMode || this.previousPage !== pageId || this.previousEditmode !== editMode),
      tap(([_, editMode, pageId]) => {
        this.previousPage = pageId;
        this.previousEditmode = editMode;
      }),
      map(([info, _, pageId]) => info.pages.filter(page => page.pageId === pageId).length === 0 ? undefined : info.pages.filter(page => page.pageId === pageId)[0].dataUrl),
    ).subscribe(dataUrl => this.canvasSetImage(dataUrl ?? ''));
  }

  canvasSetup() {
    const canvas = this.canvas?.nativeElement;
    console.log(canvas)
    if (canvas) {
      this.canvasContext = canvas.getContext('2d');
      canvas.width = this.CANVAS_SIZE;
      canvas.height = this.CANVAS_SIZE;
      const white = getComputedStyle(document.documentElement).getPropertyValue('--learnz-light-white');
      const black = getComputedStyle(document.documentElement).getPropertyValue('--learnz-light-black');
      this.canvasContext!.lineWidth = 1;
      this.canvasContext!.lineCap = 'round';
      this.canvasContext!.fillStyle = white;
      this.canvasContext!.fillRect(0, 0, canvas.width, canvas.height);
      this.canvasContext!.strokeStyle = black;
      this.canvasDraw(canvas);
      this.pageId$.next(this.pageId$.value);
    }
  }

  canvasDraw(canvas: HTMLCanvasElement) {
    const draw$ = fromEvent(canvas, 'mousedown').pipe(
      takeUntil(this.destroyed$),
      switchMap(_ => {
        return fromEvent(canvas, 'mousemove').pipe(
          takeUntil(fromEvent(canvas, 'mouseup')),
          takeUntil(fromEvent(canvas, 'mouseleave')),
          pairwise(),
        );
      }),
    );
    draw$.subscribe(res => {
      const rect = canvas.getBoundingClientRect();
      const prevPos = {
        x: (res[0] as any).clientX - rect.left,
        y: (res[0] as any).clientY - rect.top
      };
      const currentPos = {
        x: (res[1] as any).clientX - rect.left,
        y: (res[1] as any).clientY - rect.top
      };
      this.canvasDrawOnMove(prevPos, currentPos);
    });
    draw$.pipe(
      debounceTime(500),
      map(_ => canvas.toDataURL())
    ).subscribe(data => {
      this.updatePage(data);
    });
  }

  canvasDrawOnMove(prevPos: { x: number; y: number; }, currentPos: { x: number; y: number; }) {
    if (!this.canvasContext) {
      return;
    }
    this.canvasContext.beginPath();
    if (prevPos) {
      this.canvasContext.moveTo(prevPos.x, prevPos.y);
      this.canvasContext.lineTo(currentPos.x, currentPos.y);
      this.canvasContext.stroke();
    }
  }

  canvasSetImage(image: string) {
    const canvas = this.canvas?.nativeElement;
    if (this.canvasContext && canvas) {
      if (!!image) {
        const img = new Image();
        img.onload = () => {
          this.canvasContext!.drawImage(img, 0, 0);
        };
        img.src = image;
      } else {
        this.canvasSetup();
      }
    }
  }

  createPage() {
    const value = {
      collectionId: this.collectionId,
      pageId: guid(),
    } as DrawPageCreateDTO;
    this.drawingService.createPage$(value).subscribe(_ => {
      this.pageId$.next(value.pageId);
      this.router.navigate([appRoutes.App, appRoutes.Draw, this.collectionId, value.pageId], { queryParams: { [appRoutes.Edit]: true }});
    });
  }

  updatePage(dataUrl: string) {
    const value = {
      collectionId: this.collectionId,
      pageId: this.pageId$.value,
      dataUrl,
    } as DrawPageEditDTO;
    this.drawingService.updatePage$(value).subscribe();
  }

  editPage(pageId: string) {
    this.pageId$.next(pageId);
    this.router.navigate([appRoutes.App, appRoutes.Draw, this.collectionId, pageId], { queryParams: { [appRoutes.Edit]: true }});
  }

  deletePage(pageId: string, pages: DrawPageGetDTO[]) {
    const currentPage = pages.filter(p => p.pageId === pageId)[0];
    const currentIndex = pages.indexOf(currentPage);
    const previousId = pageId === this.pageId$.value ? pages[currentIndex === 0 ? 1 : currentIndex - 1].pageId : this.pageId$.value;
    const dialog$ = this.dialog.open(DrawDeleteConfirmComponent, {
      data: {
        collectionId: this.collectionId,
        pageId,
      }
    });
    dialog$.afterClosed().subscribe(_ => {
      this.pageId$.next(previousId);
      this.router.navigate([appRoutes.App, appRoutes.Draw, this.collectionId, previousId]);
    });
  }

  openPage(pageId: string) {
    this.pageId$.next(pageId);
    if (this.formControlEditMode.value) {
      this.router.navigate([appRoutes.App, appRoutes.Draw, this.collectionId, pageId], { queryParams: { [appRoutes.Edit]: true }});
    } else {
      this.router.navigate([appRoutes.App, appRoutes.Draw, this.collectionId, pageId]);
    }
  }

  getActivePage(pages: DrawPageGetDTO[]) {
    const filtered = pages.filter(p => p.pageId === this.pageId$.value);
    if (filtered.length === 0) {
      return undefined;
    }
    return filtered[0];
  }

  changeEditMode(editMode: boolean) {
    if (editMode) {
      this.router.navigate([appRoutes.App, appRoutes.Draw, this.collectionId, this.pageId$.value], { queryParams: { [appRoutes.Edit]: true }});
    } else {
      this.router.navigate([appRoutes.App, appRoutes.Draw, this.collectionId, this.pageId$.value]);
    }
    this.pageId$.next(this.pageId$.value);
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}