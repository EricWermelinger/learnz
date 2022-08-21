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
                var answerDistributeShould = rightAnswer.Split("|||").Select(ans => ans.Split("||")[0].Split("|")[0] + "|" + ans.Split("||")[1].Split("|")[0]).OrderBy(x => x).ToList();
                var answerDistributeGiven = answer.Split("||").OrderBy(x => x).ToList();
                if (answerDistributeShould.Count != answerDistributeGiven.Count)
                {
                    return false;
                }
                for (int i = 0; i < answerDistributeShould.Count; i++)
                {
                    if (answerDistributeShould[i] != answerDistributeGiven[i])
                    {
                        return false;
                    }
                }
                return true;
            case QuestionType.MultipleChoice:
                var answerMultipleChoiceShould = rightAnswer.Split("||").Select(ans => ans.Split("|")[0]).OrderBy(x => x).ToList();
                var answerMultipleChoiceGiven = answer.Split("|").OrderBy(x => x).ToList();
                if (answerMultipleChoiceShould.Count != answerMultipleChoiceGiven.Count)
                {
                    return false;
                }
                for (int i = 0; i < answerMultipleChoiceShould.Count; i++)
                {
                    if (answerMultipleChoiceShould[i] != answerMultipleChoiceGiven[i])
                    {
                        return false;
                    }
                }
                return true;
            default:
                return answer.ToLower() == rightAnswer.ToLower();
        }
    }
}