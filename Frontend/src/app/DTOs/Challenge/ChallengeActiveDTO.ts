import { ChallengePlayerResultDTO } from "./ChallengePlayerResultDTO";
import { ChallengeQuestionDTO } from "./ChallengeQuestionDTO";

export interface ChallengeActiveDTO {
    name: string;
    result: ChallengePlayerResultDTO[];
    cancelled: boolean;
    isOwner: boolean;
    question: ChallengeQuestionDTO | null;
    lastQuestionPoint: number | null;
    state: number;
}