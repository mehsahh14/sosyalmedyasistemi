using Microsoft.AspNetCore.Mvc;
using SosyalMedyaAPI.Services.Interface;
using SosyalMedyaAPI.Services.Interface; 
using static SosyalMedyaAPI.Controllers.ClubController; // DTO'ları kullanabilmek için

namespace SosyalMedyaAPI.Controllers
{
    [ApiController]
    [Route("api/club")]
    public class ClubController : ControllerBase
    {
        private readonly IClubService _clubService;

        public ClubController(IClubService clubService)
        {
            _clubService = clubService;
        }

        [HttpGet("events")]
        public async Task<IActionResult> GetEvents() => Ok(await _clubService.GetAllEventsAsync());

        [HttpGet("all")]
        public async Task<IActionResult> GetClubs() => Ok(await _clubService.GetAllClubsAsync());

        [HttpPost("join")]
        public async Task<IActionResult> JoinClub([FromBody] KulupKatilDto dto)
        {
            await _clubService.JoinClubAsync(dto);
            return Ok(new { message = "Kulübe başarıyla üye olundu! " });
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateClub([FromBody] KulupOlusturDto dto)
        {
            await _clubService.CreateClubAsync(dto);
            return Ok(new { message = "Kulüp başarıyla kuruldu! " });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClubDetails(int id)
        {
            var club = await _clubService.GetClubDetailsAsync(id);
            return club != null ? Ok(club) : NotFound(new { message = "Kulüp bulunamadı." });
        }

        [HttpGet("{id}/events")]
        public async Task<IActionResult> GetClubEvents(int id) => Ok(await _clubService.GetClubEventsAsync(id));

        [HttpGet("{id}/members")]
        public async Task<IActionResult> GetClubMembers(int id) => Ok(await _clubService.GetClubMembersAsync(id));

        [HttpPost("add-event")]
        public async Task<IActionResult> AddEvent([FromBody] EtkinlikEkleDto dto)
        {
            await _clubService.AddEventAsync(dto);
            return Ok(new { message = "Etkinlik başarıyla oluşturuldu! " });
        }

        // DTO yapıların burada kalmaya devam edebilir
        public record EtkinlikListelemeDto(int EtkinlikId, string EtkinlikAdi, string Detay, DateTime EtkinlikTarihi, string Yer, int KulupId, string KulupAdi);
        public record KulupKatilDto(int KulupId, int KullaniciId);
        public record KulupOlusturDto(string KulupAdi, string Aciklama, int OlusturanId);
        public record KulupDetayDto(int KulupId, string KulupAdi, string Aciklama, int OlusturanId, int UyeSayisi);
        public record KulupEtkinlikDto(int EtkinlikId, string EtkinlikAdi, string Detay, DateTime EtkinlikTarihi, string Yer);
        public record EtkinlikEkleDto(int KulupId, string EtkinlikAdi, string Detay, DateTime EtkinlikTarihi, string Yer);
    }
}
