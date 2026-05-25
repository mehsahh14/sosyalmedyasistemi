using SosyalMedyaAPI.Models;

namespace SosyalMedyaAPI.Services.Interface
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> RegisterAsync(Kullanici model);
        Task<(bool Success, string Message, object Data)> LoginAsync(LoginModel model);
    }
}
