using static SosyalMedyaAPI.Controllers.PostController;

namespace SosyalMedyaAPI.Services.Interface
{
    public interface IPostService
    {
        Task<(bool Success, string Message, string? ImageUrl)> CreatePostAsync(CreatePostDto dto);
        Task<IEnumerable<GonderiListelemeDto>> GetAllPostsAsync();
        Task<IEnumerable<GonderiListelemeDto>> GetUserPostsAsync(int userId);
        Task<bool> DeletePostAsync(int gonderiId, int kullaniciId);
    }
}
