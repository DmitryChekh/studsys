using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudSys.Contracts.Responses;
using StudSys.Models;

namespace StudSys.Services.Interfaces
{
    public interface ILessonService
    {
        public Task<SimpleResponseModel> CreateSubjectLesson(int subjectId, int groupId, double datetime, int lessontypeid);

        public Task<EnumerableResponseModel> GetVisitsOfLesson(int subjectId, int groupId, double datetime); 

    }
}
