export interface DrawCollectionGetDTO {
    collectionId: string;
    name: string;
    numberOfPages: number;
    firstPageId: string;
    editable: boolean;
    deletable: boolean;
    isGroupCollection: boolean;
    groupName: string | null;
    lastChanged: Date;
    lastChangedBy: string;
}