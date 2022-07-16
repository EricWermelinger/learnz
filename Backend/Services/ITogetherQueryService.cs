
namespace Learnz.Framework
{
    public interface ITogetherQueryService
    {
        Task<List<TogetherOverviewUserProfileDTO>> GetConnectionOverview(Guid guid);
        Task<TogetherChatDTO> GetMessages(Guid chatFrom, Guid chatTo);
        Task<TogetherAskOverviewDTO> GetOpenAsks(Guid guid);
    }
}