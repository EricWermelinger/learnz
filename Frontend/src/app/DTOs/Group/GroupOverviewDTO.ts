﻿export interface GroupOverviewDTO {
    groupId: string;
    groupName: string;
    profileImagePath: string;
    lastMessageSentByMe: boolean | null;
    lastMessage: string | null;
    lastMessageDateSent: Date | null;
    lastMessageWasInfoMessage: boolean | null;
    numberOfFiles: number;
}