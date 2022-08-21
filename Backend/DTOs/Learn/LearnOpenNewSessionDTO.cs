namespace Learnz.DTOs;
public class LearnOpenNewSessionDTO
{
    public Guid LearnSessionId { get; set; }
    public Guid SetId { get; set; }
    public bool OnlyHardQuestions { get; set; }
}