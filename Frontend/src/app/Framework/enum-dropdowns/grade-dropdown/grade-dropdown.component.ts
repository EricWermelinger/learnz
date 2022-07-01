import { Component, Input } from '@angular/core';
import { getGrades } from 'src/app/Enums/Grade';

@Component({
  selector: 'app-grade-dropdown',
  templateUrl: './grade-dropdown.component.html',
  styleUrls: ['./grade-dropdown.component.scss']
})
export class GradeDropdownComponent {

  grades = getGrades();
  @Input() formControlName = '';
  @Input() value = -1;
  
  constructor() { }

}
