namespace Learnz.Services;
public interface IChallengeQueryService
{
    Task TriggerWebsocket(Guid challengeId, Guid guid);
    Task TriggerWebsocket(ChallengeActiveDTO challenge, Guid challengeId, Guid guid);
    Task TriggerWebsocketAllUsers(Guid challengeId);
    Task<ChallengeActiveDTO> GetActiveChallenge(Guid challengeId);
    Task<ChallengeActiveDTO> GetActiveChallenge(Challenge challenge, Guid? guid);
    Task<List<ChallengePlayerResultDTO>> GetResult(Guid challengeId);
    Task<GeneralQuestionQuestionDTO?> GetQuestionById(Guid questionId);
    Task<int?> GetLastQuestionPoints(Guid challengeId, Guid questionId, Guid userId);
}