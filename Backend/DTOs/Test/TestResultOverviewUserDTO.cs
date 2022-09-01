namespace Learnz.DTOs;
public class TestResultOverviewUserDTO
{
    public Guid UserId { get; set; }
    public Guid TestOfUserId { get; set; }
    public string Username { get; set; }
    public int PointsScored { get; set; }
    public int TimeUsed { get; set; }
}