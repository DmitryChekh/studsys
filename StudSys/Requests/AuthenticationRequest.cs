using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StudSys.Requests
{
    public class AuthenticationRequest
    {
        public string Name { get; set; }
        public string Password { get; set; }

    }
}
