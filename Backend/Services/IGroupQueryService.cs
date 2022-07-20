
namespace Learnz.Services
{
    public interface IGroupQueryService
    {
        Task<List<FileFrontendDTO>> GetFiles(Guid userId, Guid groupId);
        Task<List<GroupOverviewDTO>> GetGroupOverview(Guid userId);
        Task<GroupMessageChatDTO> GetMessages(Guid userId, Guid groupId);
    }
}