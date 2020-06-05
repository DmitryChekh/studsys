using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudSys.Models;

namespace StudSys.Services.Interfaces
{
    public interface IClientDataService
    {

        public Task<SimpleResponseModel> ChangeGroupToUser(string username, int groupid);


    }
}
