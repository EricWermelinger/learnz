namespace Learnz.Entities;
public class LearnzFile
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string MimeType { get; set; }
    public byte[] Data { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
}
