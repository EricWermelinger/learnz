namespace Learnz.DTOs;
public class GroupMessageGetDTO
{
    public string Message { get; set; }
    public string UserName { get; set; }
    public DateTime Date { get; set; }
    public bool SentByMe { get; set; }
    public bool IsInfoMessage { get; set; }
}
