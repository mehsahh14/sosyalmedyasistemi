using MySql.Data.MySqlClient;
using SosyalMedyaAPI.Data;
using System.Data;
using SosyalMedyaAPI.Controllers;
using SosyalMedyaAPI.Services.Interface;

public class NotificationService : INotificationService
{
    private readonly DbConnection _db;

    public NotificationService(DbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<NotificationController.BildirimListelemeDto>> GetNotificationsAsync(int aliciId)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        using var cmd = new MySqlCommand("get_bildirimler", conn)
        {
            CommandType = CommandType.StoredProcedure
        };
        cmd.Parameters.AddWithValue("@p_alici_id", aliciId);

        var list = new List<NotificationController.BildirimListelemeDto>();
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(new NotificationController.BildirimListelemeDto(
                reader.GetInt32("bildirimId"),
                reader.GetInt32("tetikleyenId"),
                reader.GetString("tetikleyenKullaniciAdi"),
                reader.IsDBNull("gonderiId") ? null : reader.GetInt32("gonderiId"),
                reader.GetString("bildirimTuru"),
                reader.IsDBNull("icerik") ? "" : reader.GetString("icerik"),
                reader.GetBoolean("isRead"),
                reader.GetDateTime("olusturulmaTarihi")
            ));
        }
        return list;
    }

    public async Task<bool> MarkAsReadAsync(int bildirimId, int aliciId)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        using var cmd = new MySqlCommand("get_bildirimokundu", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@p_bildirim_id", bildirimId);
        cmd.Parameters.AddWithValue("@p_alici_id", aliciId);

        int result = await cmd.ExecuteNonQueryAsync();
        return result > 0;
    }
}