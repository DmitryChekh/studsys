using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudSys.Models;

namespace StudSys.Services.Interfaces
{
    public interface ILessonService
    {
        public Task<SimpleResponseModel> CreateSubjectLesson(int subjectId, int groupId, long date, int typeid);

    }
}
