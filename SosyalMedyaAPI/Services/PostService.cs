using MySql.Data.MySqlClient;
using SosyalMedyaAPI.Data;
using System.Data;
using SosyalMedyaAPI.Controllers;
using SosyalMedyaAPI.Services.Interface;

public class PostService : IPostService
{
    private readonly DbConnection _db;
    private readonly IWebHostEnvironment _env;

    public PostService(DbConnection db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    public async Task<(bool Success, string Message, string? ImageUrl)> CreatePostAsync(PostController.CreatePostDto dto)
    {
        string imageUrl = "";

        if (dto.FotografDosyasi != null && dto.FotografDosyasi.Length > 0)
        {
            string[] allowedExt = { ".jpg", ".jpeg", ".png" };
            string ext = Path.GetExtension(dto.FotografDosyasi.FileName).ToLower();
            if (!allowedExt.Contains(ext)) return (false, "Sadece jpg, jpeg, png yükleyebilirsin.", null);

            string fileName = Guid.NewGuid() + ext;
            string path = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            using (var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                await dto.FotografDosyasi.CopyToAsync(stream);
            }
            imageUrl = $"/uploads/{fileName}";
        }

        using var conn = _db.GetConnection();
        await conn.OpenAsync();
        using var cmd = new MySqlCommand("get_gonderiekle", conn) { CommandType = CommandType.StoredProcedure };

        cmd.Parameters.AddWithValue("@p_kullanici_id", dto.KullaniciId);
        cmd.Parameters.AddWithValue("@p_fotograf", imageUrl);
        cmd.Parameters.AddWithValue("@p_aciklama", dto.Aciklama ?? "");

        await cmd.ExecuteNonQueryAsync();
        return (true, "Gönderi başarıyla oluşturuldu.", imageUrl);
    }

    public async Task<IEnumerable<PostController.GonderiListelemeDto>> GetAllPostsAsync()
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();
        using var cmd = new MySqlCommand("get_gonderiler", conn) { CommandType = CommandType.StoredProcedure };

        var list = new List<PostController.GonderiListelemeDto>();
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new PostController.GonderiListelemeDto(
                reader.GetInt32("gonderiId"),
                reader.GetString("fotograf"),
                reader.IsDBNull("aciklama") ? "" : reader.GetString("aciklama"),
                reader.GetInt32("begeniSayisi"),
                reader.GetInt32("kullaniciId"),
                reader.GetString("kullaniciAdi")
            ));
        }
        return list;
    }

    public async Task<IEnumerable<PostController.GonderiListelemeDto>> GetUserPostsAsync(int userId)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();
        string query = @"SELECT g.gonderi_id AS gonderiId, g.fotograf, g.aciklama, 
                         COALESCE(COUNT(b.begeni_id), 0) AS begeniSayisi, 
                         g.kullanici_id AS kullaniciId, k.kullanicitakmaadi AS kullaniciAdi
                         FROM gonderiler g
                         INNER JOIN kullanicilar k ON g.kullanici_id = k.kullanici_id
                         LEFT JOIN begeniler b ON g.gonderi_id = b.gonderi_id
                         WHERE g.kullanici_id = @userId GROUP BY g.gonderi_id ORDER BY g.gonderi_id DESC";

        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@userId", userId);

        var list = new List<PostController.GonderiListelemeDto>();
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new PostController.GonderiListelemeDto(
                reader.GetInt32("gonderiId"), reader.GetString("fotograf"),
                reader.IsDBNull("aciklama") ? "" : reader.GetString("aciklama"),
                reader.GetInt32("begeniSayisi"), reader.GetInt32("kullaniciId"), reader.GetString("kullaniciAdi")
            ));
        }
        return list;
    }

    public async Task<bool> DeletePostAsync(int gonderiId, int kullaniciId)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();
        using var cmd = new MySqlCommand("get_gonderisil", conn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.AddWithValue("@p_gonderi_id", gonderiId);
        cmd.Parameters.AddWithValue("@p_kullanici_id", kullaniciId);

        return await cmd.ExecuteNonQueryAsync() > 0;
    }
}