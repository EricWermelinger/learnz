import { Subject } from "src/app/Enums/Subject";

export interface CreateSetOverviewDTO {
  setId: string;
  name: string;
  description: string;
  subjectMain: Subject;
  subjectSecond: Subject | null;
  numberOfQuestions: number;
  owner: string;
  usable: boolean;
  editable: boolean;
}