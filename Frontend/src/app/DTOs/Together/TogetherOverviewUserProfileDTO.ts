import { Grade } from "src/app/Enums/Grade";
import { Subject } from "src/app/Enums/Subject";

export interface TogetherOverviewUserProfileDTO {
  userId: string;
  username: string;
  grade: Grade;
  profileImagePath: string;
  information: string;
  goodSubject1: Subject;
  goodSubject2: Subject;
  goodSubject3: Subject;
  badSubject1: Subject;
  badSubject2: Subject;
  badSubject3: Subject;
  lastMessageSentByMe: boolean | null;
  lastMessage: string | null;
  lastMessageDateSent: Date | null;
}