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
    public class LessonService : ILessonService
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<UserModel> _userManager;
        public LessonService(DataContext dataContext, UserManager<UserModel> userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
        }

        public async Task<SimpleResponseModel> CreateSubjectLesson(int subjectId, int groupId, double date, int typeid)
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

            var createdSubjectLesson = await _dataContext.AddAsync(subjectlesson).ConfigureAwait(false);

            try
            {
                await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch
            {
                return new SimpleResponseModel { Success = false, ErrorsMessages = new[] { "Invalid TypeId" } };
            }

            var fillLessonVisit = await FillLessonVisit(subjectlesson, groupId).ConfigureAwait(false);
            if(!fillLessonVisit)
            {
                return new SimpleResponseModel { Success = false, ErrorsMessages = new[] { "Can't insert data in lesson visit" } };
            }

            return new SimpleResponseModel { Success = true };
        }

        private async Task<bool> FillLessonVisit(SubjectLessonModel subjectLessonModel, int groupid)
        {

            var lessonVisits = await _userManager.Users.Where(x => x.StudGroupId == groupid).
                   Select(x => new LessonVisitModel
                   {
                       LessonId = subjectLessonModel.Id,
                       UserId = x.Id,
                       Visited = false
                   }).ToListAsync().ConfigureAwait(false);

            try
            {
                await _dataContext.LessonVisits.AddRangeAsync(lessonVisits).ConfigureAwait(false);
                await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch
            {
                return false;
            }

            return true;

        }

        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        
        public async Task<EnumerableResponseModel> GetVisitsOfLesson(int subjectId, int groupId, double date)
        {
            var dateTime = UnixTimeStampToDateTime(date);

            var subjectGroupId = await _dataContext.SubjectGroup.
                Where(x => x.SubjectId == subjectId && x.GroupId == groupId).Select(x => x.Id).FirstOrDefaultAsync().ConfigureAwait(false);

            var members = await _userManager.Users.Where(x => x.StudGroupId == groupId).
                Select(x => new { x.FirstName, x.LastName, x.MiddleName }).FirstOrDefaultAsync().ConfigureAwait(false);

            if (subjectGroupId == 0)
            {
                return new EnumerableResponseModel { Success = false };
            }

            var subjectLesson = await _dataContext.SubjectLesson.
                FirstOrDefaultAsync(x => x.SubjectGroupId == subjectGroupId && x.Date == dateTime).ConfigureAwait(false);

            var visitList = await (from lessonVisit in _dataContext.LessonVisits
                                   join user in _userManager.Users on lessonVisit.UserId equals user.Id
                                   where lessonVisit.LessonId == subjectLesson.Id
                                   select new LessonVisitResponseModel
                                   {
                                       FirstName = user.FirstName,
                                       LastName = user.LastName,
                                       MiddleName = user.MiddleName,
                                       Visit = lessonVisit.Visited
                                   }).ToListAsync().ConfigureAwait(false);

            if(visitList.Count == 0)
            {
                return new EnumerableResponseModel { Success = false };
            }

            return new EnumerableResponseModel { Success = true, Entities = visitList };
        }
                                   
    }
}
