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
            .OnDelete(DeleteBehavior.Restrict);

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
}
