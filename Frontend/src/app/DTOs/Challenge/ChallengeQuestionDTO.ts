import { QuestionType } from "src/app/Enums/QuestionType";
import { ChallengeQuestionAnswerDTO } from "./ChallengeQuestionAnswerDTO";

export interface ChallengeQuestionDTO {
    question: string;
    description: string | null;
    questionType: QuestionType;
    answerSetOne: ChallengeQuestionAnswerDTO[] | null;
    answerSetTwo: ChallengeQuestionAnswerDTO[] | null;
}