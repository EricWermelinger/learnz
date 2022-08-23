namespace Learnz.DTOs;
public class TestDTO
{
    public Guid TestId { get; set; }
    public Guid? TestOfUserId { get; set; }
    public string Name { get; set; }
    public int MaxTime { get; set; }
    public Subject SubjectMain { get; set; }
    public Subject? SubjectSecond { get; set; }
    public bool IsGroupTest { get; set; }
    public bool IsOwner { get; set; }
    public int? PointsScored { get; set; }
    public int? PointsPossible { get; set; }
}