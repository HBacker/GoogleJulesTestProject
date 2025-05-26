using AutoMapper;
using EDMN_EgitimPortali_WebAPI.DTOs.Authentication;
using EDMN_EgitimPortali_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options; 
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EDMN_EgitimPortali_WebAPI.DTOs.Authentication;

namespace EDMN_EgitimPortali_WebAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IOptions<JWT> _jwtOptions;
        private readonly IMapper _mapper;

        public UsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JWT> jwtOptions, IMapper mapper) // Yapıcı metodu güncelleyin
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtOptions = jwtOptions; 
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                UserType = model.UserType
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new RegisterResponseDTO { Message = "Kullanıcı başarıyla kaydedildi." });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(model.UsernameOrEmail) ?? await _userManager.FindByEmailAsync(model.UsernameOrEmail);

            if (user == null)
            {
                return Unauthorized(new { Message = "Kullanıcı adı veya e-posta hatalı." });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var token = GenerateJwtToken(user);
                return Ok(new LoginResponseDTO { Token = token });
            }

            return Unauthorized(new { Message = "Şifre hatalı." });
        }

        [Authorize]
        [HttpGet("current-user")]
        public async Task<IActionResult> CurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var currentUserResponse = _mapper.Map<CurrentUserResponseDTO>(user);
            return Ok(currentUserResponse);
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserType", user.UserType)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddMinutes(_jwtOptions.Value.DurationInMinutes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiry,
                SigningCredentials = creds,
                Issuer = _jwtOptions.Value.Issuer,
                Audience = _jwtOptions.Value.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-role")]
        public async Task<IActionResult> UpdateUserType([FromBody] UserRoleUpdateDTO request)
        {
            var validUserTypes = new[] { "Admin", "Educator", "Student" };

            if (!validUserTypes.Contains(request.NewUserRole))
            {
                return BadRequest(new { Message = "Geçersiz kullanıcı tipi. Sadece: Admin, Educator, Student olabilir." });
            }

            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                return NotFound(new { Message = "Kullanıcı bulunamadı." });
            }

            user.UserType = request.NewUserRole;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return StatusCode(500, new { Message = "Kullanıcı tipi güncellenemedi.", Errors = result.Errors });
            }

            return Ok(new { Message = "Kullanıcı tipi başarıyla güncellendi." });
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _userManager.Users.ToList();

            var userDtos = _mapper.Map<List<CurrentUserResponseDTO>>(users);

            return Ok(userDtos);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(new { Message = "Kullanıcı bulunamadı." });
            }

            var userDto = _mapper.Map<CurrentUserResponseDTO>(user);

            return Ok(userDto);
        }

    }
}