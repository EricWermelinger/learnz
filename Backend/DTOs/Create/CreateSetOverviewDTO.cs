namespace Learnz.DTOs;
public class CreateSetOverviewDTO
{
    public Guid SetId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Subject SubjectMain { get; set; }
    public Subject? SubjectSecond { get; set; }
    public int NumberOfQuestions { get; set; }
    public string Owner { get; set; }
    public bool Usable { get; set; }
    public bool Editable { get; set; }
}