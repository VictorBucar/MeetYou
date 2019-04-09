using System;
using System.Linq;
using System.Threading.Tasks;
using MeetYouApp.API.Data.Interface;
using MeetYouApp.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using MeetYouApp.API.Dtos;

namespace MeetYouApp.API.Data.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private DataContext _context;
        private IConfiguration _config;

        public AuthRepository(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

            if(user == null)
                return null;

            if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for(int i = 0; i < computedHash.Length; i++)
                {
                    if(computedHash[i] != passwordHash[i])
                        return false;
                }
            }

            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(x => x.Username == username))
                return true;
            return false;
        }

        public SecurityTokenDescriptor GenerateToken(User user)
        {
           var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

           var claims = new[]
           {
             new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
             new Claim(ClaimTypes.Name, user.Username)
           };

           var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

           return new SecurityTokenDescriptor 
           {
             Subject = new ClaimsIdentity(claims),
             Expires = DateTime.Now.AddDays(1),
             SigningCredentials = creds
           };

          //return tokenDescriptor;
        }
    }
}