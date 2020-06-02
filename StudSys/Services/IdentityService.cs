using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudSys.Data;
using StudSys.Models;
using StudSys.Services.Interfaces;
using StudSys.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudSys.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<UserModel> _userManager;

        public IdentityService(UserManager<UserModel> userManager, DataContext dataContext)
        {
            _userManager = userManager;
            _dataContext = dataContext;
        }

        public async Task<SimpleResponseModel> RegisterAsync(string email, string firstName, string secondName, 
            string middleName, string password, string role)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if(existingUser != null)
            {
                return new SimpleResponseModel { Success = false, ErrorsMessages = new[] { "User with this email exist" } };
            }

            var userName = email.Substring(0, email.LastIndexOf('@'));

            var newUser = new UserModel
            {
                Email = email,
                UserName = userName,
                Role = role,
                FirstName = firstName,
                MiddleName = middleName,
                SecondName = secondName,
            };

            var createdUser = await _userManager.CreateAsync(newUser, password);

            if(!createdUser.Succeeded)
            {
                return new SimpleResponseModel { Success = false, ErrorsMessages = createdUser.Errors.Select(x => x.Description) };
            }

            return new SimpleResponseModel { Success = true };
        }


        public async Task<AuthResultModel> LoginAsync(string username, string password)
        {
            var existingUser = await _dataContext.Users.SingleOrDefaultAsync(u => u.UserName == username);

            if (existingUser != null && await _userManager.CheckPasswordAsync(existingUser, password))
            {
                return GenerateAuthResultForUser(existingUser);
            }
            else
            {
                return new AuthResultModel { Success = false, ErrorsMessages = new[] { "Email/password incorrect" } };
            }
        }



        private AuthResultModel GenerateAuthResultForUser(UserModel userModel)
        {
            var key = JwtOptions.GetSymmetricSecurityKey();

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                        new Claim("UserName", userModel.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, userModel.Email),
                        new Claim("Role", userModel.Role),
                        new Claim("id", userModel.Id),
                    }),
                Expires = DateTime.UtcNow.AddHours(2),

                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthResultModel
            {
                Token = tokenHandler.WriteToken(token),
                Success = true,
                UserFirstName = userModel.FirstName,
                UserSecondName = userModel.SecondName,
                UserMiddleName = userModel.MiddleName,
                AvatarImage = userModel.AvatarImage,
                UserRole = userModel.Role,
                Username = userModel.UserName,
            };

        }
    }
}
