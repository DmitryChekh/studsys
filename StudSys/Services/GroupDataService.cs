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
using Microsoft.AspNetCore.Authorization;

namespace StudSys.Services
{
    public class GroupDataService : IGroupDataService
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<UserModel> _userManager;

        public GroupDataService(DataContext dataContext, UserManager<UserModel> userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
        }

        //TODO: Сделать возможность сделать monitorID, courseleaderID нуллябельными


        public async Task<SimpleResponseModel> CreateGroup(string groupname, string monitorUsername, string courseleadUsername)
        { 
            var existingGroup = await _dataContext.Groups.FirstOrDefaultAsync(g => g.GroupName == groupname).ConfigureAwait(false);

            if (existingGroup != null)
            {
                return new SimpleResponseModel { Success = false, ErrorsMessages = new[] { "Group with this name already exist" } };
            }

            var courseleader = await _userManager.FindByNameAsync(courseleadUsername).ConfigureAwait(false);
            var monitor = await _userManager.FindByNameAsync(monitorUsername).ConfigureAwait(false);

            if (courseleader == null && monitor == null)
            {
                return new SimpleResponseModel { Success = false, ErrorsMessages = new[] { "Invalid monitorUsername or courseleadUsername" } };
            }

            var group = new GroupModel
            {
                GroupName = groupname,
                CourseLeaderId = courseleader.Id,
                StudMonitorId= monitor.Id
            };

     
            var createdGroup = await _dataContext.Groups.AddAsync(group).ConfigureAwait(false);


            if (createdGroup.State != EntityState.Added)
            {
                return new SimpleResponseModel { Success = false, ErrorsMessages = new[] { "Something wrong. Group is not added." } };
            }

            await _dataContext.SaveChangesAsync().ConfigureAwait(false);


            return new SimpleResponseModel { Success = true };
        }


        public async Task<EnumerableResponseModel> GetAllMembers(string groupname)
        {
            var existingGroup = await _dataContext.Groups.FirstOrDefaultAsync(g => g.GroupName == groupname).ConfigureAwait(false);

            if (existingGroup == null)
            {
                return new EnumerableResponseModel { Success = false };
            }


            var membersList = await _userManager.Users.Where(u => u.StudGroupId == existingGroup.Id).Select(u => new MembersOfGroupResponseModel
            {
                UserFirstName = u.FirstName,
                UserMiddleName = u.MiddleName,
                UserLastName = u.LastName
            })
            .ToListAsync().ConfigureAwait(false);

            if (membersList == null)
            {
                return new EnumerableResponseModel { Success = false };
            }

            return new EnumerableResponseModel { Entities = membersList , Success = true};

        }

        // TODO: Попробовать написать linq запрос через джойн
        public async Task<EnumerableResponseModel> GetAllSubjectGroup(int groupid)
        {

            var subjectGroupList = await (from subjectGroup in _dataContext.SubjectGroup
                                          join subject in _dataContext.Subject on subjectGroup.SubjectId equals subject.Id
                                          where subjectGroup.GroupId == groupid
                                          select new SubjectGroupResponseModel
                                          {
                                              Id = subjectGroup.SubjectId,
                                              SubjectName = subject.SubjectName
                                          }).ToListAsync().ConfigureAwait(false);


            if (subjectGroupList == null)
            {
                return new EnumerableResponseModel { Success = false };
            }

            return new EnumerableResponseModel { Entities = subjectGroupList, Success = true };
        }

    }
}
