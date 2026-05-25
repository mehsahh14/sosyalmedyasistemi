namespace SosyalMedyaAPI.Services.Interface
{
    public interface ICommentService
    {
        Task<bool> AddCommentAsync(int kullaniciId, int gonderiId, string yorum);
        Task<IEnumerable<object>> GetCommentsAsync(int gonderiId);
    }
}
