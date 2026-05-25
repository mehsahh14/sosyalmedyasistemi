using MySql.Data.MySqlClient;
using SosyalMedyaAPI.Data;
using System.Data;
using SosyalMedyaAPI.Controllers;
using SosyalMedyaAPI.Services.Interface;

public class ClubService : IClubService
{
    private readonly DbConnection _db;
    public ClubService(DbConnection db) => _db = db;

    public async Task<IEnumerable<ClubController.EtkinlikListelemeDto>> GetAllEventsAsync()
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();
        using var cmd = new MySqlCommand("get_etkinlikler", conn) { CommandType = CommandType.StoredProcedure };

        var list = new List<ClubController.EtkinlikListelemeDto>();
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new ClubController.EtkinlikListelemeDto(
                reader.GetInt32("etkinlikId"), reader.GetString("etkinlikAdi"),
                reader.GetString("detay"), reader.GetDateTime("etkinlikTarihi"),
                reader.GetString("yer"), reader.GetInt32("kulupId"), reader.GetString("kulupAdi")
            ));
        }
        return list;
    }

    public async Task<IEnumerable<object>> GetAllClubsAsync()
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();
        using var cmd = new MySqlCommand("get_kulupler", conn) { CommandType = CommandType.StoredProcedure };

        var list = new List<object>();
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new
            {
                kulupId = reader.GetInt32("kulupId"),
                kulupAdi = reader.GetString("kulupAdi"),
                aciklama = reader.IsDBNull("aciklama") ? "" : reader.GetString("aciklama")
            });
        }
        return list;
    }

    public async Task<bool> JoinClubAsync(ClubController.KulupKatilDto dto)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();
        using var cmd = new MySqlCommand("get_kulup_katil", conn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.AddWithValue("@p_kulup_id", dto.KulupId);
        cmd.Parameters.AddWithValue("@p_kullanici_id", dto.KullaniciId);

        await cmd.ExecuteNonQueryAsync();
        return true;
    }

    public async Task<bool> CreateClubAsync(ClubController.KulupOlusturDto dto)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();
        using var cmd = new MySqlCommand("get_kulupolustur", conn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.AddWithValue("@p_kulup_adi", dto.KulupAdi);
        cmd.Parameters.AddWithValue("@p_aciklama", dto.Aciklama);
        cmd.Parameters.AddWithValue("@p_olusturan_id", dto.OlusturanId);

        await cmd.ExecuteNonQueryAsync();
        return true;
    }

    public async Task<ClubController.KulupDetayDto?> GetClubDetailsAsync(int id)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();
        using var cmd = new MySqlCommand("get_kulup_detay", conn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.AddWithValue("@p_kulup_id", id);

        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new ClubController.KulupDetayDto(
                reader.GetInt32("kulupId"), reader.GetString("kulupAdi"),
                reader.IsDBNull("aciklama") ? "" : reader.GetString("aciklama"),
                reader.GetInt32("olusturanId"), reader.GetInt32("uyeSayisi")
            );
        }
        return null;
    }

    public async Task<IEnumerable<ClubController.KulupEtkinlikDto>> GetClubEventsAsync(int id)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();
        using var cmd = new MySqlCommand("get_kulup_etkinlikleri", conn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.AddWithValue("@p_kulup_id", id);

        var list = new List<ClubController.KulupEtkinlikDto>();
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new ClubController.KulupEtkinlikDto(
                reader.GetInt32("etkinlikId"), reader.GetString("etkinlikAdi"),
                reader.GetString("detay"), reader.GetDateTime("etkinlikTarihi"), reader.GetString("yer")
            ));
        }
        return list;
    }

    public async Task<IEnumerable<object>> GetClubMembersAsync(int id)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();
        using var cmd = new MySqlCommand("get_kulup_uyeleri", conn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.AddWithValue("@p_kulup_id", id);

        var list = new List<object>();
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new
            {
                id = reader.GetInt32("id"),
                username = reader.GetString("username"),
                firstName = reader.GetString("firstName"),
                lastName = reader.GetString("lastName")
            });
        }
        return list;
    }

    public async Task<bool> AddEventAsync(ClubController.EtkinlikEkleDto dto)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();
        using var cmd = new MySqlCommand("get_etkinlikekle", conn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.AddWithValue("@p_kulup_id", dto.KulupId);
        cmd.Parameters.AddWithValue("@p_etkinlik_adi", dto.EtkinlikAdi);
        cmd.Parameters.AddWithValue("@p_detay", dto.Detay);
        cmd.Parameters.AddWithValue("@p_etkinlik_tarihi", dto.EtkinlikTarihi);
        cmd.Parameters.AddWithValue("@p_yer", dto.Yer);

        await cmd.ExecuteNonQueryAsync();
        return true;
    }
}