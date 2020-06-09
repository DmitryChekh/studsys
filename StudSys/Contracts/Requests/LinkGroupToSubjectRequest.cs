using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudSys.Contracts.Requests
{
    public class LinkGroupToSubjectRequest
    {
        public int Subjectid { get; set; }
        public int Groupid { get; set; }
    }
}
