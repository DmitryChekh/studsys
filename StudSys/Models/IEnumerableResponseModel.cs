using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudSys.Models
{
    public class IEnumerableResponseModel
    {
        public IEnumerable<MembersOfGroupResponseModel> entities { get; set; }

        public bool Success { get; set; }
    }
}
