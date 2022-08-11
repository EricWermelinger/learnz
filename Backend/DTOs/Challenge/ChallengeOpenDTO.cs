namespace Learnz.DTOs;
public class ChallengeOpenDTO
{
    public Guid ChallengeId { get; set; }
    public string Name { get; set; }
    public string CreateSetName { get; set; }
    public Subject SubjectMain { get; set; }
    public Subject? SubjectSecond { get; set; }
    public int NumberOfPlayers { get; set; }
    public bool IsOwner { get; set; }
}