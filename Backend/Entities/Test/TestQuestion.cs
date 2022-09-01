namespace Learnz.Entities;
public class TestQuestion
{
    public Guid Id { get; set; }
    public Guid TestId { get; set; }
    public Test Test { get; set; }
    public Guid QuestionId { get; set; }
    public string Question { get; set; }
    public string? Description { get; set; }
    public QuestionType QuestionType { get; set; }
    public string? PossibleAnswers { get; set; }
    public string RightAnswer { get; set; }
    public int PointsPossible { get; set; }
    public bool Visible { get; set; }

    public ICollection<TestQuestionOfUser> TestQuestionOfUsers { get; set; }
}