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
    public CreateFilterSets(DataContext dataContext, IUserService userService, ISetPolicyChecker setPolicyChecker)
    {
        _dataContext = dataContext;
        _userService = userService;
        _setPolicyChecker = setPolicyChecker;
    }

    [HttpGet]
    public async Task<ActionResult<List<CreateSetOverviewDTO>>> GetSets(Subject? subjectMain, Subject? subjectSecond, string name)
    {
        var guid = _userService.GetUserGuid();
        var sets = await _dataContext.CreateSets.Where(crs => ((subjectMain != null && subjectSecond != null && ((crs.SubjectMain == subjectMain && crs.SubjectSecond == subjectSecond) || (crs.SubjectMain == subjectSecond && crs.SubjectSecond == subjectMain)))
                                                            || subjectMain == null
                                                            || (subjectSecond == null && crs.SubjectMain == subjectMain))
                                                                && (name == null || name == "" || crs.Name.Contains(name)))
                                                .Include(crs => crs.CreatedBy)
                                                .ToListAsync();
        var setsWithPolicy = sets.Select(crs => new CreateSetOverviewDTO
        {
            SetId = crs.Id,
            Description = crs.Description,
            Name = crs.Name,
            Owner = crs.CreatedBy.Username,
            SubjectMain = crs.SubjectMain,
            SubjectSecond = crs.SubjectSecond,
            NumberOfQuestions = crs.QuestionDistributes.Count()
                                + crs.QuestionMathematics.Count()
                                + crs.QuestionMultipleChoices.Count()
                                + crs.QuestionOpenQuestions.Count()
                                + crs.QuestionTextFields.Count()
                                + crs.QuestionTrueFalses.Count()
                                + crs.QuestionWords.Count(),
            Usable = _setPolicyChecker.SetUsable(crs, guid),
            Editable = _setPolicyChecker.SetEditable(crs, guid)
        })
        .Where(set => set.Usable);
        return Ok(setsWithPolicy);
    }
}