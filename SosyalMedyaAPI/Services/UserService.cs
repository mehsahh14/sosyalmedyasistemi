using MySql.Data.MySqlClient;
using SosyalMedyaAPI.Data;
using System.Data;
using SosyalMedyaAPI.Controllers;
using SosyalMedyaAPI.Services.Interface;

public class UserService : IUserService
{
    private readonly DbConnection _db;

    public UserService(DbConnection db) => _db = db;

    public async Task<object?> GetProfileAsync(int id)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();
        using var cmd = new MySqlCommand("get_kullanici_detay", conn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.AddWithValue("@p_user_id", id);

        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new
            {
                Id = reader.GetInt32("id"),
                KullaniciAdi = reader.GetString("username"),
                Ad = reader.GetString("first_name"),
                Soyad = reader.GetString("last_name"),
                Email = reader.GetString("email"),
                TakipEdilenSayisi = reader.GetInt32("takip_edilen_sayisi"),
                TakipciSayisi = reader.GetInt32("takipci_sayisi")
            };
        }
        return null;
    }

    public async Task<bool> UpdateProfileAsync(int id, UserController.UpdateProfileDto dto)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();
        using var cmd = new MySqlCommand("update_kullanici_profil", conn) { CommandType = CommandType.StoredProcedure };

        cmd.Parameters.AddWithValue("@p_user_id", id);
        cmd.Parameters.AddWithValue("@p_username", dto.KullaniciTakmaAdi);
        cmd.Parameters.AddWithValue("@p_first_name", dto.Ad);
        cmd.Parameters.AddWithValue("@p_last_name", dto.Soyad);

        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    public async Task<IEnumerable<object>> SearchUsersAsync(string query)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();
        using var cmd = new MySqlCommand("kullanici_ara", conn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.AddWithValue("@p_search_query", query);

        var list = new List<object>();
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new
            {
                Id = reader.GetInt32("kullanici_id"),
                KullaniciAdi = reader.GetString("kullanicitakmaadi"),
                Ad = reader.GetString("ad"),
                Soyad = reader.GetString("soyad")
            });
        }
        return list;
    }
}