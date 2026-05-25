using Microsoft.AspNetCore.Mvc;
using SosyalMedyaAPI.Services.Interface;


namespace SosyalMedyaAPI.Controllers
{
    [ApiController]
    [Route("api/like")]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> LikeAdd(int kullaniciId, int gonderiId)
        {
            var success = await _likeService.LikeAddAsync(kullaniciId, gonderiId);

            return success
                ? Ok(new { message = "Beğeni eklendi" })
                : BadRequest(new { error = "Beğeni eklenirken bir hata oluştu." });
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> LikeRemove(int kullaniciId, int gonderiId)
        {
            var success = await _likeService.LikeRemoveAsync(kullaniciId, gonderiId);

            return success
                ? Ok(new { message = "Beğeni kaldırıldı" })
                : BadRequest(new { error = "Beğeni kaldırılırken bir hata oluştu." });
        }
    }
}