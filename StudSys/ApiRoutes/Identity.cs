using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudSys.ApiRoutes 
{
    public static class Identity
    {
        public const string Login = "api/identity/login";
        public const string Register = "api/identity/registration";
        public const string GetUserInfo = "api/{username}/info";
        public const string CheckJWT = "api/identity/auth";
    }
}
