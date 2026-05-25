using static SosyalMedyaAPI.Controllers.NotificationController;

namespace SosyalMedyaAPI.Services.Interface
{
    public interface INotificationService
    {
        Task<IEnumerable<BildirimListelemeDto>> GetNotificationsAsync(int aliciId);
        Task<bool> MarkAsReadAsync(int bildirimId, int aliciId);
    }
}
