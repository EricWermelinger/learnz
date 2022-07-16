import { TogetherUserProfileDTO } from "./TogetherUserProfileDTO";

export interface TogetherConnectionOverviewDTO {
  openAsks: TogetherUserProfileDTO[];
  sentAsks: TogetherUserProfileDTO[];
}