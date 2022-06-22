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
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<TogetherAsk> TogetherAsk { get; set; }
    public virtual DbSet<TogetherConnection> TogetherConnections { get; set; }
    public virtual DbSet<TogetherSwipe> TogetherSwipes { get; set; }
    public virtual DbSet<TogetherMessage> TogetherMessages { get; set; }
    public virtual DbSet<Group> Groups { get; set; }
    public virtual DbSet<GroupMember> GroupMembers { get; set; }
    public virtual DbSet<GroupFile> GroupFiles { get; set; }
    public virtual DbSet<LearnzFile> LearnzFiles { get; set; }
}
