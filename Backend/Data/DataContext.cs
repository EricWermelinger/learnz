namespace Learnz.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        this.Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Group>()
            .HasOne(g => g.ProfileImage)
            .WithMany(f => f.GroupImageFiles)
            .HasForeignKey(g => g.ProfileImageId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Group>()
            .HasOne(g => g.Admin)
            .WithMany(u => u.GroupAdmin)
            .HasForeignKey(g => g.AdminId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GroupFile>()
            .HasOne(gf => gf.Group)
            .WithMany(g => g.GroupFiles)
            .HasForeignKey(gf => gf.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GroupFile>()
            .HasOne(gf => gf.File)
            .WithMany(f => f.GroupFiles)
            .HasForeignKey(gf => gf.FileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GroupMember>()
            .HasOne(gm => gm.Group)
            .WithMany(g => g.GroupMembers)
            .HasForeignKey(gm => gm.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GroupMember>()
            .HasOne(gm => gm.User)
            .WithMany(u => u.GroupUsers)
            .HasForeignKey(gm => gm.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GroupMessage>()
            .HasOne(gm => gm.Sender)
            .WithMany(u => u.GroupMessages)
            .HasForeignKey(gm => gm.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GroupMessage>()
            .HasOne(gm => gm.Group)
            .WithMany(g => g.GroupMessages)
            .HasForeignKey(gm => gm.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TogetherAsk>()
            .HasOne(ta => ta.InterestedUser)
            .WithMany(u => u.TogetherAskInterestedUsers)
            .HasForeignKey(ta => ta.InterestedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TogetherAsk>()
            .HasOne(ta => ta.AskedUser)
            .WithMany(u => u.TogetherAskAskedUsers)
            .HasForeignKey(ta => ta.AskedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TogetherConnection>()
            .HasOne(tc => tc.User1)
            .WithMany(u => u.TogetherConnectionUsers1)
            .HasForeignKey(tc => tc.UserId1)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TogetherConnection>()
            .HasOne(tc => tc.User2)
            .WithMany(u => u.TogetherConnectionUsers2)
            .HasForeignKey(tc => tc.UserId2)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TogetherMessage>()
            .HasOne(tm => tm.Sender)
            .WithMany(u => u.TogetherMessageSenders)
            .HasForeignKey(tm => tm.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TogetherMessage>()
            .HasOne(tm => tm.Receiver)
            .WithMany(u => u.TogetherMessageReceivers)
            .HasForeignKey(tm => tm.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TogetherSwipe>()
            .HasOne(ts => ts.SwiperUser)
            .WithMany(u => u.TogetherSwipeSwiperUsers)
            .HasForeignKey(ts => ts.SwiperUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TogetherSwipe>()
            .HasOne(ts => ts.AskedUser)
            .WithMany(u => u.TogetherSwipeAskedUsers)
            .HasForeignKey(ts => ts.AskedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasOne(u => u.ProfileImage)
            .WithMany(lfa => lfa.ProfileImages)
            .HasForeignKey(u => u.ProfileImageId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CreateSet>()
            .HasOne(cs => cs.CreatedBy)
            .WithMany(u => u.CreateSetCreated)
            .HasForeignKey(cs => cs.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CreateSet>()
            .HasOne(cs => cs.ModifiedBy)
            .WithMany(u => u.CreateSetModified)
            .HasForeignKey(cs => cs.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CreateQuestionDistribute>()
            .HasOne(cqd => cqd.Set)
            .WithMany(cs => cs.QuestionDistributes)
            .HasForeignKey(cqd => cqd.SetId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CreateQuestionDistributeAnswer>()
            .HasOne(cqa => cqa.QuestionDistribute)
            .WithMany(cqd => cqd.Answers)
            .HasForeignKey(cqa => cqa.QuestionDistributeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CreateQuestionMathematic>()
            .HasOne(cqd => cqd.Set)
            .WithMany(cs => cs.QuestionMathematics)
            .HasForeignKey(cqd => cqd.SetId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CreateQuestionMathematicVariable>()
            .HasOne(qmv => qmv.QuestionMathematic)
            .WithMany(cqm => cqm.Variables)
            .HasForeignKey(qmv => qmv.QuestionMathematicId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CreateQuestionMultipleChoice>()
            .HasOne(cqd => cqd.Set)
            .WithMany(cs => cs.QuestionMultipleChoices)
            .HasForeignKey(cqd => cqd.SetId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CreateQuestionMultipleChoiceAnswer>()
            .HasOne(qma => qma.QuestionMultipleChoice)
            .WithMany(cqm => cqm.Answers)
            .HasForeignKey(qma => qma.QuestionMultipleChoiceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CreateQuestionOpenQuestion>()
            .HasOne(cqd => cqd.Set)
            .WithMany(cs => cs.QuestionOpenQuestions)
            .HasForeignKey(cqd => cqd.SetId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CreateQuestionTextField>()
            .HasOne(cqd => cqd.Set)
            .WithMany(cs => cs.QuestionTextFields)
            .HasForeignKey(cqd => cqd.SetId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CreateQuestionTrueFalse>()
            .HasOne(cqd => cqd.Set)
            .WithMany(cs => cs.QuestionTrueFalses)
            .HasForeignKey(cqd => cqd.SetId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CreateQuestionWord>()
            .HasOne(cqd => cqd.Set)
            .WithMany(cs => cs.QuestionWords)
            .HasForeignKey(cqd => cqd.SetId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LearnzFileVersion>()
            .HasOne(lfv => lfv.File)
            .WithMany(fil => fil.LearnzFileVersions)
            .HasForeignKey(lfv => lfv.FileId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LearnzFile>()
            .HasOne(f => f.Owner)
            .WithMany(u => u.LearnzFileOwner)
            .HasForeignKey(f => f.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Challenge>()
           .HasOne(c => c.CreateSet)
           .WithMany(s => s.Challenges)
           .HasForeignKey(c => c.CreateSetId)
           .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Challenge>()
            .HasOne(c => c.Owner)
            .WithMany(u => u.ChallengeOwners)
            .HasForeignKey(c => c.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ChallengeQuestionAnswer>()
            .HasOne(cqa => cqa.Challenge)
            .WithMany(c => c.ChallengeQuestionAnswers)
            .HasForeignKey(cqa => cqa.ChallengeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ChallengeQuestionAnswer>()
            .HasOne(cqa => cqa.User)
            .WithMany(u => u.ChallengeQuestionAnswers)
            .HasForeignKey(cqa => cqa.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ChallengeQuestionAnswer>()
            .HasOne(cqa => cqa.ChallengeQuestionPosed)
            .WithMany(cqp => cqp.ChallengeQuestionAnswers)
            .HasForeignKey(cqa => cqa.ChallengeQuestionPosedId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ChallengeUser>()
            .HasOne(cu => cu.Challenge)
            .WithMany(c => c.ChallengeUsers)
            .HasForeignKey(cu => cu.ChallengeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ChallengeUser>()
            .HasOne(cu => cu.User)
            .WithMany(u => u.ChallengeUsers)
            .HasForeignKey(cu => cu.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ChallengeQuestionMathematicResolved>()
            .HasOne(cqmr => cqmr.Challenge)
            .WithMany(c => c.ChallengeQuestionMathematicResolveds)
            .HasForeignKey(cqmr => cqmr.ChallengeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ChallengeQuestionPosed>()
            .HasOne(cqp => cqp.Challenge)
            .WithMany(c => c.ChallengeQuestionsPosed)
            .HasForeignKey(cqp => cqp.ChallengeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LearnSession>()
            .HasOne(lss => lss.Set)
            .WithMany(crs => crs.LearnSessions)
            .HasForeignKey(lss => lss.SetId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LearnSession>()
            .HasOne(lss => lss.User)
            .WithMany(usr => usr.LearnSessions)
            .HasForeignKey(lss => lss.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LearnQuestion>()
            .HasOne(lqs => lqs.LearnSession)
            .WithMany(lss => lss.Questions)
            .HasForeignKey(lqs => lqs.LearnSessionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Test>()
            .HasOne(tst => tst.Set)
            .WithMany(crs => crs.Tests)
            .HasForeignKey(tst => tst.SetId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TestGroup>()
            .HasOne(tsg => tsg.Test)
            .WithMany(tst => tst.TestGroups)
            .HasForeignKey(tsg => tsg.TestId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TestGroup>()
            .HasOne(tsg => tsg.Group)
            .WithMany(grp => grp.TestGroups)
            .HasForeignKey(tsg => tsg.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TestQuestion>()
            .HasOne(tsq => tsq.Test)
            .WithMany(tst => tst.TestQuestions)
            .HasForeignKey(tsq => tsq.TestId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TestOfUser>()
            .HasOne(tou => tou.Test)
            .WithMany(tst => tst.TestOfUsers)
            .HasForeignKey(tou => tou.TestId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TestOfUser>()
            .HasOne(tou => tou.User)
            .WithMany(usr => usr.TestOfUsers)
            .HasForeignKey(tou => tou.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TestQuestionOfUser>()
            .HasOne(tqu => tqu.TestOfUser)
            .WithMany(tou => tou.TestQuestionOfUsers)
            .HasForeignKey(tqu => tqu.TestOfUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public virtual DbSet<User> Users { get; set; }
    
    public virtual DbSet<TogetherAsk> TogetherAsks { get; set; }
    public virtual DbSet<TogetherConnection> TogetherConnections { get; set; }
    public virtual DbSet<TogetherSwipe> TogetherSwipes { get; set; }
    public virtual DbSet<TogetherMessage> TogetherMessages { get; set; }
    
    public virtual DbSet<Group> Groups { get; set; }
    public virtual DbSet<GroupMember> GroupMembers { get; set; }
    public virtual DbSet<GroupFile> GroupFiles { get; set; }
    public virtual DbSet<GroupMessage> GroupMessages { get; set; }
    
    public virtual DbSet<LearnzFile> Files { get; set; }
    public virtual DbSet<LearnzFileAnonymous> FilesAnonymous { get; set; }
    public virtual DbSet<LearnzFileVersion> FileVersions { get; set; }
    
    public virtual DbSet<CreateSet> CreateSets { get; set; }
    public virtual DbSet<CreateQuestionDistribute> CreateQuestionDistributes { get; set; }
    public virtual DbSet<CreateQuestionDistributeAnswer> CreateQuestionDistributeAnswers { get; set; }
    public virtual DbSet<CreateQuestionMathematic> CreateQuestionMathematics { get; set; }
    public virtual DbSet<CreateQuestionMathematicVariable> CreateQuestionMathematicVariables { get; set; }
    public virtual DbSet<CreateQuestionMultipleChoice> CreateQuestionMultipleChoices { get; set; }
    public virtual DbSet<CreateQuestionMultipleChoiceAnswer> CreateQuestionMultipleChoiceAnswers { get; set; }
    public virtual DbSet<CreateQuestionOpenQuestion> CreateQuestionOpenQuestions { get; set; }
    public virtual DbSet<CreateQuestionTextField> CreateQuestionTextFields { get; set; }
    public virtual DbSet<CreateQuestionTrueFalse> CreateQuestionTrueFalses { get; set; }
    public virtual DbSet<CreateQuestionWord> CreateQuestionWords { get; set; }

    public virtual DbSet<Challenge> Challenges { get; set; }
    public virtual DbSet<ChallengeQuestionAnswer> ChallengeQuestionAnswers { get; set; }
    public virtual DbSet<ChallengeUser> ChallengeUsers { get; set; }
    public virtual DbSet<ChallengeQuestionMathematicResolved> ChallengeQuestionsMathematicResolved { get; set; }
    public virtual DbSet<ChallengeQuestionPosed> ChallengeQuestiosnPosed { get; set; }

    public virtual DbSet<LearnSession> LearnSessions { get; set; }
    public virtual DbSet<LearnQuestion> LearnQuestions { get; set; }

    public virtual DbSet<Test> Tests { get; set; }
    public virtual DbSet<TestGroup> TestGroups { get; set; }
    public virtual DbSet<TestQuestion> TestQuestions { get; set; }
    public virtual DbSet<TestQuestionOfUser> TestQuestionUsers { get; set; }
    public virtual DbSet<TestOfUser> TestUsers { get; set; }
}
