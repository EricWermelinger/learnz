import { GeneralQuestionQuestionDTO } from "../GeneralQuestion/GeneralQuestionQuestionDTO";

export interface LearnQuestionDTO {
    question: GeneralQuestionQuestionDTO;
    answered: boolean;
    answeredCorrect: boolean | null;
    markedAsHard: boolean;
    solution: string | null;
}