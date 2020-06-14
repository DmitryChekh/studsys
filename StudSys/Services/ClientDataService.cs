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
    public class ClientDataService : IClientDataService
    {

        private readonly DataContext _dataContext;
        private readonly UserManager<UserModel> _userManager;

        public ClientDataService(DataContext dataContext, UserManager<UserModel> userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
        }
        public async Task<SimpleResponseModel> ChangeGroupToUser(string username, int groupid)
        {
            var existingGroup = await _dataContext.Groups.FirstOrDefaultAsync(u => u.Id == groupid).ConfigureAwait(false);

            if(existingGroup == null)
            {
                return new SimpleResponseModel { Success = false, ErrorsMessages = new[] { "Invalid group id" } };
            }

            var existingUser = await _userManager.FindByNameAsync(username).ConfigureAwait(false);

            if(existingUser == null)
            {
                return new SimpleResponseModel { Success = false, ErrorsMessages = new[] { "Invalid username" } };
            }

            existingUser.StudGroupId = groupid;

            var updateUser = await _userManager.UpdateAsync(existingUser).ConfigureAwait(false);
            
            if(updateUser == null)
            {
                return new SimpleResponseModel { Success = false, ErrorsMessages = new[] { "Update wrong" } };
            }

            return new SimpleResponseModel { Success = true };

        }
    }
}
