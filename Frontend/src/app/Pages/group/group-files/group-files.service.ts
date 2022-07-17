import { Injectable } from '@angular/core';
import { merge, Observable } from 'rxjs';
import { endpoints } from 'src/app/Config/endpoints';
import { FileInfoDTO } from 'src/app/DTOs/File/FileInfoDTO';
import { GroupFilesEditDTO } from 'src/app/DTOs/Group/GroupFilesEditDTO';
import { ApiService } from 'src/app/Framework/API/api.service';
import { WebSocketService } from 'src/app/Framework/API/web-socket.service';

@Injectable({
  providedIn: 'root'
})
export class GroupFilesService {

  constructor(
    private api: ApiService,
    private ws: WebSocketService,
  ) { }

  getFiles(groupId: string): Observable<FileInfoDTO[]> {
    return merge(
      this.api.callApi<FileInfoDTO[]>(endpoints.GroupFiles, { groupId }, 'GET'),
      this.ws.webSocketData<FileInfoDTO[]>(endpoints.GroupFiles, [] as FileInfoDTO[], groupId),
    );
  }

  editFiles(files: GroupFilesEditDTO) {
    this.api.callApi(endpoints.GroupFiles, files, 'POST').subscribe();
  }
}