using System.Data;

namespace Learnz.Services;
public class TestQueryService : ITestQueryService
{
    private readonly DataContext _dataContext;
    public TestQueryService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<bool> CreateTestQuestions(Guid testId, Guid setId)
    {
        var set = await _dataContext.CreateSets
            .Where(crs => crs.Id == setId)
            .Include(crs => crs.QuestionDistributes)
            .ThenInclude(qd => qd.Answers)
            .Include(crs => crs.QuestionMathematics)
            .ThenInclude(qm => qm.Variables)
            .Include(crs => crs.QuestionMultipleChoices)
            .ThenInclude(qmc => qmc.Answers)
            .Include(crs => crs.QuestionOpenQuestions)
            .Include(crs => crs.QuestionTrueFalses)
            .Include(crs => crs.QuestionWords)
            .FirstOrDefaultAsync();
        if (set == null)
        {
            return false;
        }

        foreach (var question in set.QuestionDistributes)
        {
            var newQuestion = new TestQuestion
            {
                TestId = testId,
                QuestionId = question.Id,
                Question = question.Question,
                QuestionType = QuestionType.Distribute,
                PossibleAnswers = string.Join("|||", question.Answers.Select(ans => ans.LeftSideId.ToString() + "|" + ans.LeftSide + "||" + ans.RightSideId.ToString() + "|" + ans.RightSide)),
                RightAnswer = string.Join("|||", question.Answers.Select(ans => ans.LeftSideId.ToString() + "|" + ans.LeftSide + "||" + ans.RightSideId.ToString() + "|" + ans.RightSide)),
                PointsPossible = 1,
                Visible = true
            };
            _dataContext.TestQuestions.Add(newQuestion);
        }
        foreach (var question in set.QuestionMathematics)
        {
            string mathematicQuestion = question.Question;
            string mathematicAnswer = question.Answer;
            string answerComputed = "";
            Random random = new();
            foreach (var variable in question.Variables)
            {
                if (variable != null)
                {
                    int steps = (int)Math.Floor((variable.Max - variable.Min) / variable.Interval == 0 ? 1 : variable.Interval);
                    int randomStep = random.Next(0, steps);
                    double variableValue = Math.Round(variable.Min + randomStep * variable.Interval, variable.Digits);
                    mathematicQuestion = mathematicQuestion.Replace(variable.Display, variableValue.ToString());
                    mathematicAnswer = mathematicAnswer.Replace(variable.Display, variableValue.ToString());
                }
            }
            try
            {
                var mathematicAnswerObj = new DataTable().Compute(mathematicAnswer, "");
                if (mathematicAnswerObj != null)
                {
                    answerComputed = mathematicAnswerObj.ToString();
                }
            }
            catch
            {
                continue;
            }
            var newQuestion = new TestQuestion
            {
                TestId = testId,
                QuestionId = question.Id,
                Question = mathematicQuestion,
                QuestionType = QuestionType.Mathematic,
                Description = question.Digits.ToString(),
                RightAnswer = answerComputed ?? "",
                PointsPossible = 1,
                Visible = true
            };
            _dataContext.TestQuestions.Add(newQuestion);
        }
        foreach (var question in set.QuestionMultipleChoices)
        {
            var newQuestion = new TestQuestion
            {
                TestId = testId,
                QuestionId = question.Id,
                Question = question.Question,
                QuestionType = QuestionType.MultipleChoice,
                PossibleAnswers = string.Join("||", question.Answers.Select(ans => ans.Id.ToString() + "|" + ans.Answer)),
                RightAnswer = string.Join("||", question.Answers.Where(ans => ans.IsRight).Select(ans => ans.Id.ToString() + "|" + ans.Answer)),
                PointsPossible = 1,
                Visible = true
            };
            _dataContext.TestQuestions.Add(newQuestion);
        }
        foreach (var question in set.QuestionOpenQuestions)
        {
            var newQuestion = new TestQuestion
            {
                TestId = testId,
                QuestionId = question.Id,
                Question = question.Question,
                QuestionType = QuestionType.OpenQuestion,
                RightAnswer = question.Answer,
                PointsPossible = 1,
                Visible = true
            };
            _dataContext.TestQuestions.Add(newQuestion);
        }
        foreach (var question in set.QuestionTrueFalses)
        {
            var newQuestion = new TestQuestion
            {
                TestId = testId,
                QuestionId = question.Id,
                Question = question.Question,
                QuestionType = QuestionType.TrueFalse,
                RightAnswer = question.Answer ? "true" : "false",
                PointsPossible = 1,
                Visible = true
            };
            _dataContext.TestQuestions.Add(newQuestion);
        }
        foreach (var question in set.QuestionWords)
        {
            var newQuestion = new TestQuestion
            {
                TestId = testId,
                QuestionId = question.Id,
                Question = question.LanguageSubjectMain,
                QuestionType = QuestionType.Word,
                Description = set.SubjectSecond == null ? null : ((int)set.SubjectSecond).ToString(),
                RightAnswer = question.LanguageSubjectSecond,
                PointsPossible = 1,
                Visible = true
            };
            _dataContext.TestQuestions.Add(newQuestion);
        }
        await _dataContext.SaveChangesAsync();

        return true;
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
                return answer.Trim().ToLower() == rightAnswer.Trim().ToLower();
        }
    }
}