using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudSys.Contracts.Requests
{
    public class CreateSubjectLessonRequest
    {
        public int SubjectId { get; set; }
        public int GroupId { get; set; }
        public int utxDateTime { get; set; }

        public int TypeId { get; set; }

    }
}
