export interface FileInfoDTO {
    fileNameExternal: string;
    filePath: string;
    created: Date;
    createdUsername: string;
    modified: Date;
    modifiedUsername: string;
    fileFromMe: boolean;
    fileEditable: boolean;
    fileDeletable: boolean;
}