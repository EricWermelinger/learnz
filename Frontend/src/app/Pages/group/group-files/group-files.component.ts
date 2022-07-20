import { Component, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable, Subject, takeUntil } from 'rxjs';
import { appRoutes } from 'src/app/Config/appRoutes';
import { FileFrontendHistorizedDTO } from 'src/app/DTOs/File/FileFrontendHistorizedDTO';
import { GroupFilesService } from './group-files.service';

@Component({
  selector: 'app-group-files',
  templateUrl: './group-files.component.html',
  styleUrls: ['./group-files.component.scss']
})
export class GroupFilesComponent implements OnDestroy {

  groupId: string;
  files$: Observable<FileFrontendHistorizedDTO[]>;
  private destroyed$ = new Subject<void>();

  constructor(
    private filesService: GroupFilesService,
    private activatedRoute: ActivatedRoute,
  ) {
    this.groupId = this.activatedRoute.snapshot.paramMap.get(appRoutes.GroupFilesId) ?? '';
    this.files$ = this.filesService.getFiles(this.groupId).pipe(takeUntil(this.destroyed$));
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}