import { Injectable } from '@angular/core';
import { endpoints } from 'src/app/Config/endpoints';
import { FileRevertDTO } from 'src/app/DTOs/File/FileRevertDTO';
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
    const value = {
      versionPath,
      filePath,
    } as FileRevertDTO;
    return this.api.callApi(endpoints.FileVersions, value, 'POST');
  }

  downloadVersion(filePath: string) {
    return this.api.callFileDownload(endpoints.FileVersionUploadDownload, { filePath });
  }
}
