namespace SosyalMedyaAPI.Services.Interface
{
    public interface ILikeService
    {
        Task<bool> LikeAddAsync(int kullaniciId, int gonderiId);
        Task<bool> LikeRemoveAsync(int kullaniciId, int gonderiId);
    }
}
