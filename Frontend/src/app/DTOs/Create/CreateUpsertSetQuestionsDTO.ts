import { CreateQuestionDistributeDTO } from "./createQuestionDistributeDTO";
import { CreateQuestionMathematicDTO } from "./createQuestionMathematicDTO";
import { CreateQuestionMultipleChoiceDTO } from "./createQuestionMultipleChoiceDTO";
import { CreateQuestionOpenQuestionDTO } from "./createQuestionOpenQuestionDTO";
import { CreateQuestionTextFieldDTO } from "./createQuestionTextFieldDTO";
import { CreateQuestionTrueFalseDTO } from "./createQuestionTrueFalseDTO";
import { CreateQuestionWordDTO } from "./createQuestionWordDTO";

export interface CreateUpsertSetQuestionsDTO {
    setId: string;
    questionsDistribute: CreateQuestionDistributeDTO[];
    questionsMathematic: CreateQuestionMathematicDTO[];
    questionsMultipleChoice: CreateQuestionMultipleChoiceDTO[];
    questionsOpenQuestion: CreateQuestionOpenQuestionDTO[];
    questionsTextField: CreateQuestionTextFieldDTO[];
    questionsTrueFalse: CreateQuestionTrueFalseDTO[];
    questionsWord: CreateQuestionWordDTO[];
}