import { LearnzFileFrontendDTO } from "../File/LearnzFileFrontendDTO";
import { GroupInfoMemberDTO } from "./GroupInfoMemberDTO";

export interface GroupInfoDTO {
  groupId: string;
  name: string;
  description: string;
  profileImage: LearnzFileFrontendDTO;
  members: GroupInfoMemberDTO[];
  isUserAdmin: boolean;
}