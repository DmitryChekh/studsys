using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudSys.Contracts.Responses
{
    public class AuthResultModel
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public string UserFirstName { get; set; }
        public string UserMiddleName { get; set; }
        public string UserLastName { get; set; }
        public byte[] AvatarImage { get; set; }
        public IList<string> UserRole { get; set; }
        public string Username { get; set; }

        public IEnumerable<string> ErrorsMessages { get; set; }
    }
}
