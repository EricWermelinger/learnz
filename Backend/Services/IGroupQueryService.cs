namespace Learnz.Services;
public interface IGroupQueryService
{
    Task<List<FileInfoDTO>> GetFiles(Guid userId, Guid groupId);
    Task<List<GroupOverviewDTO>> GetGroupOverview(Guid userId);
    Task<List<GroupMessageGetDTO>> GetMessages(Guid userId, Guid groupId);
}