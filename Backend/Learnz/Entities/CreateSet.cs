namespace Learnz.Entities;
public class CreateSet
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Subject Subject { get; set; }
    public DateTime Created { get; set; }
    public Guid CreatedById { get; set; }
    public User CreatedBy { get; set; }
    public DateTime Modified { get; set; }
    public Guid ModifiedById { get; set; }
    public User ModifiedBy { get; set; }
    public SetPolicy SetPolicy { get; set; }
}
