using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using EDMN_EgitimPortali_WebAPI.DTOs.Comments; 
using EDMN_EgitimPortali_WebAPI.Models;
using EDMN_EgitimPortali_WebAPI.Repositories;
using System.Collections.Generic;

namespace EDMN_EgitimPortali_WebAPI.Controllers
{
    [ApiController]
    [Route("api/courses/{courseId}/comments")] 
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsRepository _commentsRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentsController(ICommentsRepository commentsRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _commentsRepository = commentsRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet] 
        [AllowAnonymous]
        public async Task<IActionResult> GetComments(int courseId)
        {
            var comments = await _commentsRepository.GetCommentsByCourseId(courseId);
            var commentDtos = _mapper.Map<List<CommentDTO>>(comments); 
            return Ok(commentDtos);
        }


        [HttpGet("{commentId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetComment(int courseId, int commentId)
        {
            var comment = await _commentsRepository.GetCommentWithUser(commentId);
            if (comment == null || comment.CourseId != courseId)
            {
                return NotFound("Yorum bulunamadı.");
            }
            var commentDto = _mapper.Map<CommentDTO>(comment);
            return Ok(commentDto);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(int courseId, [FromBody] CommentCreateDTO commentCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user == null)
            {
                return Unauthorized("Kullanıcı bulunamadı.");
            }

            var comment = _mapper.Map<Comment>(commentCreateDto);
            comment.CourseId = courseId;
            comment.UserId = user.Id; 
            comment.CreatedAt = DateTime.UtcNow;

            await _commentsRepository.AddAsync(comment);

            var commentDto = _mapper.Map<CommentDTO>(comment);
            return CreatedAtAction(nameof(GetComment), new { courseId, commentId = comment.Id }, commentDto);
        }

        [HttpDelete("{commentId}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int courseId, int commentId)
        {
            var comment = await _commentsRepository.GetByIdAsync(commentId);
            if (comment == null || comment.CourseId != courseId)
            {
                return NotFound("Yorum bulunamadı.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRoles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            if (comment.UserId == userId || userRoles.Contains("Admin"))
            {
                await _commentsRepository.DeleteAsync(commentId);
                return NoContent();
            }

            return Forbid("Bu yorumu silmeye yetkiniz yok.");
        }
    }
}
