using Microsoft.AspNetCore.Mvc;
using SosyalMedyaAPI.Services.Interface;

using static SosyalMedyaAPI.Controllers.PostController; 

namespace SosyalMedyaAPI.Controllers
{
    [ApiController]
    [Route("api/post")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

       
        public class CreatePostDto
        {
            public int KullaniciId { get; set; }
            public string? Aciklama { get; set; }
            public IFormFile? FotografDosyasi { get; set; }
        }

        public record GonderiListelemeDto(
            int GonderiId, string Fotograf, string Aciklama,
            int BegeniSayisi, int KullaniciId, string KullaniciAdi
        );

        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostDto dto)
        {
            var (success, message, imageUrl) = await _postService.CreatePostAsync(dto);

            return success
                ? Ok(new { message, imageUrl })
                : BadRequest(message);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllPosts()
            => Ok(await _postService.GetAllPostsAsync());

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserPosts(int userId)
            => Ok(await _postService.GetUserPostsAsync(userId));

        [HttpDelete("{gonderiId}/{kullaniciId}")]
        public async Task<IActionResult> DeletePost(int gonderiId, int kullaniciId)
        {
            var success = await _postService.DeletePostAsync(gonderiId, kullaniciId);
            return success
                ? Ok(new { message = "Gönderi başarıyla silindi." })
                : BadRequest(new { error = "Gönderi silinemedi." });
        }
    }
}