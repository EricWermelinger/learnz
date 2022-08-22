namespace Learnz.DTOs;
public class TestGroupTestCreateDTO
{
    public Guid TestId { get; set; }
    public Guid GroupId { get; set; }
    public Guid SetId { get; set; }
    public string Name { get; set; }
    public int MaxTime { get; set; }
}