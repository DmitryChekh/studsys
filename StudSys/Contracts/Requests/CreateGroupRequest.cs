using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StudSys.Contracts.Requests
{
    public class CreateGroupRequest
    {
        public string GroupName {get;set;}

        public string MonitorUsername { get; set; }

        public string CourseLeadUsername { get; set; }
    }
}
