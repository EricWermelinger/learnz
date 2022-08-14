namespace Learnz.DTOs;
public class ChallengeCreateDTO
{
    public Guid ChallengeId { get; set; }
    public string Name { get; set; }
    public Guid CreateSetId { get; set; }
}