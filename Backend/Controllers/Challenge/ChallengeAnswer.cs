﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ChallengeAnswer : Controller
{
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;
    public ChallengeAnswer(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

}
