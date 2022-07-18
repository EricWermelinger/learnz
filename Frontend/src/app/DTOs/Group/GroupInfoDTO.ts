import { GroupInfoMemberDTO } from "./GroupInfoMemberDTO";

export interface GroupInfoDTO {
  groupId: string;
  name: string;
  description: string;
  profileImagePath: string;
  members: GroupInfoMemberDTO[];
  isUserAdmin: boolean;
}