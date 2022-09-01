namespace Learnz.Entities;
public class TestQuestionOfUser
{
    public Guid Id { get; set; }
    public Guid TestOfUserId { get; set; }
    public TestOfUser TestOfUser { get; set; }
    public Guid TestQuestionId { get; set; }
    public TestQuestion TestQuestion { get; set; }
    public string? AnswerByUser { get; set; }
    public bool? AnsweredCorrect { get; set; }
    public int? PointsScored { get; set; }
}