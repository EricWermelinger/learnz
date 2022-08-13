using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CreateSetQuestions : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly ISetPolicyChecker _setPolicyChecker;
    public CreateSetQuestions(DataContext dataContext, IUserService userService, ISetPolicyChecker setPolicyChecker)
    {
        _dataContext = dataContext;
        _userService = userService;
        _setPolicyChecker = setPolicyChecker;
    }

    [HttpGet]
    public async Task<ActionResult<CreateUpsertSetQuestionsDTO>> QuestionsOfSet(Guid setId)
    {
        var guid = _userService.GetUserGuid();
        var set = await _dataContext.CreateSets
                                    .Include(crs => crs.QuestionDistributes)
                                    .ThenInclude(qd => qd.Answers)
                                    .Include(crs => crs.QuestionMathematics)
                                    .ThenInclude(qm => qm.Variables)
                                    .Include(crs => crs.QuestionMultipleChoices)
                                    .ThenInclude(qmc => qmc.Answers)
                                    .Include(crs => crs.QuestionOpenQuestions)
                                    .Include(crs => crs.QuestionTextFields)
                                    .Include(crs => crs.QuestionTrueFalses)
                                    .Include(crs => crs.QuestionWords)
                                    .FirstOrDefaultAsync(crs => crs.Id == setId);
        if (set == null || !_setPolicyChecker.SetUsable(set, guid))
        {
            return NotFound(ErrorKeys.SetNotAccessible);
        }
        var questions = new CreateUpsertSetQuestionsDTO
        {
            SetId = set.Id,
            QuestionsDistribute = set.QuestionDistributes.Select(qd => new CreateQuestionDistributeDTO
            {
                Id = qd.Id,
                Question = qd.Question,
                Answers = qd.Answers.Select(qda => new CreateQuestionDistributeAnswerDTO
                {
                    Id = qda.Id,
                    LeftSide = qda.LeftSide,
                    LeftSideId = qda.LeftSideId,
                    RightSide = qda.RightSide,
                    RightSideId = qda.RightSideId
                }).ToList()
            }).ToList(),
            QuestionsMathematic = set.QuestionMathematics.Select(qm => new CreateQuestionMathematicDTO
            {
                Id = qm.Id,
                Question = qm.Question,
                Answer = qm.Answer,
                Digits = qm.Digits,
                Variables = qm.Variables.Select(qmv => new CreateQuestionMathematicVariableDTO
                {
                    Id = qmv.Id,
                    Digits = qmv.Digits,
                    Display = qmv.Display,
                    Interval = qmv.Interval,
                    Min = qmv.Min,
                    Max = qmv.Max
                }).ToList()
            }).ToList(),
            QuestionsMultipleChoice = set.QuestionMultipleChoices.Select(qmc => new CreateQuestionMultipleChoiceDTO
            {
                Id = qmc.Id,
                Question = qmc.Question,
                Answers = qmc.Answers.Select(qmca => new CreateQuestionMultipleChoiceAnswerDTO
                {
                    Id = qmca.Id,
                    Answer = qmca.Answer,
                    IsRight = qmca.IsRight
                }).ToList()
            }).ToList(),
            QuestionsOpenQuestion = set.QuestionOpenQuestions.Select(qoq => new CreateQuestionOpenQuestionDTO
            {
                Id = qoq.Id,
                Answer = qoq.Answer,
                Question = qoq.Question
            }).ToList(),
            QuestionsTextField = set.QuestionTextFields.Select(qtf => new CreateQuestionTextFieldDTO
            {
                Id = qtf.Id,
                Question = qtf.Question
            }).ToList(),
            QuestionsTrueFalse = set.QuestionTrueFalses.Select(qtf => new CreateQuestionTrueFalseDTO
            {
                Id = qtf.Id,
                Question = qtf.Question,
                Answer = qtf.Answer
            }).ToList(),
            QuestionsWord = set.QuestionWords.Select(qw => new CreateQuestionWordDTO
            {
                Id = qw.Id,
                LanguageSubjectMain = qw.LanguageSubjectMain,
                LanguageSubjectSecond = qw.LanguageSubjectSecond
            }).ToList()
        };
        return Ok(questions);
    }

    [HttpPost]
    public async Task<ActionResult> UpsertQuestions(CreateUpsertSetQuestionsDTO request)
    {
        var guid = _userService.GetUserGuid();
        var setId = request.SetId;
        var set = await _dataContext.CreateSets
                                            .Include(crs => crs.QuestionDistributes)
                                            .ThenInclude(qd => qd.Answers)
                                            .Include(crs => crs.QuestionMathematics)
                                            .ThenInclude(qm => qm.Variables)
                                            .Include(crs => crs.QuestionMultipleChoices)
                                            .ThenInclude(qmc => qmc.Answers)
                                            .Include(crs => crs.QuestionOpenQuestions)
                                            .Include(crs => crs.QuestionTextFields)
                                            .Include(crs => crs.QuestionTrueFalses)
                                            .Include(crs => crs.QuestionWords)
                                            .FirstOrDefaultAsync(crs => crs.Id == request.SetId);
        if (set != null && !_setPolicyChecker.SetEditable(set, guid))
        {
            return BadRequest(ErrorKeys.SetNotAccessible);
        }

        if (set != null)
        {
            var distAnswersId = request.QuestionsDistribute.SelectMany(q => q.Answers).Select(a => a.Id);
            var distAnswersDeleted = set.QuestionDistributes.SelectMany(q => q.Answers).Where(a => !distAnswersId.Contains(a.Id));
            var distQuestionsId = request.QuestionsDistribute.Select(q => q.Id);
            var distQuestionsDeleted = set.QuestionDistributes.Where(q => !distQuestionsId.Contains(q.Id));

            var mathVariablesId = request.QuestionsMathematic.SelectMany(q => q.Variables).Select(a => a.Id);
            var mathVariablesDeleted = set.QuestionMathematics.SelectMany(q => q.Variables).Where(a => !mathVariablesId.Contains(a.Id));
            var mathQuestionsId = request.QuestionsMathematic.Select(q => q.Id);
            var mathQuestionsDeleted = set.QuestionMathematics.Where(q => !mathQuestionsId.Contains(q.Id));

            var mcAnswersId = request.QuestionsMultipleChoice.SelectMany(q => q.Answers).Select(a => a.Id);
            var mcAnswersDeleted = set.QuestionMultipleChoices.SelectMany(q => q.Answers).Where(a => !mcAnswersId.Contains(a.Id));
            var mcQuestionsId = request.QuestionsMultipleChoice.Select(q => q.Id);
            var mcQuestionsDeleted = set.QuestionMultipleChoices.Where(q => !mcQuestionsId.Contains(q.Id));

            var oqQuestionsId = request.QuestionsOpenQuestion.Select(q => q.Id);
            var oqQuestionsDeleted = set.QuestionOpenQuestions.Where(q => !oqQuestionsId.Contains(q.Id));

            var tfQuestionsId = request.QuestionsTrueFalse.Select(q => q.Id);
            var tfQuestionsDeleted = set.QuestionTrueFalses.Where(q => !tfQuestionsId.Contains(q.Id));

            var wordQuestionsId = request.QuestionsWord.Select(q => q.Id);
            var wordQuestionsDeleted = set.QuestionWords.Where(q => !wordQuestionsId.Contains(q.Id));

            var textFieldQuestionsId = request.QuestionsTextField.Select(q => q.Id);
            var textFieldQuestionsDeleted = set.QuestionTextFields.Where(q => !textFieldQuestionsId.Contains(q.Id));

            _dataContext.RemoveRange(distAnswersDeleted);
            _dataContext.RemoveRange(distQuestionsDeleted);
            _dataContext.RemoveRange(mathVariablesDeleted);
            _dataContext.RemoveRange(mathQuestionsDeleted);
            _dataContext.RemoveRange(mcAnswersDeleted);
            _dataContext.RemoveRange(mcQuestionsDeleted);
            _dataContext.RemoveRange(oqQuestionsDeleted);
            _dataContext.RemoveRange(tfQuestionsDeleted);
            _dataContext.RemoveRange(wordQuestionsDeleted);
            _dataContext.RemoveRange(textFieldQuestionsDeleted);
            await _dataContext.SaveChangesAsync();
        }

        var distExistingQuestions = await _dataContext.CreateQuestionDistributes.Where(q => q.SetId == setId).ToListAsync();
        var distUpdatedQuestions = request.QuestionsDistribute.Select(q => new CreateQuestionDistribute
        {
            Id = q.Id,
            Question = q.Question,
            SetId = setId,
        }).ToList();
        foreach (var question in distUpdatedQuestions)
        {
            var existingQuestion = distExistingQuestions.FirstOrDefault(q => q.Id == question.Id);
            if (existingQuestion != null)
            {
                existingQuestion.Question = question.Question;
            }
            else
            {
                _dataContext.CreateQuestionDistributes.Add(question);
            }
        }
        var distExistingAnswers = await _dataContext.CreateQuestionDistributeAnswers.Where(a => distExistingQuestions.Select(q => q.Id).Contains(a.QuestionDistributeId)).ToListAsync();
        var distUpdatedAnswers = request.QuestionsDistribute.SelectMany(q => q.Answers!.Select(a => new { Answer = a, QuestionId = q.Id })).Select(a => new CreateQuestionDistributeAnswer
        {
            Id = a.Answer.Id,
            LeftSide = a.Answer.LeftSide,
            LeftSideId = a.Answer.LeftSideId,
            RightSide = a.Answer.RightSide,
            RightSideId = a.Answer.RightSideId,
            QuestionDistributeId = a.QuestionId
        }).ToList();
        foreach (var answer in distUpdatedAnswers)
        {
            var existingAnswer = distExistingAnswers.FirstOrDefault(a => a.Id == answer.Id);
            if (existingAnswer != null)
            {
                existingAnswer.LeftSide = answer.LeftSide;
                existingAnswer.LeftSideId = answer.LeftSideId;
                existingAnswer.RightSide = answer.RightSide;
                existingAnswer.RightSideId = answer.RightSideId;
            }
            else
            {
                _dataContext.CreateQuestionDistributeAnswers.Add(answer);
            }
        }

        var mathExistingQuestions = _dataContext.CreateQuestionMathematics.Where(q => q.SetId == setId).ToList();
        var mathUpdatedQuestions = request.QuestionsMathematic.Select(q => new CreateQuestionMathematic
        {
            Id = q.Id,
            Question = q.Question,
            Answer = q.Answer,
            Digits = q.Digits,
            SetId = setId
        }).ToList();
        foreach (var question in mathUpdatedQuestions)
        {
            var existingQuestion = mathExistingQuestions.FirstOrDefault(q => q.Id == question.Id);
            if (existingQuestion != null)
            {
                existingQuestion.Question = question.Question;
                existingQuestion.Answer = question.Answer;
                existingQuestion.Digits = question.Digits;
            }
            else
            {
                _dataContext.CreateQuestionMathematics.Add(question);
            }
        }
        var mathExistingVariables = _dataContext.CreateQuestionMathematicVariables.Where(v => mathExistingQuestions.Select(q => q.Id).Contains(v.QuestionMathematicId)).ToList();
        var mathUpdatedVariables = request.QuestionsMathematic.SelectMany(q => q.Variables!.Select(a => new { Variable = a, QuestionId = q.Id })).Select(a => new CreateQuestionMathematicVariable
        {
            Id = a.Variable.Id,
            Digits = a.Variable.Digits,
            Display = a.Variable.Display,
            Interval = a.Variable.Interval,
            Max = a.Variable.Max,
            Min = a.Variable.Min,
            QuestionMathematicId = a.QuestionId
        }).ToList();
        foreach (var variable in mathUpdatedVariables)
        {
            var existingVariable = mathExistingVariables.FirstOrDefault(v => v.Id == variable.Id);
            if (existingVariable != null)
            {
                existingVariable.Digits = variable.Digits;
                existingVariable.Display = variable.Display;
                existingVariable.Interval = variable.Interval;
                existingVariable.Max = variable.Max;
                existingVariable.Min = variable.Min;
            }
            else
            {
                _dataContext.CreateQuestionMathematicVariables.Add(variable);
            }
        }

        var mcExistingQuestions = _dataContext.CreateQuestionMultipleChoices.Where(q => q.SetId == setId).ToList();
        var mcUpdatedQuestions = request.QuestionsMultipleChoice.Select(q => new CreateQuestionMultipleChoice
        {
            Id = q.Id,
            Question = q.Question,
            SetId = setId
        }).ToList();
        foreach (var question in mcUpdatedQuestions)
        {
            var existingQuestion = mcExistingQuestions.FirstOrDefault(q => q.Id == question.Id);
            if (existingQuestion != null)
            {
                existingQuestion.Question = question.Question;
            }
            else
            {
                _dataContext.CreateQuestionMultipleChoices.Add(question);
            }
        }
        var mcExistingAnswers = _dataContext.CreateQuestionMultipleChoiceAnswers.Where(a => mcExistingQuestions.Select(q => q.Id).Contains(a.QuestionMultipleChoiceId)).ToList();
        var mcUpdatedAnswers = request.QuestionsMultipleChoice.SelectMany(q => q.Answers!.Select(a => new { Answer = a, QuestionId = q.Id })).Select(a => new CreateQuestionMultipleChoiceAnswer
        {
            Id = a.Answer.Id,
            Answer = a.Answer.Answer,
            QuestionMultipleChoiceId = a.QuestionId,
            IsRight = a.Answer.IsRight
        }).ToList();
        foreach (var answer in mcUpdatedAnswers)
        {
            var existingAnswer = mcExistingAnswers.FirstOrDefault(a => a.Id == answer.Id);
            if (existingAnswer != null)
            {
                existingAnswer.Answer = answer.Answer;
                existingAnswer.IsRight = answer.IsRight;
            }
            else
            {
                _dataContext.CreateQuestionMultipleChoiceAnswers.Add(answer);
            }
        }

        var oqExistingQuestions = _dataContext.CreateQuestionOpenQuestions.Where(q => q.SetId == setId).ToList();
        var oqUpdatedQuestions = request.QuestionsOpenQuestion.Select(q => new CreateQuestionOpenQuestion
        {
            Id = q.Id,
            Question = q.Question,
            Answer = q.Answer,
            SetId = setId
        }).ToList();
        foreach (var question in oqUpdatedQuestions)
        {
            var existingQuestion = oqExistingQuestions.FirstOrDefault(q => q.Id == question.Id);
            if (existingQuestion != null)
            {
                existingQuestion.Question = question.Question;
                existingQuestion.Answer = question.Answer;
            }
            else
            {
                _dataContext.CreateQuestionOpenQuestions.Add(question);
            }
        }

        var tfExistingQuestions = _dataContext.CreateQuestionTextFields.Where(q => q.SetId == setId).ToList();
        var tfUpdatedQuestions = request.QuestionsTextField.Select(q => new CreateQuestionTextField
        {
            Id = q.Id,
            Question = q.Question,
            SetId = setId
        }).ToList();
        foreach (var question in tfUpdatedQuestions)
        {
            var existingQuestion = tfExistingQuestions.FirstOrDefault(q => q.Id == question.Id);
            if (existingQuestion != null)
            {
                existingQuestion.Question = question.Question;
            }
            else
            {
                _dataContext.CreateQuestionTextFields.Add(question);
            }
        }

        var trueFalseExistingQuestions = _dataContext.CreateQuestionTrueFalses.Where(q => q.SetId == setId).ToList();
        var trueFalseUpdatedQuestions = request.QuestionsTrueFalse.Select(q => new CreateQuestionTrueFalse
        {
            Id = q.Id,
            Question = q.Question,
            Answer = q.Answer,
            SetId = setId
        }).ToList();
        foreach (var question in trueFalseUpdatedQuestions)
        {
            var existingQuestion = trueFalseExistingQuestions.FirstOrDefault(q => q.Id == question.Id);
            if (existingQuestion != null)
            {
                existingQuestion.Question = question.Question;
                existingQuestion.Answer = question.Answer;
            }
            else
            {
                _dataContext.CreateQuestionTrueFalses.Add(question);
            }
        }

        var wordExistingQuestions = _dataContext.CreateQuestionWords.Where(q => q.SetId == setId).ToList();
        var wordUpdatedQuestions = request.QuestionsWord.Select(q => new CreateQuestionWord
        {
            Id = q.Id,
            LanguageSubjectMain = q.LanguageSubjectMain,
            LanguageSubjectSecond = q.LanguageSubjectSecond,
            SetId = setId
        }).ToList();
        foreach (var question in wordUpdatedQuestions)
        {
            var existingQuestion = wordExistingQuestions.FirstOrDefault(q => q.Id == question.Id);
            if (existingQuestion != null)
            {
                existingQuestion.LanguageSubjectMain = question.LanguageSubjectMain;
                existingQuestion.LanguageSubjectSecond = question.LanguageSubjectSecond;
            }
            else
            {
                _dataContext.CreateQuestionWords.Add(question);
            }
        }

        await _dataContext.SaveChangesAsync();
        return Ok();
    }
}
