import { HttpEventType } from '@angular/common/http';
import { Component, EventEmitter, Output } from '@angular/core';
import { endpoints } from 'src/app/Config/endpoints';
import { ApiService } from '../API/api.service';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.scss']
})
export class FileUploadComponent {

  progress: number = 0;
  message: string = '';
  @Output() public onUploadFinished = new EventEmitter();
  
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
    this.api.callFileUpload(endpoints.FileUploadAnonymous, formData).subscribe((event: any) => {
      if (event.type === HttpEventType.UploadProgress) {
        this.progress = Math.round(event.loaded * 100 / event.total);
      } else if (event.type === HttpEventType.Response) {
        this.message = 'fileUpload.uploadSuccessful';
        this.onUploadFinished.emit(event.body);
      }
    });
  }
}
