using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRoutes 
{
    public static class Identity
    {
        public const string Login = "api/identity/login";
        public const string Register = "api/identity/registration";
        public const string GetUserInfo = "api/{username}/info";
        public const string CheckJWT = "api/identity/auth";
    }

    public static class Test
    {
        public const string Text = "api/test/getinfo";

    }

    public static class Group
    {
        public const string CreateGroup = "api/group/create";
    }
}
