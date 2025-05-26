using AutoMapper;
using EDMN_EgitimPortali_WebAPI.DTOs.WatchedContents;
using EDMN_EgitimPortali_WebAPI.Models;
using EDMN_EgitimPortali_WebAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EDMN_EgitimPortali_WebAPI.Controllers
{
    [Route("api/watched-contents")]
    [ApiController]
    [Authorize]
    public class WatchedContentsController : ControllerBase
    {
        private readonly IWatchedContentsRepository _watchedContentsRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public WatchedContentsController(IWatchedContentsRepository watchedContentsRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _watchedContentsRepository = watchedContentsRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddWatchedContent([FromBody] WatchedContentDTO watchedContentDTO)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var existing = await _watchedContentsRepository.GetWatchedContentAsync(user.Id, watchedContentDTO.LessonId);
            if (existing != null)
            {
                return BadRequest("Bu içerik zaten izlendi olarak işaretlenmiş.");
            }

            var watchedContent = new WatchedContent
            {
                UserId = user.Id,
                LessonId = watchedContentDTO.LessonId,
                WatchedDate = System.DateTime.UtcNow
            };

            await _watchedContentsRepository.AddAsync(watchedContent);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveWatchedContent([FromBody] WatchedContentDTO watchedContentDTO)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var existing = await _watchedContentsRepository.GetWatchedContentAsync(user.Id, watchedContentDTO.LessonId);
            if (existing == null)
            {
                return NotFound();
            }

            await _watchedContentsRepository.DeleteAsync(existing.Id);
            return Ok();
        }
    }
}
