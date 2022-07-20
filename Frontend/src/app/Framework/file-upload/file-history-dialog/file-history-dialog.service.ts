import { Injectable } from '@angular/core';
import { endpoints } from 'src/app/Config/endpoints';
import { FileVersionInfoDTO } from 'src/app/DTOs/File/FileVersionInfoDTO';
import { ApiService } from '../../API/api.service';

@Injectable({
  providedIn: 'root'
})
export class FileHistoryDialogService {

  constructor(
    private api: ApiService,
  ) { }

  getVersions(path: string) {
    return this.api.callApi<FileVersionInfoDTO[]>(endpoints.FileVersions, { path }, 'GET');
  }

  revertVersion(versionPath: string, filePath: string) {
    return this.api.callApi(endpoints.FileVersions, { versionPath, filePath }, 'POST');
  }

  downloadVersion(filePath: string) {
    this.api.callApi(endpoints.FileVersionUploadDownload, { filePath }, 'GET').subscribe();
  }
}
