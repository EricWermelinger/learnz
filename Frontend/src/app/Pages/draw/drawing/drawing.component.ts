import { Component, ElementRef, OnDestroy, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject, combineLatest, debounceTime, distinctUntilChanged, filter, first, fromEvent, map, merge, Observable, pairwise, Subject, switchMap, takeUntil, tap, throttleTime } from 'rxjs';
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
import { DrawCanvasType, getDrawCanvasColors, getDrawCanvasTypeWithIcons } from 'src/app/Enums/DrawCanvasType';
import { DrawCanvasStorageDTO } from 'src/app/DTOs/Draw/DrawCanvasStorageDTO';
import { getCanvasStandardColor, getDistanceBetweenSegments } from 'src/app/Framework/Helpers/CanvasHelper';

@Component({
  selector: 'app-drawing',
  templateUrl: './drawing.component.html',
  styleUrls: ['./drawing.component.scss']
})
export class DrawingComponent implements OnDestroy {

  private destroyed$ = new Subject<void>();
  private reloaded$ = new Subject<void>();
  private CANVAS_SIZE = 600;

  collectionId: string;

  pageId$ = new BehaviorSubject<string | undefined>(undefined);
  info$: Observable<DrawDrawingDTO>;
  editMode$: Observable<boolean>;

  formControlEditMode = new FormControl<boolean>(false);
  formControlMode = new FormControl('Draw');
  formControlColor = new FormControl(getCanvasStandardColor());
  
  colors = getDrawCanvasColors();
  modes = getDrawCanvasTypeWithIcons();

  @ViewChild('canvas') canvas: ElementRef<HTMLCanvasElement> | undefined;
  canvasContext: CanvasRenderingContext2D | null = null;
  previousPage: string | undefined = undefined;
  previousEditmode: boolean | null = null;
  modeLinePreviousPoint: Event | null = null;
  modeLineLastEndedPoint: Event | null = null;
  eraseSegmentsBuffer: DrawCanvasStorageDTO[] = [];
  setupDone = false;

  newUserMakingChangesNameDialogRef: MatDialogRef<DrawNotifyComponent> | null = null;

  canvasStorage: DrawCanvasStorageDTO[] = [];

  constructor(
    private drawingService: DrawingService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private dialog: MatDialog,
  ) {
    this.collectionId = this.activatedRoute.snapshot.paramMap.get(appRoutes.DrawCollectionId) ?? '';
    this.pageId$.next(this.activatedRoute.snapshot.paramMap.get(appRoutes.DrawPageId) ?? '');

    this.editMode$ = this.activatedRoute.queryParamMap.pipe(
      map(params => params.has(appRoutes.Edit))
    );
    this.editMode$.subscribe(editMode => this.formControlEditMode.patchValue(editMode));

    this.info$ = this.editMode$.pipe(
      switchMap(editMode => this.drawingService.getPages$(this.collectionId, editMode)),
      takeUntil(this.destroyed$),
    );
    this.info$.subscribe();
    
    this.info$.pipe(
      first(),
      tap(info => this.canvasStorage = info.drawSegmensts ?? []),
    ).subscribe(_ => this.canvasSetup());

    this.info$.pipe(
      filter(_ => !!this.formControlEditMode.value),
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
        this.info$ = this.drawingService.getPages$(this.collectionId, !!this.formControlEditMode.value).pipe(
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
      const notDeleted = this.canvasStorage.filter(storage => !storage.deleted);
      this.canvasDrawMultipleLines(notDeleted);
    }
    if (!this.setupDone) {
      this.pageId$.next(this.pageId$.value);
      this.setupDone = true;
    }
  }

  canvasDraw(canvas: HTMLCanvasElement) {
    this.reloaded$.next();
    this.reloaded$.complete();
    this.reloaded$ = new Subject<void>();

    const mouseDown$ = fromEvent(canvas, 'mousedown').pipe(
      takeUntil(this.reloaded$),
      takeUntil(this.destroyed$)
    );

    const draw$ = mouseDown$.pipe(
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
      this.canvasDrawOnMove(positions);
    });

    const line$ = mouseDown$.pipe(
      filter(_ => this.formControlMode.value === 'Line'),
      filter(point => {
        if (!this.modeLinePreviousPoint) {
          if (!this.modeLineLastEndedPoint) {
            this.modeLinePreviousPoint = point;
          } else {
            const positions = this.canvasMapPoints(canvas.getBoundingClientRect(), [this.modeLineLastEndedPoint, point]);
            if (positions.fromPosistion.x !== positions.toPosition?.x || positions.fromPosistion.y !== positions.toPosition?.y) {
              this.modeLinePreviousPoint = point;
            }
          }
          return false;
        }
        return true;
      }),
      map(point => { return { positions: this.canvasMapPoints(canvas.getBoundingClientRect(), [this.modeLinePreviousPoint, point]), point }}),
      filter(pos => pos.positions.fromPosistion.x !== pos.positions.toPosition!.x || pos.positions.fromPosistion.y !== pos.positions.toPosition!.y),
    );
    line$.subscribe(pos => {
      this.modeLinePreviousPoint = null;
      this.modeLineLastEndedPoint = pos.point;
      this.canvasDrawOnMove(pos.positions);
    });

    const erase$ = mouseDown$.pipe(
      filter(_ => this.formControlMode.value === 'Erase'),
      switchMap(_ => {
        return fromEvent(canvas, 'mousemove').pipe(
          takeUntil(fromEvent(canvas, 'mouseup')),
          takeUntil(fromEvent(canvas, 'mouseleave')),
          pairwise(),
        );
      }),
      map(res => this.canvasMapPoints(canvas.getBoundingClientRect(), res)),
      tap(points => {
        this.eraseSegmentsBuffer.push(points);
      }),
      distinctUntilChanged(),
      debounceTime(100),
    );
    erase$.subscribe(_ => {
      this.canvasEraseOnMove(this.eraseSegmentsBuffer);
      this.eraseSegmentsBuffer = [];
    });

    merge(
      draw$,
      line$,
      erase$,
    ).pipe(
      debounceTime(500),
    ).subscribe(_ => {
      this.updatePage(canvas.toDataURL());
    });
  }

