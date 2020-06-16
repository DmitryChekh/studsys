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


namespace StudSys.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<UserModel> _userManager;


        public RoleService(UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<SimpleResponseModel> CreateRole(string rolename)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(rolename)).ConfigureAwait(false);

            if (!result.Succeeded)
            {
                ErrorsResponse(result);
            }
                
            return new SimpleResponseModel { Success = true };
        }

        public async Task<SimpleResponseModel> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id).ConfigureAwait(false);

            var result = await _roleManager.DeleteAsync(role).ConfigureAwait(false);

            if (!result.Succeeded)
            {
                ErrorsResponse(result);
            }

            return new SimpleResponseModel { Success = true };
        }

        public async Task<EnumerableResponseModel> GetAllRoles()
        {
            var result = await _roleManager.Roles.Select(x => new RolesResponseModel
            {
                Id = x.Id,
                Name = x.Name
            }
            ).ToListAsync().ConfigureAwait(false);

            return new EnumerableResponseModel { Entities = result, Success = true };
        }

        public async Task<SimpleResponseModel> ChangeUserRole(string username, string role)
        {
            var existingUser = await _userManager.FindByNameAsync(username).ConfigureAwait(false);

            if (existingUser == null)
            {
                return new SimpleResponseModel { Success = false, ErrorsMessages = new[] { "User doesn't exist" } };
            }

            var existingRole = await _roleManager.FindByNameAsync(role).ConfigureAwait(false);

            if (existingRole == null)
            {
                return new SimpleResponseModel { Success = false, ErrorsMessages = new[] { "Invalid role" } };
            }

            var result = await _userManager.AddToRoleAsync(existingUser, role).ConfigureAwait(false);

            if (!result.Succeeded)
            {
                ErrorsResponse(result);
            }

            return new SimpleResponseModel { Success = true };

        }
        private SimpleResponseModel ErrorsResponse(IdentityResult result)
        {
            var response = new SimpleResponseModel { Success = false };
            List<string> errors = new List<string>();
            foreach (IdentityError error in result.Errors)
                errors.Add(error.Description);

            response.ErrorsMessages = errors;

            return response;
        }

    }
}
