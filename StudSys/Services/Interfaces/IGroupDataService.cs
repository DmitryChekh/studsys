using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudSys.Models;


namespace StudSys.Services.Interfaces
{
    public interface IGroupDataService
    {
        public Task<SimpleResponseModel> CreateGroup(string groupname, int monitorid, int courseleaderid);

        public Task<IEnumerable<MembersOfGroupResponseModel>> GetAllMembers(string groupname);

    }
}
