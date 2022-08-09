namespace Learnz.DTOs;
public class TogetherChatDTO
{
    public List<TogetherChatMessageDTO> Messages { get; set; }
    public TogetherUserProfileDTO User { get; set; }
}
