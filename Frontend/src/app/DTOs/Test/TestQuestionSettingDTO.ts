import { GeneralQuestionQuestionDTO } from "../GeneralQuestion/GeneralQuestionQuestionDTO";

export interface TestQuestionSettingDTO {
  question: GeneralQuestionQuestionDTO;
  pointsPossible: number;
  visible: boolean;
}