using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudSys.Contracts.Responses
{
    public class EnumerableResponseModel
    {
        public IEnumerable<IResponseModel> Entities { get; set; } 

        public bool Success { get; set; }
    }
}
