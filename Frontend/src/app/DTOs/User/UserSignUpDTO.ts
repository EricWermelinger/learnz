﻿export interface UserSignUpDTO {
  username: string;
  password: string;
  firstname: string;
  lastname: string;
  birthdate: Date;
  grade: number;
  profileImagePath: string;
  information: string;
  language: number;
  goodSubject1: number;
  goodSubject2: number;
  goodSubject3: number;
  badSubject1: number;
  badSubject2: number;
  badSubject3: number;
}