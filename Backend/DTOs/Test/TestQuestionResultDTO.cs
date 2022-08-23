namespace Learnz.DTOs;
public class TestQuestionResultDTO
{
    public GeneralQuestionQuestionDTO Question { get; set; }
    public string? Answer { get; set; }
    public bool WasRight { get; set; }
    public int PointsScored { get; set; }
    public int PointsPossible { get; set; }
}