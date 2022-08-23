namespace Learnz.DTOs;
public class TestResultDTO
{
    public string Name { get; set; }
    public int MaxTime { get; set; }
    public int TimeUsed { get; set; }
    public Subject SubjectMain { get; set; }
    public Subject? SubjectSecond { get; set; }
    public int PointsScored { get; set; }
    public int PointsPossible { get; set; }
    public List<TestQuestionResultDTO> Questions { get; set; }
}