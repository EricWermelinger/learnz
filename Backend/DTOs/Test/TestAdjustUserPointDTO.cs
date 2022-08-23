namespace Learnz.DTOs;
public class TestAdjustUserPointDTO
{
    public Guid UserId { get; set; }
    public Guid TestId { get; set; }
    public Guid QuestionId { get; set; }
    public int PointsScored { get; set; }
    public bool IsCorrect { get; set; }
}