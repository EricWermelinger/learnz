import { HttpEventType } from '@angular/common/http';
import { Component, Input } from '@angular/core';
import { AbstractControl, ControlValueAccessor, NG_VALIDATORS, NG_VALUE_ACCESSOR, ValidationErrors, Validator } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { endpoints } from 'src/app/Config/endpoints';
import { FileChangePolicyDTO } from 'src/app/DTOs/File/FileChangePolicyDTO';
import { FileFrontendHistorizedDTO } from 'src/app/DTOs/File/FileFrontendHistorizedDTO';
import { FilePathDTO } from 'src/app/DTOs/File/FilePathDTO';
import { FilePolicy, getPolicies } from 'src/app/Enums/FilePolicy';
import { ApiService } from '../API/api.service';
import { FileHistoryDialogComponent } from './file-history-dialog/file-history-dialog.component';

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

  _filePath: string = '';
  _externalFilename: string = '';
  _historizedFile: FileFrontendHistorizedDTO | null = null;
  policies = getPolicies();
  @Input() set filePath(filePath: string) {
    this._filePath = filePath;
  }
  @Input() set externalFilename(externalFilename: string) {
    this._externalFilename = externalFilename;
  }
  @Input() isAnonymous: boolean = false;
  @Input() fileTypes: string = '';
  @Input() translationKey: string = '';
  @Input() set historizedFile (file: FileFrontendHistorizedDTO) {
    this._historizedFile = file;
    this._filePath = this.historizedFile.path;
    this._externalFilename = this.historizedFile.externalFilename;
  }
  
  constructor(
    private api: ApiService,
    private matDialog: MatDialog,
  ) { }

  uploadFile(files: any, isNewVersion: boolean = false) {
    if (files.length === 0) {
      return;
    }
    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    if (isNewVersion) {
      formData.append('path', this._filePath);
    }
    this.api.callFileUpload(this.getEndpoint(isNewVersion), formData).subscribe((event: any) => {
      if (event.type === HttpEventType.Response) {
        const body = (event.body as FilePathDTO);
        this.updateValue(body.path);
        this._externalFilename = body.externalFilename;
      }
    });
  }

  removeFile() {
    const path = this._filePath;
    this.updateValue('');
    this._externalFilename = '';
    this.api.callApi(this.getEndpoint(), {
      filePath: path
    }, 'DELETE').subscribe();
  }

  downloadFile() {
    this.api.callFileDownload(this.getEndpoint(), { filePath: this._filePath }).subscribe((event: any) => {
      if (event.type === HttpEventType.Response) {
        this.download(event);
      }
    });
  }

  openHistory() {
    this.matDialog.open(FileHistoryDialogComponent, {
      data: {
        path: this._filePath,
        revertable: this._historizedFile!.editable
      }
    });
  }

  setPolicy(policyNumber: number) {
    const value = {
      filePath: this._filePath,
      policy: policyNumber,
    } as FileChangePolicyDTO;
    this.api.callApi(endpoints.FileChangePolicy, value, 'POST').subscribe();
  }
  
  private getEndpoint(isNewVersion: boolean = false): string {
    if (isNewVersion) {
      return endpoints.FileVersionUploadDownload;
    }
    return this.isAnonymous ? endpoints.FileUploadDownloadAnonymous : endpoints.FileUploadDownload;
  }

  private download(data: any) {
    const downloadedFile = new Blob([data.body], { type: data.body.type });
    const a = document.createElement('a');
    a.setAttribute('style', 'display:none;');
    document.body.appendChild(a);
    a.download = this._externalFilename;
    a.href = URL.createObjectURL(downloadedFile);
    a.target = '_blank';
    a.click();
    document.body.removeChild(a);
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
