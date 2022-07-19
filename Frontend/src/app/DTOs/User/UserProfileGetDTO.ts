import { LearnzFileFrontendDTO } from "../File/LearnzFileFrontendDTO";

export interface UserProfileGetDTO {
  username: string;
  firstname: string;
  lastname: string;
  birthdate: Date;
  grade: number;
  profileImage: LearnzFileFrontendDTO;
  information: string;
  language: number;
  goodSubject1: number;
  goodSubject2: number;
  goodSubject3: number;
  badSubject1: number;
  badSubject2: number;
  badSubject3: number;
  darkTheme: boolean;
}