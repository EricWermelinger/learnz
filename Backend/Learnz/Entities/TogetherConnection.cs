﻿namespace Learnz.Entities;

public class TogetherConnection
{
    public Guid Id { get; set; }
    public Guid UserId1 { get; set; }
    public User User1 { get; set; }
    public Guid UserId2 { get; set; }
    public User User2 { get; set; }
}
