using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudSys.Contracts.Requests
{
    public class ChangeUserRoleRequest
    {
        public string UserName { get; set; }
        public string Role { get; set; }
    }
}
