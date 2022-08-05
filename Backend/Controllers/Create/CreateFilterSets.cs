using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CreateFilterSets : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    private readonly ISetPolicyChecker _setPolicyChecker;
    private readonly ICreateQueryService _createQueryService;
    public CreateFilterSets(DataContext dataContext, IUserService userService, ISetPolicyChecker setPolicyChecker, ICreateQueryService createQueryService)
    {
        _dataContext = dataContext;
        _userService = userService;
        _setPolicyChecker = setPolicyChecker;
        _createQueryService = createQueryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CreateSetOverviewDTO>>> GetSets(int? subjectMain, int? subjectSecond, string? name)
    {
        var guid = _userService.GetUserGuid();
        var sets = await _dataContext.CreateSets.Where(crs => ((subjectMain != null && subjectMain != -1 && subjectSecond != null && subjectSecond != -1 && (((int)crs.SubjectMain == subjectMain && (crs.SubjectSecond == null ? 0 : (int)crs.SubjectSecond) == subjectSecond) || ((int)crs.SubjectMain == subjectSecond && (crs.SubjectSecond == null ? 0 : (int)crs.SubjectSecond) == subjectMain)))
                                                            || subjectMain == null || subjectMain == -1
                                                            || ((subjectSecond == null || subjectSecond == -1 || subjectMain == subjectSecond) && (int)crs.SubjectMain == subjectMain))
                                                                && (name == null || name == "" || crs.Name.Contains(name)))
                                                .Include(crs => crs.CreatedBy)
                                                .Include(crs => crs.QuestionDistributes)
                                                .Include(crs => crs.QuestionMathematics)
                                                .Include(crs => crs.QuestionMultipleChoices)
                                                .Include(crs => crs.QuestionOpenQuestions)
                                                .Include(crs => crs.QuestionTextFields)
                                                .Include(crs => crs.QuestionTrueFalses)
                                                .Include(crs => crs.QuestionWords)
                                                .Select(crs => new
                                                {
                                                    Set = crs,
                                                    TieBreaker = Guid.NewGuid()
                                                })
                                                .OrderBy(x => x.TieBreaker)
                                                .Take(20)
                                                .Select(x => x.Set)
                                                .ToListAsync();
        var setsWithPolicy = sets.Select(crs => new CreateSetOverviewDTO
        {
            SetId = crs.Id,
            Name = crs.Name,
            Owner = crs.CreatedBy.Username,
            SubjectMain = crs.SubjectMain,
            SubjectSecond = crs.SubjectSecond,
            NumberOfQuestions = _createQueryService.NumberOfWords(crs),
            Usable = _setPolicyChecker.SetUsable(crs, guid),
            Editable = _setPolicyChecker.SetEditable(crs, guid)
        })
        .Where(set => set.Usable);
        return Ok(setsWithPolicy);
    }
}