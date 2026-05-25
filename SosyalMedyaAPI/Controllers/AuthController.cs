using Microsoft.AspNetCore.Mvc;
using SosyalMedyaAPI.Models;
using SosyalMedyaAPI.Services.Interface;
using SosyalMedyaAPI.Services.Interface; // Arayüzü buraya dahil ediyoruz

namespace SosyalMedyaAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        // Artık veritabanı bağlantısı yerine servisi enjekte ediyoruz
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Kullanici model)
        {
            // Servis katmanından gelen sonucu kullanıyoruz
            var (success, message) = await _authService.RegisterAsync(model);

            if (!success) return BadRequest(new { message });

            return Ok(new { message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // Servis katmanından gelen başarı durumunu ve veriyi alıyoruz
            var (success, message, data) = await _authService.LoginAsync(model);

            if (!success) return BadRequest(new { message });

            return Ok(data);
        }
    }
}