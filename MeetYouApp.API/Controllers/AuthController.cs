using Microsoft.AspNetCore.Mvc;
using MeetYouApp.API.Data.Interface;
using System.Threading.Tasks;
using MeetYouApp.API.Models;
using MeetYouApp.API.Dtos;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace MeetYouApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthRepository _authRepository;
        private IConfiguration _config;

        public AuthController(IAuthRepository authRepository, IConfiguration config)
        {
            _authRepository = authRepository;
            _config = config;
        }
        

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if(await _authRepository.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already exists");

            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
                
            };

            var createdUser = await _authRepository.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {

            //throw new Exception("I Said NO!");
            var userToLogin = await _authRepository.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if(userToLogin == null)
                return Unauthorized();

            // var claims = new []
            // {
            //     new Claim(ClaimTypes.NameIdentifier, userToLogin.Id.ToString()),
            //     new Claim(ClaimTypes.Name, userToLogin.Username)
            // };

            // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            // var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // var tokenDescriptor = new SecurityTokenDescriptor
            // {
            //     Subject = new ClaimsIdentity(claims),
            //     Expires = DateTime.Now.AddDays(1),
            //     SigningCredentials = credentials
            // };

            var tokenDescriptor = _authRepository.GenerateToken(userToLogin);
            
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });

        }
    }
}