import { Component, Input } from '@angular/core';
import { getSubjects } from 'src/app/Enums/Subject';

@Component({
  selector: 'app-subject-dropdown',
  templateUrl: './subject-dropdown.component.html',
  styleUrls: ['./subject-dropdown.component.scss']
})
export class SubjectDropdownComponent {

  subjects = getSubjects();
  @Input() formControlName = '';
  @Input() value = null;
  
  constructor() { }

}
