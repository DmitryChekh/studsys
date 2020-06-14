using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudSys.Contracts.Requests
{
    public class GetLessonVisitsRequest
    {
        public int GroupId { get; set; }
        public int SubjectId { get; set; }
        public double DateTime { get; set; }
    }
}
