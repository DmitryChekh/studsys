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
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Data.SqlClient;
using StudSys.Contracts.Responses;

namespace StudSys.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly DataContext _dataContext;

        public SubjectService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<SimpleResponseModel> CreateSubject(string subjectname)
        {
            var subjectExisting = await _dataContext.Subject.FirstOrDefaultAsync(s => s.SubjectName == subjectname).ConfigureAwait(false);

            if (subjectExisting != null)
            {
                return new SimpleResponseModel { Success = false, ErrorsMessages = new[] { "Subject with this name already exist" } };
            }


            var subject = new SubjectModel
            {
                SubjectName = subjectname
            };

            var createdSubject = await _dataContext.Subject.AddAsync(subject).ConfigureAwait(false);


            if (createdSubject.State != EntityState.Added)
            {
                return new SimpleResponseModel { Success = false, ErrorsMessages = new[] { "Something wrong. Subject is not added." } };
            }

            await _dataContext.SaveChangesAsync().ConfigureAwait(false);


            return new SimpleResponseModel { Success = true };
        }


        //TODO: решить проблему с отловом SqlException
        public async Task<SimpleResponseModel> LinkGroupToSubject(int groupid, int subjectid)
        {
            var existingLink = _dataContext.SubjectGroup.Where(x => x.GroupId == groupid && x.SubjectId == subjectid);

            if(existingLink != null)
            {
                return new SimpleResponseModel { Success = false, ErrorsMessages = new[] { "This link already exist" } };
            }

            var subjectgroup = new SubjectGroupModel
            {
                SubjectId = subjectid,
                GroupId = groupid
            };

            try
            {
                await _dataContext.SubjectGroup.AddAsync(subjectgroup).ConfigureAwait(false);
                await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch
            {
                return new SimpleResponseModel { Success = false, ErrorsMessages = new[] { "Invalid subject ID or group ID" } };
            }



            return new SimpleResponseModel { Success = true };
        }



    }
}
