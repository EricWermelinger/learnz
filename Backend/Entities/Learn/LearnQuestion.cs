namespace Learnz.Entities;
public class LearnQuestion
{
    public Guid Id { get; set; }
    public Guid LearnSessionId { get; set; }
    public LearnSession LearnSession { get; set; }
    public Guid QuestionId { get; set; }
    public string Question { get; set; }
    public string? Description { get; set; }
    public QuestionType QuestionType { get; set; }
    public string? PossibleAnswers { get; set; }
    public string RightAnswer { get; set; }
    public string? AnswerByUser { get; set; }
    public bool? AnsweredCorrect { get; set; }
    public bool? MarkedAsHard { get; set; }
}