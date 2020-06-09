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
    public class LessonService : ILessonService
    {
        private readonly DataContext _dataContext;
        public LessonService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<SimpleResponseModel> CreateSubjectLesson(int subjectId, int groupId, long date, int typeid)
        {
            var dateTime = UnixTimeStampToDateTime(date);

            
            var subjectgroup = await _dataContext.SubjectGroup.
                FirstOrDefaultAsync(x => x.GroupId == groupId && x.SubjectId == subjectId).ConfigureAwait(false);

            if(subjectgroup == null)
            {
                return new SimpleResponseModel { Success = false, ErrorsMessages = new[] { "This subjectGroup doesn't exist" } };
            }

            var existingSubjectLesson = await _dataContext.SubjectLesson.
                FirstOrDefaultAsync(x => x.Date == dateTime && x.SubjectGroupId == subjectgroup.Id).ConfigureAwait(false);

            if (existingSubjectLesson != null)
            {
                return new SimpleResponseModel { Success = false, ErrorsMessages = new[] { "It is also exist" } };
            }

            var subjectlesson = new SubjectLessonModel
            {
                SubjectGroupId = subjectgroup.Id,
                Date = dateTime,
                TypeId = typeid
            };

            try
            {
                var createdSubjectLesson = await _dataContext.AddAsync(subjectlesson).ConfigureAwait(false);
                await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch
            {
                return new SimpleResponseModel { Success = false, ErrorsMessages = new[] { "Invalid TypeId" } };
            }



            return new SimpleResponseModel { Success = true };
        }

        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
