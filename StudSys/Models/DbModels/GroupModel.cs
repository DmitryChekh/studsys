using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudSys.Models
{
    public class GroupModel
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string StudMonitorId { get; set; }
        public string CourseLeaderId { get; set; }

        //public ICollection<UserModel> Members { get; set; }
    }
}
