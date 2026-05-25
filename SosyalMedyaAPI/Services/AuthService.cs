using MySql.Data.MySqlClient;
using SosyalMedyaAPI.Data;
using SosyalMedyaAPI.Models;
using SosyalMedyaAPI.Services.Interface;
using System.Data;

namespace SosyalMedyaAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly DbConnection _db;
        public AuthService(DbConnection db) => _db = db;

        public async Task<(bool Success, string Message)> RegisterAsync(Kullanici model)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Sifre);

            using var cmd = new MySqlCommand("get_insertKullanici", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@p_kullanicitakmaadi", model.KullaniciTakmaAdi);
            cmd.Parameters.AddWithValue("@p_ad", model.Ad);
            cmd.Parameters.AddWithValue("@p_soyad", model.Soyad);
            cmd.Parameters.AddWithValue("@p_email", model.Email);
            cmd.Parameters.AddWithValue("@p_sifre", hashedPassword);

            await cmd.ExecuteNonQueryAsync();
            return (true, "Kayıt başarılı");
        }

        public async Task<(bool Success, string Message, object Data)> LoginAsync(LoginModel model)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new MySqlCommand("get_girisyap", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@p_email", model.Email);

            using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return (false, "Email bulunamadı", null!);

            string dbHash = reader["sifre"].ToString()!;
            if (!BCrypt.Net.BCrypt.Verify(model.Sifre, dbHash)) return (false, "Şifre hatalı", null!);

            return (true, "Giriş başarılı", new
            {
                id = reader["kullanici_id"],
                kullaniciAdi = reader["kullanicitakmaadi"],
                email = reader["email"]
            });
        }
    }
}
