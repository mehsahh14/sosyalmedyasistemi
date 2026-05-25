using MySql.Data.MySqlClient;
using SosyalMedyaAPI.Data;
using SosyalMedyaAPI.Services.Interface;
using System.Data;

public class FollowService : IFollowService
{
    private readonly DbConnection _db;

    public FollowService(DbConnection db)
    {
        _db = db;
    }

    public async Task<(bool Success, string Message, IEnumerable<object> Data)> FollowAsync(int takipEdenId, int takipEdilenId)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();
        using var cmd = new MySqlCommand("get_takipet", conn) { CommandType = CommandType.StoredProcedure };

        cmd.Parameters.AddWithValue("@p_takip_eden_id", takipEdenId);
        cmd.Parameters.AddWithValue("@p_takip_edilen_id", takipEdilenId);

        var takipBilgileri = new List<object>();
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            takipBilgileri.Add(new
            {
                TakipEdilenId = reader.GetInt32("takip_edilen_id"),
                TakipTarihi = reader.GetDateTime("takip_tarihi")
            });
        }
        return (true, "Takip edildi", takipBilgileri);
    }

    public async Task<bool> UnfollowAsync(int takipEdenId, int takipEdilenId)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();
        using var cmd = new MySqlCommand("get_takibibirak", conn) { CommandType = CommandType.StoredProcedure };

        cmd.Parameters.AddWithValue("@p_takip_eden_id", takipEdenId);
        cmd.Parameters.AddWithValue("@p_takip_edilen_id", takipEdilenId);

        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    public async Task<IEnumerable<object>> GetFollowersAsync(int userId)
    {
        return await GetUserRelationList("get_takipciler", "@p_user_id", userId);
    }

    public async Task<IEnumerable<object>> GetFollowingAsync(int userId)
    {
        return await GetUserRelationList("get_takip_edilenler", "@p_user_id", userId);
    }

 
    private async Task<IEnumerable<object>> GetUserRelationList(string spName, string paramName, int userId)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();
        using var cmd = new MySqlCommand(spName, conn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.AddWithValue(paramName, userId);

        var liste = new List<object>();
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            liste.Add(new
            {
                Id = reader.GetInt32("id"),
                Username = reader.GetString("username"),
                FirstName = reader.GetString("firstName"),
                LastName = reader.GetString("lastName")
            });
        }
        return liste;
    }
}