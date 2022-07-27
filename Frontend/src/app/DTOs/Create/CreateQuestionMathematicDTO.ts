import { CreateQuestionMathematicVariableDTO } from "./createQuestionMathematicVariableDTO";

export interface CreateQuestionMathematicDTO {
    id: string;
    question: string;
    answer: string;
    digits: number;
    variables: CreateQuestionMathematicVariableDTO[];
}