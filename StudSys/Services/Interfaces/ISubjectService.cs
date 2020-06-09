using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudSys.Models;


namespace StudSys.Services.Interfaces
{
    public interface ISubjectService
    {
        public Task<SimpleResponseModel> CreateSubject(string subjectname);

        public Task<SimpleResponseModel> LinkGroupToSubject(int groupid, int subjectid);

    }
}
