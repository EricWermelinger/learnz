namespace Learnz.DTOs;
public class TestResultOverviewDTO
{
    public string TestName { get; set; }
    public int PointsPossible { get; set; }
    public int MaxTime { get; set; }
    public List<TestResultOverviewUserDTO> Results { get; set; }
}