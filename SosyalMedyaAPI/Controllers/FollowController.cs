using Microsoft.AspNetCore.Mvc;
using SosyalMedyaAPI.Services.Interface;


namespace SosyalMedyaAPI.Controllers
{
    [ApiController]
    [Route("api/follow")]
    public class FollowController : ControllerBase
    {
        private readonly IFollowService _followService;

        public FollowController(IFollowService followService)
        {
            _followService = followService;
        }

        public record FollowRequestDto(int TakipEdenId, int TakipEdilenId);

        [HttpPost]
        public async Task<IActionResult> Follow([FromBody] FollowRequestDto dto)
        {
            var result = await _followService.FollowAsync(dto.TakipEdenId, dto.TakipEdilenId);

            return result.Success
                ? Ok(new { message = result.Message, data = result.Data })
                : BadRequest(new { error = result.Message });
        }

        [HttpDelete("eden/{takipEdenId}/edilen/{takipEdilenId}")]
        public async Task<IActionResult> Unfollow([FromRoute] int takipEdenId, [FromRoute] int takipEdilenId)
        {
            var success = await _followService.UnfollowAsync(takipEdenId, takipEdilenId);

            return success
                ? Ok(new { message = "Takip başarıyla bırakıldı" })
                : BadRequest(new { error = "Takip bırakılamadı." });
        }

        [HttpGet("{userId}/followers")]
        public async Task<IActionResult> GetFollowers([FromRoute] int userId)
        {
            var followers = await _followService.GetFollowersAsync(userId);
            return Ok(followers);
        }

        [HttpGet("{userId}/following")]
        public async Task<IActionResult> GetFollowing([FromRoute] int userId)
        {
            var following = await _followService.GetFollowingAsync(userId);
            return Ok(following);
        }
    }
}