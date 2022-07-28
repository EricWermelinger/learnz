namespace Learnz.Entities;
public class LearnzFileAnonymous
{
    public Guid Id { get; set; }
    public string FileNameInternal { get; set; }
    public string FileNameExternal { get; set; }
    public string Path { get; set; }
    public DateTime Created { get; set; }
    
    public ICollection<User> ProfileImages { get; set; }
}
