namespace Learnz.Services;
public interface IChallengeQueryService
{
    Task TriggerWebsocket(Guid challengeId, Guid guid);
    Task TriggerWebsocket(ChallengeActiveDTO challenge, Guid challengeId, Guid guid);
    Task TriggerWebsocketAllUsers(Guid challengeId, bool? cancelledEvluated = null);
    Task<ChallengeActiveDTO> GetActiveChallenge(Challenge challenge, Guid guid,
        ChallengeQuestionPosed? currentQuestionEvaluated = null, int? numberOfAnswersEvaluated = null,
        int? numberOfPlayersEvaluated = null, GeneralQuestionQuestionDTO? currentQuestionDtoEvaluated = null,
        List<ChallengePlayerResultDTO>? resultEvaluated = null, bool? cancelledEvluated = null);
    Task<List<ChallengePlayerResultDTO>> GetResult(Guid challengeId);
    Task<GeneralQuestionQuestionDTO?> GetQuestionById(Guid questionId);
    Task<int?> GetLastQuestionPoints(Guid challengeId, Guid questionId, Guid userId);
}