  canvasMapPoints(rect: any, res: any) {
    const fromPosistion = {
      x: (res[0] as any).clientX - rect.left,
      y: (res[0] as any).clientY - rect.top
    };
    const toPosition = {
      x: (res[1] as any).clientX - rect.left,
      y: (res[1] as any).clientY - rect.top
    };
    return {
      id: guid(),
      color: this.formControlColor.value,
      created: new Date(),
      deleted: null,
      fromPosistion: {
        id: guid(),
        x: fromPosistion.x,
        y: fromPosistion.y,
      },
      toPosition: {
        id: guid(),
        x: toPosition!.x,
        y: toPosition!.y,
      },
      text: null,
    } as DrawCanvasStorageDTO;
  }

  canvasDrawOnMove(storage: DrawCanvasStorageDTO) {
    if (!this.canvasContext) {
      return;
    }
    this.canvasContext.beginPath();
    this.canvasContext.strokeStyle = this.formControlColor.value ?? getCanvasStandardColor();
    if (storage.fromPosistion && storage.toPosition) {
      this.canvasContext.moveTo(storage.fromPosistion.x, storage.fromPosistion.y);
      this.canvasContext.lineTo(storage.toPosition.x, storage.toPosition.y);
      this.canvasContext.stroke();
      this.canvasStorage.push(storage);
    }
  }

  canvasDrawMultipleLines(storage: DrawCanvasStorageDTO[]) {
    if (!this.canvasContext) {
      return;
    }
    this.canvasContext.beginPath();
    this.canvasContext.strokeStyle = this.formControlColor.value ?? getCanvasStandardColor();
    for (let i = 0; i < storage.length; i++) {
      const item = storage[i];
      if (item.fromPosistion && item.toPosition) {
        this.canvasContext.moveTo(item.fromPosistion.x, item.fromPosistion.y);
        this.canvasContext.lineTo(item.toPosition.x, item.toPosition.y);
        this.canvasContext.stroke();
      }
    }
  }

  canvasEraseOnMove(eraseAll: DrawCanvasStorageDTO[]) {
    if (!this.canvasContext) {
      return;
    }

    const timeStamp = new Date();
    for (let i = 0; i < eraseAll.length; i++) {
      const erase = eraseAll[i];
      const erased = this.canvasStorage
        .filter(s => s.deleted === null)
        .filter(s => {
          const path = { from: s.fromPosistion, to: s.toPosition };
          return getDistanceBetweenSegments(path.from.x, path.from.y, path.to!.x, path.to!.y, erase.fromPosistion.x, erase.fromPosistion.y, erase.toPosition!.x, erase.toPosition!.y) < 10;
      });
      for (let j = 0; j < erased.length; j++) {
        const storage = this.canvasStorage.find(s => s.id === erased[j].id);
        if (storage) {
          storage.deleted = timeStamp;
        }
      }
    }
    this.canvasSetup();
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

  isSelectedMode(mode: DrawCanvasType) {
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