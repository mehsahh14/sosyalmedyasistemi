using MySql.Data.MySqlClient;
using SosyalMedyaAPI.Data;
using SosyalMedyaAPI.Services.Interface;
using System.Data;

public class LikeService : ILikeService
{
    private readonly DbConnection _db;

    public LikeService(DbConnection db)
    {
        _db = db;
    }

    public async Task<bool> LikeAddAsync(int kullaniciId, int gonderiId)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        using var cmd = new MySqlCommand("get_begeniekle", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@p_kullanici_id", kullaniciId);
        cmd.Parameters.AddWithValue("@p_gonderi_id", gonderiId);

        int result = await cmd.ExecuteNonQueryAsync();
        return result > 0;
    }

    public async Task<bool> LikeRemoveAsync(int kullaniciId, int gonderiId)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        using var cmd = new MySqlCommand("get_begenikaldir", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@p_kullanici_id", kullaniciId);
        cmd.Parameters.AddWithValue("@p_gonderi_id", gonderiId);

        int result = await cmd.ExecuteNonQueryAsync();
        return result > 0;
    }
}