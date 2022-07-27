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
                    RightSide = qda.RightSide
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
}
