using static SosyalMedyaAPI.Controllers.UserController;

namespace SosyalMedyaAPI.Services.Interface
{
    public interface IUserService
    {
        Task<object?> GetProfileAsync(int id);
        Task<bool> UpdateProfileAsync(int id, UpdateProfileDto dto);
        Task<IEnumerable<object>> SearchUsersAsync(string query);
    }
}
