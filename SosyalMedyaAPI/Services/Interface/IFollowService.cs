namespace SosyalMedyaAPI.Services.Interface
{
    public interface IFollowService
    {
        Task<(bool Success, string Message, IEnumerable<object> Data)> FollowAsync(int takipEdenId, int takipEdilenId);
        Task<bool> UnfollowAsync(int takipEdenId, int takipEdilenId);
        Task<IEnumerable<object>> GetFollowersAsync(int userId);
        Task<IEnumerable<object>> GetFollowingAsync(int userId);
    }
}
