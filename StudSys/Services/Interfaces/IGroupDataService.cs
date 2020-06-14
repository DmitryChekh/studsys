using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudSys.Contracts.Responses;
using StudSys.Models;


namespace StudSys.Services.Interfaces
{
    public interface IGroupDataService
    {
        public Task<SimpleResponseModel> CreateGroup(string groupname, string monitorUsername, string courseleaderUsername);

        public Task<EnumerableResponseModel> GetAllMembers(string groupname);

        public Task<EnumerableResponseModel> GetAllSubjectGroup(int groupid);

    }
}
