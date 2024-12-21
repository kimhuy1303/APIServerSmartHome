using APIServerSmartHome.DTOs;
using APIServerSmartHome.Entities;
using APIServerSmartHome.Services;
using APIServerSmartHome.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace APIServerSmartHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtService _jwtService;
        public AuthController(IUnitOfWork unitOfWork, JwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO request)
        {
            var _user = await _unitOfWork.Users.GetUserByUsername(request.Username);
            if (_user == null || !_unitOfWork.Users.VerifyPassword(_user, request.Password)) 
            {
                return BadRequest(new { message = "Username or password is invalid!" }); 
            }
            var token = _jwtService.generateJwtAccessToken(_user.Id, _user.Username!);
            return Ok(new {accessToken = token});
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterDTO request)
        {
            var isExist = await _unitOfWork.Users.GetUserByUsername(request.Username);
            if (isExist != null) return BadRequest(new { message = "Username already existed!" });
            if (!request.Password.Equals(request.ConfirmPassword)) { return BadRequest(new { message = "Passwords do not match!" }); }
            var newUser = new User
            {
                Username = request.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };
            await _unitOfWork.Users.AddAsync(newUser);
            return Ok(new { message = "Register successfully!", user = newUser });
        }
    }
}
