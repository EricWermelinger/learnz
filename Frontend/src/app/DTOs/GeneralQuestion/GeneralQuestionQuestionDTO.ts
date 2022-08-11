import { QuestionType } from "src/app/Enums/QuestionType";
import { ChallengeQuestionAnswerDTO } from "../Challenge/ChallengeQuestionAnswerDTO";

export interface GeneralQuestionQuestionDTO {
    question: string;
    description: string | null;
    questionType: QuestionType;
    answerSetOne: ChallengeQuestionAnswerDTO[] | null;
    answerSetTwo: ChallengeQuestionAnswerDTO[] | null;
}