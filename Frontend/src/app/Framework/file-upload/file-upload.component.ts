import { HttpEventType, HttpResponse } from '@angular/common/http';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { endpoints } from 'src/app/Config/endpoints';
import { FilePathDTO } from 'src/app/DTOs/File/FilePathDTO';
import { ApiService } from '../API/api.service';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.scss']
})
export class FileUploadComponent {

  progress: number = 0;
  message: string = '';
  _filePath: string = '';
  _externalFileName: string = '';
  @Output() public onUploadFinished = new EventEmitter();
  @Input() set filePath(filePath: string) {
    this._filePath = filePath;
  }
  @Input() set externalFileName(externalFileName: string) {
    this._externalFileName = externalFileName;
  }
  @Input() isAnonymous: boolean = false;
  
  constructor(
    private api: ApiService,
  ) { }

  uploadFile(files: any) {
    if (files.length === 0) {
      return;
    }

    this.message = 'fileUpload.uploading';
    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    const endpoint = this.isAnonymous ? endpoints.FileUploadDownloadAnonymous : endpoints.FileUploadDownload;
    this.api.callFileUpload(endpoint, formData).subscribe((event: any) => {
      if (event.type === HttpEventType.UploadProgress) {
        this.progress = this.sanitizePercent(event.loaded, event.total);
      } else if (event.type === HttpEventType.Response) {
        this.message = 'fileUpload.uploadSuccessful';
        const body = (event.body as FilePathDTO);
        this._filePath = body.path;
        this._externalFileName = body.externalFileName;
        this.progress = 0;
        this.onUploadFinished.emit(this._filePath);
      }
    });
  }

  removeFile() {
    this._filePath = '';
    this._externalFileName = '';
    this.message = '';
    this.progress = 0;
  }

  downloadFile() {
    this.message = 'fileUpload.downloading';
    const endpoint = this.isAnonymous ? endpoints.FileUploadDownloadAnonymous : endpoints.FileUploadDownload;
    this.api.callFileDownload(endpoint, { filePath: this._filePath }).subscribe((event: any) => {
      if (event.type === HttpEventType.DownloadProgress) {
        this.progress = this.sanitizePercent(event.loaded, event.total);
      } else if (event.type === HttpEventType.Response) {
        this.message = 'fileUpload.downloadSuccessful';
        this.progress = 0;
        this.download(event);
      }
    });
  }

  private download(data: any) {
    const downloadedFile = new Blob([data.body], { type: data.body.type });
    const a = document.createElement('a');
    a.setAttribute('style', 'display:none;');
    document.body.appendChild(a);
    a.download = this._filePath;
    a.href = URL.createObjectURL(downloadedFile);
    a.target = '_blank';
    a.click();
    document.body.removeChild(a);
  }

  private sanitizePercent(loaded: number, total: number): number {
    const percent = Math.round(loaded * 100 / total);
    return percent === 100 ? 99 : percent;
  }
}
