using static SosyalMedyaAPI.Controllers.ClubController;

namespace SosyalMedyaAPI.Services.Interface
{
    public interface IClubService
    {
        Task<IEnumerable<EtkinlikListelemeDto>> GetAllEventsAsync();
        Task<IEnumerable<object>> GetAllClubsAsync();
        Task<bool> JoinClubAsync(KulupKatilDto dto);
        Task<bool> CreateClubAsync(KulupOlusturDto dto);
        Task<KulupDetayDto?> GetClubDetailsAsync(int id);
        Task<IEnumerable<KulupEtkinlikDto>> GetClubEventsAsync(int id);
        Task<IEnumerable<object>> GetClubMembersAsync(int id);
        Task<bool> AddEventAsync(EtkinlikEkleDto dto);
    }
}
