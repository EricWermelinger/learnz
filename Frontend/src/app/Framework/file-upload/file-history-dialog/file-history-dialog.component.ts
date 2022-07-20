import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { FileVersionInfoDTO } from 'src/app/DTOs/File/FileVersionInfoDTO';
import { FileHistoryDialogService } from './file-history-dialog.service';

@Component({
  selector: 'app-file-history-dialog',
  templateUrl: './file-history-dialog.component.html',
  styleUrls: ['./file-history-dialog.component.scss']
})
export class FileHistoryDialogComponent {

  versions$: Observable<FileVersionInfoDTO[]>;
  path: string;

  constructor(
    private fileHistoryService: FileHistoryDialogService,
    @Inject(MAT_DIALOG_DATA) public data: string,
  ) {
    this.path = this.data;
    this.versions$ = this.fileHistoryService.getVersions(this.path);
  }

  revertVersion(versionPath: string) {
    this.fileHistoryService.revertVersion(versionPath, this.path).subscribe(_ => {
      this.versions$ = this.fileHistoryService.getVersions(this.path);
    });
  }

  downloadVersion(filePath: string) {
    this.fileHistoryService.downloadVersion(filePath);
  }
}