using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudSys.Contracts.Responses
{
    public class RolesResponseModel : IResponseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
