import { DrawCanvasStoragePointDTO } from "./DrawCanvasStoragePointDTO";

export interface DrawCanvasStorageDTO {
    id: string;
    created: Date;
    deleted: Date | null;
    color: string;
    fromPosistion: DrawCanvasStoragePointDTO;
    toPosition: DrawCanvasStoragePointDTO | null;
    text: string | null;
}