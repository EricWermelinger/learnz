namespace Learnz.DTOs;
public class GroupInfoCreateDTO
{
    public Guid GroupId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid ProfileImageId { get; set; }
    public List<Guid> Members { get; set; }
}
