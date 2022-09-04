namespace Learnz.DTOs;
public class DrawDrawingDTO
{
    public List<DrawPageGetDTO> Pages { get; set; }
    public string Name { get; set; }
    public bool Editable { get; set; }
    public string? NewUserMakingChangesName { get; set; }
}