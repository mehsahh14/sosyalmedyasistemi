using MySql.Data.MySqlClient;
using SosyalMedyaAPI.Data;
using SosyalMedyaAPI.Services.Interface;
using System.Data;

public class CommentService : ICommentService
{
    private readonly DbConnection _db;

    public CommentService(DbConnection db)
    {
        _db = db;
    }

    public async Task<bool> AddCommentAsync(int kullaniciId, int gonderiId, string yorum)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        using var cmd = new MySqlCommand("get_yorumyap", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@p_kullanici_id", kullaniciId);
        cmd.Parameters.AddWithValue("@p_gonderi_id", gonderiId);
        cmd.Parameters.AddWithValue("@p_yorum", yorum);

        int result = await cmd.ExecuteNonQueryAsync();
        return result > 0;
    }

    public async Task<IEnumerable<object>> GetCommentsAsync(int gonderiId)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        using var cmd = new MySqlCommand("get_yorumlar", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@p_gonderi_id", gonderiId);

        var yorumListesi = new List<object>();
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            yorumListesi.Add(new
            {
                YorumId = reader.GetInt32("yorumId"),
                YorumMetni = reader.GetString("yorumMetni"),
                KullaniciAdi = reader.GetString("kullaniciAdi")
            });
        }

        return yorumListesi;
    }
}