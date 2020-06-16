using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudSys.Contracts.Responses;
using StudSys.Models;


namespace StudSys.Services.Interfaces
{
    public interface IRoleService
    {
        public Task<SimpleResponseModel> CreateRole(string rolename);

        public Task<EnumerableResponseModel> GetAllRoles();

        public Task<SimpleResponseModel> DeleteRole(string rolename);

        public Task<SimpleResponseModel> ChangeUserRole(string username, string role);

    }
}
