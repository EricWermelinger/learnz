namespace Learnz.Services;
public interface ILearnQueryService
{
    string GetAnswer(LearnQuestion question);
    bool EvaluateAnswer(string answer, string rightAnswer, QuestionType questionType);
}