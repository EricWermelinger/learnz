import { HttpEventType } from '@angular/common/http';
import { Component, Input } from '@angular/core';
import { AbstractControl, ControlValueAccessor, NG_VALIDATORS, NG_VALUE_ACCESSOR, ValidationErrors, Validator } from '@angular/forms';
import { endpoints } from 'src/app/Config/endpoints';
import { FilePathDTO } from 'src/app/DTOs/File/FilePathDTO';
import { ApiService } from '../API/api.service';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi:true,
      useExisting: FileUploadComponent
    },
    {
      provide: NG_VALIDATORS,
      multi: true,
      useExisting: FileUploadComponent
    }
  ],
})
export class FileUploadComponent implements ControlValueAccessor, Validator {

  progress: number = 0;
  message: string = '';
  _filePath: string = '';
  _externalFileName: string = '';
  @Input() set filePath(filePath: string) {
    this._filePath = filePath;
  }
  @Input() set externalFileName(externalFileName: string) {
    this._externalFileName = externalFileName;
  }
  @Input() isAnonymous: boolean = false;
  @Input() fileTypes: string = '';
  @Input() translationKey: string = '';
  @Input() breakLine: boolean = false;
  
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
        this.updateValue(body.path);
        this._externalFileName = body.externalFileName;
        this.progress = 0;
      }
    });
  }

  removeFile() {
    this.updateValue('');
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
    a.download = this._externalFileName;
    a.href = URL.createObjectURL(downloadedFile);
    a.target = '_blank';
    a.click();
    document.body.removeChild(a);
  }

  private sanitizePercent(loaded: number, total: number): number {
    const percent = Math.round(loaded * 100 / total);
    return percent === 100 ? 99 : percent;
  }

  touched = false;
  disabled = false;
  onChange = (value: any) => {};
  onTouched = () => {};
  writeValue(value: string): void {
    this._filePath = value;
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
  updateValue(value: string) {
    this.markAsTouched();
    if (!this.disabled) {
      this._filePath = value;
      this.onChange(value);
    }
  }
  markAsTouched() {
    if (!this.touched) {
      this.onTouched();
      this.touched = true;
    }
  }
  setDisabledState(disabled: boolean) {
    this.disabled = disabled;
  }

  validate(control: AbstractControl<any, any>): ValidationErrors | null {
    return !!this._filePath ? null : { required: true };
  }
}
