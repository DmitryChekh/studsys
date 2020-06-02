using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudSys.Models
{
    public class GroupModel
    {
        public string Id { get; set; }
        public int StudMonitorId { get; set; }
        public int CourseLeaderId { get; set; }

        public ICollection<UserModel> Members { get; set; }
    }
}
