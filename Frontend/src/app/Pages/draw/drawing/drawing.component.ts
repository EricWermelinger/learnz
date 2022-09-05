import { Component, ElementRef, OnDestroy, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject, combineLatest, debounceTime, distinctUntilChanged, filter, first, fromEvent, map, merge, Observable, pairwise, Subject, switchMap, takeUntil, tap } from 'rxjs';
import { appRoutes } from 'src/app/Config/appRoutes';
import { DrawPageGetDTO } from 'src/app/DTOs/Draw/DrawPageGetDTO';
import { DrawingService } from './drawing.service';
import { v4 as guid } from 'uuid';
import { DrawPageCreateDTO } from 'src/app/DTOs/Draw/DrawPageCreateDTO';
import { DrawPageEditDTO } from 'src/app/DTOs/Draw/DrawPageEditDTO';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { DrawDeleteConfirmComponent } from '../draw-delete-confirm/draw-delete-confirm.component';
import { FormControl } from '@angular/forms';
import { DrawDrawingDTO } from 'src/app/DTOs/Draw/DrawDrawingDTO';
import { DrawNotifyComponent } from '../draw-notify/draw-notify.component';
import { KeyValue } from '@angular/common';

type DrawMode = 'Draw' | 'Line' | 'Text';

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
  newUserMakingChangesNameDialogRef: MatDialogRef<DrawNotifyComponent> | null = null;
  formControlMode: FormControl;
  modeLinePreviousPoint: Event | null = null;
  formControlColor: FormControl;
  colors: KeyValue<string, string>[];
  modes: KeyValue<DrawMode, string>[];

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

    this.info$.pipe(
      filter(_ => this.formControlEditMode.value),
      map(info => info.newUserMakingChangesName),
      distinctUntilChanged(),
      filter(name => !!name),
      filter(_ => this.newUserMakingChangesNameDialogRef == null),
    ).subscribe(name => {
      this.newUserMakingChangesNameDialogRef = this.dialog.open(DrawNotifyComponent, {
        data: {
          newUserMakingChangesName: name,
          currentUserMakingChangesName: null,
        },
      });
      this.newUserMakingChangesNameDialogRef!.afterClosed().subscribe(_ => {
        this.info$ = this.drawingService.getPages$(this.collectionId).pipe(
          takeUntil(this.destroyed$),
        );
        this.newUserMakingChangesNameDialogRef = null;
        this.router.navigate([appRoutes.App, appRoutes.Draw, this.collectionId, this.pageId$.value]);
      });
    });

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

    const color = getComputedStyle(document.documentElement).getPropertyValue('--learnz-light-black');
    this.formControlColor = new FormControl(color);
    this.colors = [
      { key: 'Standard', value: color },
      { key: 'White', value: 'white' },
      { key: 'Grey', value: 'grey' },
      { key: 'Yellow', value: 'yellow' },
      { key: 'Orange', value: 'orange' },
      { key: 'Red', value: 'red' },
      { key: 'Pink', value: 'pink' },
      { key: 'Purple', value: 'purple' },
      { key: 'Green', value: 'green' },
      { key: 'Blue', value: 'blue' },
      { key: 'Brown', value: 'brown' },
      { key: 'Black', value: 'black' },
    ];

    this.formControlMode = new FormControl('Draw');
    this.modes = [
      { key: 'Draw', value: 'edit' },
      { key: 'Line', value: 'minimize' },
      { key: 'Text', value: 'text_fields' },
    ];
  }

  canvasSetup() {
    const canvas = this.canvas?.nativeElement;
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
      filter(_ => this.formControlMode.value === 'Draw'),
      switchMap(_ => {
        return fromEvent(canvas, 'mousemove').pipe(
          takeUntil(fromEvent(canvas, 'mouseup')),
          takeUntil(fromEvent(canvas, 'mouseleave')),
          pairwise(),
        );
      }),
      map(res => this.canvasMapPoints(canvas.getBoundingClientRect(), res)),
    );
    draw$.subscribe(positions => {
      this.canvasDrawOnMove(positions.prevPos, positions.currentPos);
    });

    const line$ = fromEvent(canvas, 'mousedown').pipe(
      takeUntil(this.destroyed$),
      filter(_ => this.formControlMode.value === 'Line'),
      filter(point => {
        if (this.modeLinePreviousPoint == null) {
          this.modeLinePreviousPoint = point;
          return false;
        }
        return true;
      }),
      map(point => this.canvasMapPoints(canvas.getBoundingClientRect(), [this.modeLinePreviousPoint, point])),
      tap(_ => this.modeLinePreviousPoint = null),
    );
    line$.subscribe(positions => {
      this.canvasDrawOnMove(positions.prevPos, positions.currentPos);
    });

    // todo save with updatepage
  }

  canvasMapPoints(rect: any, res: any) {
    const prevPos = {
      x: (res[0] as any).clientX - rect.left,
      y: (res[0] as any).clientY - rect.top
    };
    const currentPos = {
      x: (res[1] as any).clientX - rect.left,
      y: (res[1] as any).clientY - rect.top
    };
    return {
      prevPos,
      currentPos,
    };
  }

  canvasDrawOnMove(prevPos: { x: number; y: number; }, currentPos: { x: number; y: number; }) {
    if (!this.canvasContext) {
      return;
    }
    this.canvasContext.beginPath();
    this.canvasContext.strokeStyle = this.formControlColor.value;
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

  canvasChangeMode() {
    this.modeLinePreviousPoint = null;
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

  editPage(pageId: string, editingPersonName: string | null) {
    this.pageId$.next(pageId);
    if (!!editingPersonName) {
      const dialog$ = this.dialog.open(DrawNotifyComponent, {
        data: {
          newUserMakingChangesName: null,
          currentUserMakingChangesName: editingPersonName,
        },
      });
      dialog$.afterClosed().subscribe(_ => this.router.navigate([appRoutes.App, appRoutes.Draw, this.collectionId, pageId], { queryParams: { [appRoutes.Edit]: true }}));
    } else {
      this.router.navigate([appRoutes.App, appRoutes.Draw, this.collectionId, pageId], { queryParams: { [appRoutes.Edit]: true }});
    }
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

  changeEditMode(editMode: boolean, editingPersonName: string | null) {
    if (!!editingPersonName) {
      const dalog$ = this.dialog.open(DrawNotifyComponent, {
        data: {
          newUserMakingChangesName: null,
          currentUserMakingChangesName: editingPersonName,
        },
      });
      dalog$.afterClosed().subscribe(_ => this.navigateEditMode(editMode));
    } else {
      this.navigateEditMode(editMode);
    }
  }

  navigateEditMode(editMode: boolean) {
    if (editMode) {
      this.router.navigate([appRoutes.App, appRoutes.Draw, this.collectionId, this.pageId$.value], { queryParams: { [appRoutes.Edit]: true }});
    } else {
      this.router.navigate([appRoutes.App, appRoutes.Draw, this.collectionId, this.pageId$.value]);
    }
    this.pageId$.next(this.pageId$.value);
  }

  isSelectedColor(color: string) {
    return this.formControlColor.value === color;
  }

  isSelectedMode(mode: DrawMode) {
    return this.formControlMode.value === mode;
  }

  selectedModeIcon() {
    const mode = this.modes.find(m => m.key === this.formControlMode.value);
    return mode?.value;
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}