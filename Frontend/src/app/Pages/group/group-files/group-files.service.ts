import { Injectable } from '@angular/core';
import { merge, Observable } from 'rxjs';
import { endpoints } from 'src/app/Config/endpoints';
import { FileFrontendDTO } from 'src/app/DTOs/File/FileFrontendDTO';
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

  getFiles(groupId: string): Observable<FileFrontendDTO[]> {
    return merge(
      this.api.callApi<FileFrontendDTO[]>(endpoints.GroupFiles, { groupId }, 'GET'),
      this.ws.webSocketData<FileFrontendDTO[]>(endpoints.GroupFiles, [] as FileFrontendDTO[], groupId),
    );
  }

  editFiles(files: GroupFilesEditDTO) {
    this.api.callApi(endpoints.GroupFiles, files, 'POST').subscribe();
  }
}