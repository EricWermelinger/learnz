﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace Learnz.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class UserSignUp : Controller
{
    private readonly DataContext _dataContext;
    private readonly IConfiguration _configuration;
    private readonly IFileAnonymousFinder _fileAnonymousFinder;

    public UserSignUp(DataContext dataContext, IConfiguration configuration, IFileAnonymousFinder fileAnonymousFinder)
    {
        _dataContext = dataContext;
        _configuration = configuration;
        _fileAnonymousFinder = fileAnonymousFinder;
    }

    [HttpPost]
    public async Task<ActionResult<TokenDTO>> SignUp(UserSignUpDTO request)
    {
        if (request == null
            || request.Username == "" || request.Password == "" || request.Firstname == "" || request.Lastname == "" || request.Information == "")
        {
            return BadRequest(ErrorKeys.FillFormCorrectly);
        }

        List<Subject> differentSubjects = new List<Subject> { request.GoodSubject1, request.GoodSubject2, request.GoodSubject3, request.BadSubject1, request.BadSubject2, request.BadSubject3 };
        if (differentSubjects.Distinct().ToList().Count != 6)
        {
            return BadRequest(ErrorKeys.FillFormCorrectly);
        }

        if (request.Birthdate > DateTime.UtcNow || request.Birthdate.Year < 1900)
        {
            return BadRequest(ErrorKeys.FillFormCorrectly);
        }

        var usernameTaken = await _dataContext.Users.AnyAsync(usr => usr.Username == request.Username);
        if (usernameTaken)
        {
            return BadRequest(ErrorKeys.UsernameTaken);
        }

        var profileImageId =
            await _fileAnonymousFinder.GetFileId(_dataContext, request.ProfileImagePath);
        if (profileImageId == null)
        {
            return BadRequest(ErrorKeys.FileNotValid);
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Birthdate = request.Birthdate,
            Grade = request.Grade,
            ProfileImageId = (Guid)profileImageId,
            Information = request.Information,
            Language = request.Language,
            GoodSubject1 = request.GoodSubject1,
            GoodSubject2 = request.GoodSubject2,
            GoodSubject3 = request.GoodSubject3,
            BadSubject1 = request.BadSubject1,
            BadSubject2 = request.BadSubject2,
            BadSubject3 = request.BadSubject3,
            DarkTheme = request.DarkTheme
        };

        using var hmac = new HMACSHA512();
        user.PasswordSalt = hmac.Key;
        user.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password));

        var token = TokenAuthentication.CreateToken(user.Id, _configuration);

        user.RefreshToken = token.RefreshToken;
        user.RefreshExpires = token.RefreshExpires;

        await _dataContext.Users.AddAsync(user);
        await _dataContext.SaveChangesAsync();

        return Ok(token);
    }
}
