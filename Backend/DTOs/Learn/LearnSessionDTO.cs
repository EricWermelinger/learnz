namespace Learnz.DTOs;
public class LearnSessionDTO
{
    public Guid LearnSessionId { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Ended { get; set; }
    public Guid SetId { get; set; }
    public string SetName { get; set; }
    public Subject SubjectMain { get; set; }
    public Subject? SubjectSecond { get; set; }
    public int NumberOfRightAnswers { get; set; }
    public int NumberOfWrongAnswers { get; set; }
    public int NumberOfNotAnswerd { get; set; }
}