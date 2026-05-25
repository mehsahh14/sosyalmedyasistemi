using Microsoft.AspNetCore.Mvc;
using SosyalMedyaAPI.Services.Interface;

using static SosyalMedyaAPI.Controllers.NotificationController; // DTO'yu kullanabilmek için

namespace SosyalMedyaAPI.Controllers
{
    [ApiController]
    [Route("api/notification")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public record BildirimListelemeDto(
            int BildirimId, int TetikleyenId, string TetikleyenKullaniciAdi,
            int? GonderiId, string BildirimTuru, string Icerik,
            bool IsRead, DateTime OlusturulmaTarihi
        );

        [HttpGet("{aliciId}")]
        public async Task<IActionResult> GetNotifications([FromRoute] int aliciId)
        {
            var notifications = await _notificationService.GetNotificationsAsync(aliciId);
            return Ok(notifications);
        }

        [HttpPut("read/{bildirimId}/{aliciId}")]
        public async Task<IActionResult> MarkAsRead([FromRoute] int bildirimId, [FromRoute] int aliciId)
        {
            var success = await _notificationService.MarkAsReadAsync(bildirimId, aliciId);
            return success
                ? Ok(new { message = "Bildirim okundu olarak işaretlendi." })
                : BadRequest(new { error = "Bildirim güncellenemedi." });
        }
    }
}