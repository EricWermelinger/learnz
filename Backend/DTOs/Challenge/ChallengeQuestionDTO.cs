﻿namespace Learnz.DTOs;
public class ChallengeQuestionDTO
{
    public string Question { get; set; }
    public string? Description { get; set; }
    public QuestionType QuestionType { get; set; }
    public List<ChallengeQuestionAnswerDTO>? AnswerSetOne { get; set; }
    public List<ChallengeQuestionAnswerDTO>? AnswerSetTwo { get; set; }
}