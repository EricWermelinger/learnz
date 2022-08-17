namespace Learnz.Services;
public class LearnQueryService : ILearnQueryService
{
    public string GetAnswer(LearnQuestion question)
    {
        string answer = "";
        switch (question.QuestionType)
        {
            case QuestionType.Distribute:
                answer = string.Join(" & ", question.RightAnswer.Split("|||").Select(ans => ans.Split("||")[0].Split("|")[1] + " - " + ans.Split("||")[1].Split("|")[1]));
                break;
            case QuestionType.MultipleChoice:
                answer = string.Join(" & ", question.RightAnswer.Split("||").Select(ans => ans.Split("|")[1]));
                break;
            case QuestionType.Mathematic:
            case QuestionType.OpenQuestion:
            case QuestionType.TrueFalse:
            case QuestionType.Word:
                answer = question.RightAnswer;
                break;
        }
        return answer;
    }

    public bool EvaluateAnswer(string answer, string rightAnswer, QuestionType questionType)
    {
        switch (questionType)
        {
            case QuestionType.Distribute:
                // todo
                break;
            case QuestionType.MultipleChoice:
                // todo
                break;
            default:
                return answer.ToLower() == rightAnswer.ToLower();
        }
        return false;
    }
}