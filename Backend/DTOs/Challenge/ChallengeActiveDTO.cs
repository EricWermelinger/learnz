namespace Learnz.DTOs;
public class ChallengeActiveDTO
{
    public string Name { get; set; }
    public List<ChallengePlayerResultDTO> Result { get; set; }
    public bool Cancelled { get; set; }
    public bool IsOwner { get; set; }
    public ChallengeQuestionDTO Question { get; set; }
    public ChallengeState State { get; set; }
}