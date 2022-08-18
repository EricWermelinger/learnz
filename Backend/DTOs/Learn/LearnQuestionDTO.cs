namespace Learnz.DTOs;
public class LearnQuestionDTO
{
    public GeneralQuestionQuestionDTO Question { get; set; }
    public bool Answered { get; set; }
    public bool? AnsweredCorrect { get; set; }
    public bool MarkedAsHard { get; set; }
    public string? Solution { get; set; }
}