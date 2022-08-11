import { Subject } from "src/app/Enums/Subject";

export interface ChallengeOpenDTO {
    challengeId: string;
    name: string;
    createSetName: string;
    subjectMain: Subject;
    subjectSecond: Subject | null;
    numberOfPlayers: number;
    isOwner: boolean;
}