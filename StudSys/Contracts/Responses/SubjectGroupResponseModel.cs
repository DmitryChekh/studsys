using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudSys.Contracts.Responses
{
    public class SubjectGroupResponseModel : IResponseModel
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
    }
}
