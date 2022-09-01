namespace Learnz.Entities;
public class TestGroup
{
    public Guid Id { get; set; }
    public Guid TestId { get; set; }
    public Test Test { get; set; }
    public Guid GroupId { get; set; }
    public Group Group { get; set; }
}