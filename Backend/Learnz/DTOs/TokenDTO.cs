﻿namespace Learnz.DTOs;

public class TokenDTO
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshExpires { get; set; }
}