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

namespace StudSys.Services
{
    public class SubjectService: ISubjectService
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<UserModel> _userManager;

        public SubjectService(DataContext dataContext, UserManager<UserModel> userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
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
    }
}
