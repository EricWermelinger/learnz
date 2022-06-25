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
            .HasOne(g => g.File)
            .WithMany(f => f.GroupImageFiles)
            .HasForeignKey(g => g.FileId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Group>()
            .HasOne(g => g.User)
            .WithMany(u => u.GroupUsers)
            .HasForeignKey(g => g.UserId)
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
            .WithMany(u => u.GroupMembers)
            .HasForeignKey(gm => gm.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LearnzFile>()
            .HasOne(f => f.CreatedBy)
            .WithMany(u => u.LearnzFileCreated)
            .HasForeignKey(f => f.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LearnzFile>()
            .HasOne(f => f.ModifiedBy)
            .WithMany(u => u.LearnzFileModified)
            .HasForeignKey(f => f.ModifiedById)
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
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<TogetherAsk> TogetherAsks { get; set; }
    public virtual DbSet<TogetherConnection> TogetherConnections { get; set; }
    public virtual DbSet<TogetherSwipe> TogetherSwipes { get; set; }
    public virtual DbSet<TogetherMessage> TogetherMessages { get; set; }
    public virtual DbSet<Group> Groups { get; set; }
    public virtual DbSet<GroupMember> GroupMembers { get; set; }
    public virtual DbSet<GroupFile> GroupFiles { get; set; }
    public virtual DbSet<LearnzFile> Files { get; set; }
}
