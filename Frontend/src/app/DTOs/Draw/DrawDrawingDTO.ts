import { DrawCanvasStorageDTO } from "./DrawCanvasStorageDTO";
import { DrawPageGetDTO } from "./DrawPageGetDTO";

export interface DrawDrawingDTO {
    pages: DrawPageGetDTO[];
    drawSegmensts: DrawCanvasStorageDTO[] | null;
    name: string;
    editable: boolean;
    newUserMakingChangesName: string | null;
}