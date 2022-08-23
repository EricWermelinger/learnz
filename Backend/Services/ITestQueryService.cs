namespace Learnz.Services;
public interface ITestQueryService
{
    Task<bool> CreateTestQuestions(Guid testId, Guid setId);
    bool EvaluateAnswer(string answer, string rightAnswer, QuestionType questionType);
}