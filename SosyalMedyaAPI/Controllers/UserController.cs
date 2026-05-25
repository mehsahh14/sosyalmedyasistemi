using Microsoft.AspNetCore.Mvc;
using SosyalMedyaAPI.Services.Interface;

using static SosyalMedyaAPI.Controllers.UserController; // DTO'yu kullanabilmek için

namespace SosyalMedyaAPI.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // DTO yapıların burada kalmaya devam edebilir
        public record UpdateProfileDto(string KullaniciTakmaAdi, string Ad, string Soyad);

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfile([FromRoute] int id)
        {
            var profile = await _userService.GetProfileAsync(id);
            return profile != null
                ? Ok(profile)
                : NotFound(new { message = "Kullanıcı bulunamadı." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile([FromRoute] int id, [FromBody] UpdateProfileDto dto)
        {
            var success = await _userService.UpdateProfileAsync(id, dto);
            return success
                ? Ok(new { message = "Profil başarıyla güncellendi." })
                : BadRequest(new { error = "Profil güncellenemedi." });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string query)
        {
            if (string.IsNullOrEmpty(query)) return BadRequest("Arama kelimesi boş olamaz.");

            var users = await _userService.SearchUsersAsync(query);
            return Ok(new
            {
                message = $"'{query}' için arama sonuçları listelendi.",
                data = users
            });
        }
    }
}