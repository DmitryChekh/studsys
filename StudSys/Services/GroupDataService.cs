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
    public class GroupDataService : IGroupDataService
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<UserModel> _userManager;

        public GroupDataService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<SimpleResponseModel> CreateGroup(string groupname, int monitorid, int courseleaderid)
        {

            //var existingGroup = await _dataContext.Groups.FirstOrDefaultAsync(g => g.GroupName == groupname);

            //if (existingGroup != null)
            //{
            //    return new SimpleResponseModel { Success = false, ErrorsMessages = new[] { "Group with this name already exist" } };
            //}

            var group = new GroupModel
            {
                GroupName = groupname,
                CourseLeaderId = courseleaderid,
                StudMonitorId = monitorid
            };

     
            var createdGroup = await _dataContext.Groups.AddAsync(group);


            if (createdGroup.State != EntityState.Added)
            {
                return new SimpleResponseModel { Success = false, ErrorsMessages = new[] { "Something wrong. Group is not added." } };
            }

            await _dataContext.SaveChangesAsync();

            return new SimpleResponseModel { Success = true };

        }
    }
}
