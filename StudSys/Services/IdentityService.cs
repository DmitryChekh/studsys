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
using StudSys.Models.DbModels;
using StudSys.Contracts.Responses;
using Newtonsoft.Json;

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

        public async Task<SimpleResponseModel> RegisterAsync(string email, string userName,  string firstName, string lastName, 
            string middleName, string password, int groupid)
        {
            var existingEmail = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
            var existingUsername = await _userManager.FindByNameAsync(userName).ConfigureAwait(false);

            if(existingEmail != null)
            {
                return new SimpleResponseModel { Success = false, ErrorsMessages = new[] { "User with this email exist" } };
            }

            if(existingUsername != null)
            {
                return new SimpleResponseModel { Success = false, ErrorsMessages = new[] { "User with this username exist" } };
            }

            var newUser = new UserModel
            {
                Email = email,
                UserName = userName,
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                StudGroupId = groupid
            };

            var createdUser = await _userManager.CreateAsync(newUser, password).ConfigureAwait(false);

            await _userManager.AddToRoleAsync(newUser, "student");

            if(!createdUser.Succeeded)
            {
                return new SimpleResponseModel { Success = false, ErrorsMessages = createdUser.Errors.Select(x => x.Description) };
            }

            return new SimpleResponseModel { Success = true };
        }


        public async Task<AuthResultModel> LoginAsync(string username, string password)
        {
            var existingUser = await _dataContext.Users.SingleOrDefaultAsync(u => u.UserName == username).ConfigureAwait(false);

            if (existingUser != null && await _userManager.CheckPasswordAsync(existingUser, password).ConfigureAwait(false))
            {
                return await GenerateAuthResultForUser(existingUser);
            }
            else
            {
                return new AuthResultModel { Success = false, ErrorsMessages = new[] { "Email/password incorrect" } };
            }
        }


        // TODO: Решить проблему с Roles
        private async Task<AuthResultModel> GenerateAuthResultForUser(UserModel userModel)
        {
            IdentityOptions _options = new IdentityOptions();
            var roles = await _userManager.GetRolesAsync(userModel);

            var jsRoles = JsonConvert.SerializeObject(roles);

            var key = JwtOptions.GetSymmetricSecurityKey();
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserName", userModel.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, userModel.Email),
                    new Claim("id", userModel.Id),
                }),
               
                Expires = DateTime.UtcNow.AddHours(2),

                SigningCredentials = signingCredentials
            };

            foreach (var role in roles)
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
            }


            var token = tokenHandler.CreateToken(tokenDescriptor);


            return new AuthResultModel
            {
                Token = tokenHandler.WriteToken(token),
                Success = true,
                UserFirstName = userModel.FirstName,
                UserLastName = userModel.LastName,
                UserMiddleName = userModel.MiddleName,
                AvatarImage = userModel.AvatarImage,
                UserRole = roles,
                Username = userModel.UserName,

            };

        }
    }
}
