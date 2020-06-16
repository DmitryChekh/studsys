using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudSys.Contracts.Responses;
using StudSys.Models;

namespace StudSys.Services.Interfaces
{
    public interface IIdentityService
    {

        public Task<SimpleResponseModel> RegisterAsync(string email, string userName, string firstName, string secondName, 
            string middleName, string password, int groupid);
        public Task<AuthResultModel> LoginAsync(string username, string password);
    }
}
