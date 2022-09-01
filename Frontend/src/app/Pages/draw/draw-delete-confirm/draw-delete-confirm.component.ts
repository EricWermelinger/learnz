import { Component } from '@angular/core';
import { DrawDeleteConfirmService } from './draw-delete-confirm.service';

@Component({
  selector: 'app-draw-delete-confirm',
  templateUrl: './draw-delete-confirm.component.html',
  styleUrls: ['./draw-delete-confirm.component.scss']
})
export class DrawDeleteConfirmComponent {

  constructor(
    private deleteConfirmService: DrawDeleteConfirmService,
  ) { }

}