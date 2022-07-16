
namespace Learnz.Framework
{
    public interface ITogetherQueryService
    {
        Task<List<TogetherOverviewUserProfileDTO>> GetConnectionOverview(Guid guid);
        Task<List<TogetherChatMessageDTO>> GetMessages(Guid chatFrom, Guid chatTo);
        Task<TogetherAskOverviewDTO> GetOpenAsks(Guid guid);
    }
}