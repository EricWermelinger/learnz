namespace Learnz.DTOs;
public class CreateUpsertSetHeaderDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Subject SubjectMain { get; set; }
    public Subject? SubjectSecond { get; set; }
    public SetPolicy SetPolicy { get; set; }
    public bool IsEditable { get; set; }
    public bool IsPolicyEditable { get; set; }
}