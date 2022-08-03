import { SetPolicy } from "src/app/Enums/SetPolicy";
import { Subject } from "src/app/Enums/Subject";

export interface CreateUpsertSetHeaderDTO {
    id: string;
    name: string;
    description: string;
    subjectMain: Subject;
    subjectSecond: Subject | null;
    setPolicy: SetPolicy;
    isEditable: boolean;
}