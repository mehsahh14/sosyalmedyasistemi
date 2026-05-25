using Microsoft.AspNetCore.Mvc;
using SosyalMedyaAPI.Services.Interface;


namespace SosyalMedyaAPI.Controllers
{
    [ApiController]
    [Route("api/comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddComment(int kullaniciId, int gonderiId, string yorum)
        {
            var success = await _commentService.AddCommentAsync(kullaniciId, gonderiId, yorum);

            return success
                ? Ok(new { message = "Yorum eklendi" })
                : BadRequest(new { error = "Yorum eklenirken bir hata oluştu." });
        }

        [HttpGet("{gonderiId}")]
        public async Task<IActionResult> GetComments([FromRoute] int gonderiId)
        {
            var comments = await _commentService.GetCommentsAsync(gonderiId);
            return Ok(comments);
        }
    }
}